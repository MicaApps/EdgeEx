using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.Args
{
    public class LoadingEventArg
    {
        public bool IsLoading { get; set; }
        public string Title { get; set; }
        public LoadingEventArg(bool isLoading, string title)
        {
            IsLoading = isLoading;
            Title = title;
        }
    }
}
