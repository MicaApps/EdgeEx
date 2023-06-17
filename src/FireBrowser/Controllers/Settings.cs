using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBrowser.Controllers
{
    public class Settings
    {
        public enum NewTabLayout
        {
            Classic,
            Simple,
            Productive
        }
        public enum NewTabBackground
        {
            None,
            Custom,
            Featured //Bing for now, in the future Unsplash or our own service
        }

        public enum UILayout
        {
            Classic,
            Compact,
            Vertical
        }
    }
}
