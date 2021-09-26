using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Windows.Forms;

namespace ImageGallery_Main_
{ 
    
    class DataFetcher
    {
        public async Task<string> GetDataFromServiceAsync(string searchString)
        {
            string readInput = "";
            try
            {
                String url = @"https://imagefetcherapi.azurewebsites.net/api/fetch_images?query=" +
                searchString + "&max_count=4";
                HttpClient hc = new HttpClient();
                readInput = await hc.GetStringAsync(url);
            }
            catch (Exception )
            {


                readInput = File.ReadAllText(@"..\..\Resources\sampleData.json");
                
            }

            return readInput;
        }

        public async Task<List<ImageItem>> GetImageData(string search)
        {
            string result = await GetDataFromServiceAsync(search);
            return JsonConvert.DeserializeObject<List<ImageItem>>(result);
        }    
    
    
    
    
    
    }
 
}
