using System.ComponentModel;

namespace _3iRegistry.Core
{
    public enum MaritalStatus
    {
        [Description("Cohabiting")]
        Cohabit,
        [Description("Engaged")]
        Engaged,
        [Description("Married in community of property")]
        MarriedICOP,
        [Description("Married out of community of property")]
        MarriedOCOP,
    }
}