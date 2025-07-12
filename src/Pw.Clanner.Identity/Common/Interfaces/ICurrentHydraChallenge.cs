namespace Pw.Clanner.Identity.Common.Interfaces;

public interface ICurrentHydraChallenge
{
    string LoginChallenge { get; set; }
    
    string Subject { get; set; }
    
    string ConsentChallenge { get; set; }
}