using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.Args
{
    
    public class LoadingProgressEventArg
    {
        public double Progress { get; set; }
        public LoadingProgressEventArg(double progress)
        {
            this.Progress = progress;
        }
    }
}
