using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocateRestarantsService.objects
{
    public class PlacesApiQueryResponse
    {
        public List<object> html_attributions { get; set; }
        public List<Result> results { get; set; }
        public string status { get; set; }
    }
}
