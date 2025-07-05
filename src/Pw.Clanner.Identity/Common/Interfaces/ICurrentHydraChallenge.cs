namespace Pw.Clanner.Identity.Common.Interfaces;

public interface ICurrentHydraChallenge
{
    string LoginChallenge { get; }
    
    string ConsentChallenge { get; }
}