using LocateRestarantsService.Dtos;
using LocateRestarantsService.objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LocateRestarantsService.Services
{
    public class FindLocationsService
    {
        public LocationRequestDto RequestDetails { get; set; }
        public HttpClient RequestHttpClient { get; set; }
        public FindLocationsService(LocationRequestDto request)
        {
            RequestDetails = request;
            RequestHttpClient = new HttpClient();
        }

        public async Task<IEnumerable<Result>> FindLocations()
        {
            List<Result> allPlaces = new List<Result>();
            string url = BuildRequestUrl(true, null);
            dynamic response;
            do
            {
                response = await RequestHttpClient.GetStringAsync(url);
                PlacesApiQueryResponse res = JsonConvert.DeserializeObject<PlacesApiQueryResponse>(response);
                if (RequestDetails.MaxResults < allPlaces.Count + res.results.Count)
                {
                    allPlaces.AddRange(res.results.Take(RequestDetails.MaxResults - allPlaces.Count));
                    break;
                }
                else
                {
                    allPlaces.AddRange(res.results);
                }
                url = BuildRequestUrl(false, HasProperty(response, "next_page_token"));
            }
            while (url != null && HasProperty(response, "next_page_token") != null);
            return allPlaces;
        }

        public string BuildRequestUrl(bool isFirstCall, TokenObject token)
        {
            double latitude = RequestDetails.StartingLocation.lat;
            double longitude = RequestDetails.StartingLocation.lng;
            double radius = RequestDetails.LocationRadious;
            string searchType = RequestDetails.FoodType.ToString();
            string url = string.Format("https://maps.googleapis.com/maps/api/place/textsearch/json?query={0}&location={1},{2}&radius={3}&key=AIzaSyDj6sMZP-LM7A9z1aDrPF5o7gyEtFOpk-4", 
                            searchType, latitude, longitude, radius);
            return isFirstCall ? url : AddNextTokenRequest(url, token);
        }

        public string AddNextTokenRequest(string baseUrl, TokenObject token)
        {
            if (token != null)
            {
                if (baseUrl.Contains("pagetoken"))
                      baseUrl = baseUrl.Split(new string[] { "&pagetoken=" }, StringSplitOptions.None)[0];
                baseUrl += "&pagetoken=" + token.next_page_token;
                return baseUrl;
            }
            return null;
        }

        public TokenObject HasProperty(dynamic obj, string name)
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
