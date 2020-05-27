using _3iRegistry.Core;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3iRegistry.WPF.Messages
{
    public class UpdateListMessage { }

    public class PageTransitionMessage { }

    public class CloseDialogMessage { }

    public class DoneSignal { }

    public class SaveNewMessage { }

    public class SaveEditMessage { }

    public class DeleteMessage
    {
        public static object SyncContext { get; } = new object();
    }
}
