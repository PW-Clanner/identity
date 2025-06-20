using FluentValidation.Results;

namespace Pw.Clanner.Identity.Common.Exceptions;

public class AppValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public AppValidationException() :
        base("Одна или более проверок не пройдены")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public AppValidationException(IEnumerable<ValidationFailure> failures)
    {
        Errors = failures
            .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
            .ToDictionary(x => x.Key, x => x.ToArray());
    }
}