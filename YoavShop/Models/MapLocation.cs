using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoavShop.Models
{
    public class MapLocation
    {
        public int Id { get; set; } 
        public string PlaceName { get; set; }
        public double GeoLong { get; set; }
        public double GeoLat { get; set; }
        public string Info { get; set; }
    }
}