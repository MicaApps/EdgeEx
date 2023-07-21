using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace EdgeEx.WinUI3.Models
{
    public class BookMark
    {
        public BookMark() { }
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        public string Icon { get;set; }
        public string Title { get;set; }
        public string Description { get;set; }
        public string Url { get;set; }

        public DateTime CreateTime { get;set; }
        public string Screenshot { get; set; }
        public string FolderId { get; set; }
    }
}
