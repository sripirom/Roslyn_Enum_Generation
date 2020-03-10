namespace Sripirom.EnumGenerator.Domains.Enums
{
    using System.ComponentModel;

    [TableName("base.EnumOnboardingStatus", "OnboardingStatusId")]
    public enum EnumOnboardingStatus
    {
        /// <summary>
        /// Unknown = 0
        /// </summary>
        [Description("Onbekend")]
        Unknown = 0,
        /// <summary>
        /// Accepted = 1
        /// </summary>
        [Description("Test_Akkoord")]
        Accepted = 1,
        /// <summary>
        /// Signed = 2
        /// </summary>
        [Description("Handtekening")]
        Signed = 2,
        /// <summary>
        /// Complete = 3
        /// </summary>
        [Description("Compleet")]
        Complete = 3,
        /// <summary>
        /// Activated = 4
        /// </summary>
        [Description("Geactiveerd")]
        Activated = 4,
    }
}