using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocateRestarantsService.objects
{
    public class Photo
    {
        public int height { get; set; }
        public List<string> html_attributions { get; set; }
        public string photo_reference { get; set; }
        public int width { get; set; }
    }
}
