// backend/Models/Product.cs

using System.Collections.Generic;

namespace backend.Models
{
    public class Product
    {
        public string Name { get; set; }
        public double PopularityScore { get; set; }
        public double Weight { get; set; }
        public Dictionary<string, string> Images { get; set; }
    }
}