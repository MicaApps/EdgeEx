using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webbrowser_winui3.Models;
using Webbrowser_winui3.Services.Interface;

namespace Webbrowser_winui3.Services
{
    public class FavlistService : IFavlistService
    {
        public async Task<List<FavFileModel>> GetFavFileModels()
        {
            return await Task.Run(() =>{
                var ls = new List<FavFileModel>();
                for (int i = 0; i < 30; i++)
                {
                    var item = new FavFileModel();
                    item.FavName = "一级书签" + i;
                    item.Id = Guid.NewGuid();

                    var list = new List<FavFileModel>();
                    for (int j = 0; j < 10; j++)
                    {
                        var item2=new FavFileModel();
                        item2.FavName = "二级书签" + i;
                        item2.Id = Guid.NewGuid();
                        var list2 = new List<FavFileModel>();

                        for (int k = 0; k < 30; k++)
                        {
                            var item3=new FavFileModel();
                            item3.FavName = "百度";
                            item3.Url = "www.baidu.com";
                            list2.Add(item3);

                        }
                        item2.FavChildren = new System.Collections.ObjectModel.ObservableCollection<FavFileModel>(list2);
                        list.Add(item2);
                    }
                    item.FavChildren = new System.Collections.ObjectModel.ObservableCollection<FavFileModel>(list);
                    ls.Add(item);
                }


                return ls;
            });
            
        }
    }
}
