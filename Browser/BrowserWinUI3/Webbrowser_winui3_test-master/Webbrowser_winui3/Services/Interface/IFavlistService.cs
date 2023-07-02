using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webbrowser_winui3.Models;

namespace Webbrowser_winui3.Services.Interface
{
    public interface IFavlistService
    {
        public Task<List<FavFileModel>> GetFavFileModels();
    }
}
