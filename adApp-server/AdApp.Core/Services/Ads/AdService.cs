using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdApp.Models;
using AdApp.DAL;

namespace AdApp.Core.Services.Ads
{
    public class AdService:IAdService
    {
        private readonly JsonStorage _jsonStorage = new JsonStorage();
        private readonly string _filePath;

        public AdService()
        {
           // _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DATA", "ads.json");
            // Define the folder path
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DATA");

            // Ensure the folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Combine the folder path with the file name
            string filePath = Path.Combine(folderPath, "ads.json");

            // Create the file if it does not exist (initialize with an empty array)
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }
            _filePath = filePath;
        }

        public List<Ad> GetAllAds()
        {
            var ads = _jsonStorage.Load<List<Ad>>(_filePath);
            return ads ?? new List<Ad>();
        }

        public Ad GetAdById(int id)
        {
            return GetAllAds().FirstOrDefault(ad => ad.id == id);
        }

        public void CreateAd(Ad ad)
        {
            var ads = GetAllAds();
            ad.id = ads.Any() ? ads.Max(a => a.id) + 1 : 1;
            ads.Add(ad);
            _jsonStorage.Save(_filePath, ads);
        }

        public bool UpdateAd( Ad updatedAd)
        {
            try
            {      
                
                var ads = GetAllAds();
                var adIndex = ads.FindIndex(ad => ad.id == updatedAd.id);
                if (adIndex >= 0)
                {
                    ads[adIndex] = updatedAd;
                    _jsonStorage.Save(_filePath, ads);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex);
                return false;
            }
        }

        public bool DeleteAd(int id)
        {
            try
            {
                var ads = GetAllAds();
                ads.RemoveAll(ad => ad.id == id);
                _jsonStorage.Save(_filePath, ads);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex);
                return false;
            }
        }
    }

}
