using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.Args
{
    public class FrameStatusEventArg
    {
        public string PersistenceId { get; set; }
        public string TabItemName { get; set; }
        public bool CanGoBack { get; set; }
        public bool CanGoForward { get; set; }
        public bool CanRefresh { get; set; }
        
        public FrameStatusEventArg(string persistenceId, string tabItemName, bool canGoBack, bool canGoForward, bool canRefresh)
        {
            PersistenceId = persistenceId;
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
            CanRefresh = canRefresh;
            TabItemName = tabItemName;
        }
        public FrameStatusEventArg() { }
    }
}
