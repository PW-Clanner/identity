using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Pw.Clanner.Identity;
using Pw.Clanner.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddCors(options =>
{
    options.AddPolicy("enableAll", cors =>
    {
        cors.WithOrigins(
                "https://clanner.pw",
                "http://localhost:5175/",
                "http://localhost:5175",
                "http://localhost",
                "https://auth.clanner.pw"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Configuration.AddEnvironmentVariables("ENV_");

builder.Services.AddControllers(options => { options.Filters.Add<ApiFilter>(); });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();

builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var useOtlp = builder.Configuration.GetValue<bool>("Jaeger:Enabled");

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
            .AddOtlpExporter(o => { o.Endpoint = new Uri(builder.Configuration["Jaeger:Endpoint"]!); })
        )
        .WithMetrics(opts => opts
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("identity"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(o => { o.Endpoint = new Uri(builder.Configuration["Jaeger:Endpoint"]!); }));
}

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseHttpsRedirection();
app.UseSession();

app.UseCors("enableAll");
app.MapControllers();
app.UseHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();