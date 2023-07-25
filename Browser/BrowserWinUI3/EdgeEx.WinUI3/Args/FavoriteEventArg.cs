using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeEx.WinUI3.Args
{
    public class FavoriteEventArg
    {
        public string PersistenceId { get; set; }
        public string TabItemName { get; set; }
        public bool IsFavorite { get; set; }
        public string Uri { get; set; }
        public string Title { get; set; }
        public string FolderId { get; set; }
        public FavoriteEventArg(string persistenceId, string tabItemName, bool isFavorite, string uri, string title,string folderId)
        {
            PersistenceId = persistenceId;
            TabItemName = tabItemName;
            IsFavorite = isFavorite;
            Uri = uri;
            Title = title;
            FolderId = folderId;
        }
    }
}
