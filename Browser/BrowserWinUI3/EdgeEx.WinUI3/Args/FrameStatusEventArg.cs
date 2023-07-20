using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.Args
{
    public class FrameStatusEventArg
    {
        public bool CanGoBack { get; set; }
        public bool CanGoForward { get; set; }
        public bool CanRefresh { get; set; }
        public string PersistenceId { get; set; }
        public FrameStatusEventArg(string persistenceId, bool canGoBack, bool canGoForward, bool canRefresh)
        {
            PersistenceId = persistenceId;
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
            CanRefresh = canRefresh;
        }
    }
}
