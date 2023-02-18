using System;
using System.Net.Http;
using System.Threading.Tasks;
using LocateRestarantsService.objects;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Device.Location;
using System.Collections.Generic;
using LocateRestarantsService.Dtos;
using System.Net;
using LocateRestarantsService.Services;

namespace LocateRestarantsService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocatePlacesController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<Result>> GetAsync()
        {

            double latitude = 31.972549;
            double longitude = 34.761719;
            List<Result> allPlaces = new List<Result>();
            using (var client = new HttpClient())
            {
                string url = string.Format("https://maps.googleapis.com/maps/api/place/textsearch/json?query=shawarma&location={0},{1}&radius=1500&key=AIzaSyDj6sMZP-LM7A9z1aDrPF5o7gyEtFOpk-4", latitude, longitude);
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
                    PlacesApiQueryResponse res = JsonConvert.DeserializeObject<PlacesApiQueryResponse>(response);
                    allPlaces.AddRange(res.results);
                }
                return allPlaces;
            }
          }

         [HttpPost]
         public async Task<IEnumerable<Result>> GetLocations([FromBody] LocationRequestDto request)
         {
            if (ModelState.IsValid)
            {
                FindLocationsService locationService = new FindLocationsService(request);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return await locationService.FindLocations();
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return new List<Result>();
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


