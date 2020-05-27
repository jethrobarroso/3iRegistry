using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace _3iRegistry.Core
{
    public enum Grant
    {
        [Description("None")]
        None,
        [Description("Child Support")]
        ChildSupport,
        [Description("Foster Child")]
        FosterChild,
        [Description("Disability")]
        Disability,
        [Description("Pension")]
        Pension,
        [Description("Coronavirus (Covid-19)")]
        Coronavirus,
        [Description("Care Dependency")]
        CareDependency
    }
}
