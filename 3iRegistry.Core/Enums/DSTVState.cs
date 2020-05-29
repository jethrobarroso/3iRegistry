using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace _3iRegistry.Core
{
    public enum DSTVState
    {
        [Description("None")]
        None,
        [Description("New Install")]
        New_Install,
        [Description("Transferred")]
        Transferred
    }
}
