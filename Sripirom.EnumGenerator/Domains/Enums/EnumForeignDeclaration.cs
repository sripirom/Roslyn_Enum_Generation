namespace Sripirom.EnumGenerator.Domains.Enums
{
    using System.ComponentModel;

    [TableName("base.EnumForeignDeclaration", "ForeignDeclarationId")]
    public enum EnumForeignDeclaration
    {
        /// <summary>
        /// Unknown = 0
        /// </summary>
        [Description("Onbekend")]
        Unknown = 0,
        /// <summary>
        /// SentWgAdv = 1
        /// </summary>
        [Description("Test_Verstuurd aan WG/ADV")]
        SentWgAdv = 1,
        /// <summary>
        /// Received = 2
        /// </summary>
        [Description("Ontvangen")]
        Received = 2,
        /// <summary>
        /// SubmittedToLegal = 3
        /// </summary>
        [Description("Voorgelegd aan legal")]
        SubmittedToLegal = 3,
        /// <summary>
        /// Approved = 4
        /// </summary>
        [Description("Goedgekeurd")]
        Approved = 4,
        /// <summary>
        /// Rejected = 5
        /// </summary>
        [Description("Afgewezen")]
        Rejected = 5,
    }
}