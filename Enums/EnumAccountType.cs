namespace EnumGenerator.Enums
{
    using System.ComponentModel;

    [TableName("dbo.EnumAccountType", "AccountTypeId")]
    public enum EnumAccountType
    {
        /// <summary>
        /// AccountNormal = 1
        /// </summary>
        [Description("Desc AccountNormal")]
        AccountNormal = 1,
        /// <summary>
        /// AccountNumber = 2
        /// </summary>
        [Description("Desc AccountNumber")]
        AccountNumber = 2,
        /// <summary>
        /// AccountSpecial = 3
        /// </summary>
        [Description("Desc AccountSpecial")]
        AccountSpecial = 3,
    }
}