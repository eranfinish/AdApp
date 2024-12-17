using AdApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdApp.Core.Services.Ads
{
    public interface IAdService
    {
        List<Ad> GetAllAds();
        Ad GetAdById(int id); 
        void CreateAd(Ad ad);
        bool UpdateAd(Ad updatedAd);
        bool DeleteAd(int id);
    }
}
