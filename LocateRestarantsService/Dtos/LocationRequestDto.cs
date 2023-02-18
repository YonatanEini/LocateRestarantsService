using LocateRestarantsService.enums;
using LocateRestarantsService.objects;
using System.ComponentModel.DataAnnotations;

namespace LocateRestarantsService.Dtos
{
    public class LocationRequestDto
    {
        [Required]
        public foodTypes FoodType { get; set; }
        [Required]
        public Location StartingLocation { get; set; }
        [Required]
        public double LocationRadious { get; set; }

        [Required]
        public int MaxResults { get; set; }

    }
}
