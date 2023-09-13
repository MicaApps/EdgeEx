using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace EdgeEx.WinUI3.Models
{
    /// <summary>
    /// Bookmark Orm
    /// </summary>
    public class Bookmark
    {
        public Bookmark() { }
        public bool IsFolder { get; set; }
        [SugarColumn(IsNullable = true)]
        public string Icon { get;set; }
        public string Title { get;set; }
        [SugarColumn(IsNullable = true)]
        public string Description { get;set; }
        [SugarColumn(IsPrimaryKey = true)]
        public string Uri { get;set; }
        public DateTime CreateTime { get;set; }
        [SugarColumn(IsNullable = true)]
        public DateTime LastModified { get;set; }
        [SugarColumn(IsNullable = true)]
        public string Screenshot { get; set; }
        [SugarColumn(DefaultValue = "default")]
        public string FolderId { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<Bookmark> Children { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string Host { 
            get
            {
                try
                {
                    Uri uri = new Uri(Uri);
                    return uri.Host;
                }catch (Exception)
                {
                    return "";
                }
            }
        }
    }
}
