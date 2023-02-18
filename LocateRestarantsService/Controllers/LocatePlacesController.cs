using System;
using System.Net.Http;
using System.Threading.Tasks;
using LocateRestarantsService.objects;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Device.Location;
using System.Collections.Generic;

namespace LocateRestarantsService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocatePlacesController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<PlacesApiQueryResponse>> GetAsync()
        {

            double latitude = 31.972549;
            double longitude = 34.761719;
            List<PlacesApiQueryResponse> allPlaces = new List<PlacesApiQueryResponse>();
            using (var client = new HttpClient())
            {
                string url = string.Format("https://maps.googleapis.com/maps/api/place/textsearch/json?query=asianfood&location={0},{1}&radius=1500&key=AIzaSyD3EseMyzh4Bq5Qq0wlxiZ5tcdzHf4Ii0o", latitude, longitude);
                dynamic response = null;
                while (response == null || HasProperty(response, "next_page_token") != null)
                {
                    if (response != null && HasProperty(response, "next_page_token") != null)
                    {
                        if (url.Contains("pagetoken"))
                            url = url.Split(new string[] { "&pagetoken=" }, StringSplitOptions.None)[0];
                        TokenObject token = HasProperty(response, "next_page_token");
                        url += "&pagetoken=" + token.next_page_token;

                    }
                    response = await client.GetStringAsync(url);
                    var res = JsonConvert.DeserializeObject<PlacesApiQueryResponse>(response);
                    allPlaces.Add(res);
                    await Task.Delay(5000);
                }
                return allPlaces;
            }
          }
            public static TokenObject HasProperty(dynamic obj, string name)
            {
                try
                {
                    TokenObject tokenObject = JsonConvert.DeserializeObject<TokenObject>(obj);
                    return tokenObject.next_page_token != null ? tokenObject : null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
     }


