namespace Pw.Clanner.Identity.Common.Interfaces;

public interface ICurrentHydraChallenge
{
    string LoginChallenge { get; set; }
    
    string ConsentChallenge { get; set; }
}