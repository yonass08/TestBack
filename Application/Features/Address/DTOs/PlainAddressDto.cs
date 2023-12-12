using Application.Features.Common;
using Domain;

namespace Application.Features.Addresses.DTOs
{
    public class PlainAddressDto
    {
        public string Country { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public string Woreda { get; set; }
        public string City { get; set; }
        public string SubCity { get; set; }
        public double Longitude { get; set;}
        public double Latitude { get; set;}
        public string Summary { get; set; }


    }
}