using System.Text.Json.Serialization;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Pw.Clanner.Identity.Storage;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PwClanner")));

builder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer(options => { })
    .AddInMemoryClients([
        new Client
        {
            ClientId = "client",
            ClientSecrets = new List<Secret> { new("test") },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { "https://oidcdebugger.com/debug" },
            PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
            FrontChannelLogoutUri = "https://localhost:5002/signout-oidc",
            AllowedScopes = { "openid", "profile", "email", "phone" }
        }
    ])
    .AddInMemoryIdentityResources([
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email(),
        new IdentityResources.Phone(),
    ])
    .AddAspNetIdentity<IdentityUser>();

builder.Services.AddLogging(options => { options.AddFilter("Duende", LogLevel.Debug); });

var useOtlp = builder.Configuration.GetValue<bool>("App:Jaeger:Enabled");

if (useOtlp)
{
    builder.Services.AddOpenTelemetry()
        .WithTracing(opts => opts
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("identity"))
            .AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
                options.Filter = context => context.Request.Path.Value?.Contains("/health") == false;
            })
            .AddHttpClientInstrumentation(options =>
            {
                options.RecordException = true;
                options.FilterHttpWebRequest = request =>
                    request.RequestUri.PathAndQuery.Contains("/health") == false;
                options.FilterHttpRequestMessage = message =>
                    message.RequestUri?.PathAndQuery.Contains("/health") == false;
            })
            .AddOtlpExporter(o => { o.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]!); })
        )
        .WithMetrics(opts => opts
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("identity"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(o => { o.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]!); }));
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseIdentityServer();
app.UseAuthorization();

var sampleTodos = new Todo[]
{
    new(1, "Walk the dog"),
    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
};

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}