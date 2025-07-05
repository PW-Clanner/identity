namespace Pw.Clanner.Identity.Common.Security;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class HydraChallengeAttribute : Attribute
{
    public bool NeedLoginChallenge { get; set; }

    public bool NeedConsentChallenge { get; set; }

    public HydraChallengeAttribute()
    {
    }

    public HydraChallengeAttribute(bool needLoginChallenge, bool needConsentChallenge)
    {
        NeedLoginChallenge = needLoginChallenge;
        NeedConsentChallenge = needConsentChallenge;
    }
}