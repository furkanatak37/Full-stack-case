// backend/Controllers/ProductController.cs

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using backend.Models;
using backend.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly GoldPriceService _goldPriceService;

        public ProductController(IWebHostEnvironment env, GoldPriceService goldPriceService)
        {
            _env = env;
            _goldPriceService = goldPriceService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "Data", "products.json");
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            
            // 1. JSON serileştirme seçenekleri oluşturuluyor.
            // Bu seçenek, JSON dosyasındaki "name" gibi camelCase isimlendirmeyi,
            // C# sınıfındaki "Name" gibi PascalCase isimlendirmeyle eşleştirir.
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // 2. Deserializer'a seçenekler ekleniyor.
            var products = JsonSerializer.Deserialize<List<Product>>(json, options);

            double goldPrice = await _goldPriceService.GetGoldPriceAsync();

            var result = products.Select(p => new
            {
                // 3. 'p.name' -> 'p.Name' olarak düzeltildi.
                p.Name,
                p.Images,
                PopularityScore = Math.Round(p.PopularityScore*5, 2),
                p.Weight,
                Price = Math.Round((p.PopularityScore +1) * p.Weight * goldPrice/31.1, 2)
            });

            return Ok(result);
        }
    }
}