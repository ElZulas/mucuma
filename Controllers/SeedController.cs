using Microsoft.AspNetCore.Mvc;
using PracticaAPI.Data;

namespace PracticaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly DataSeeder _seeder;

        public SeedController(DataSeeder seeder)
        {
            _seeder = seeder;
        }

        public class SeedRequest
        {
            public int Categories { get; set; } = 0;
            public int Budgets { get; set; } = 0;
            public int Expenses { get; set; } = 0;
        }

        [HttpPost]
        public IActionResult Seed([FromBody] SeedRequest request)
        {
            _seeder.Seed(request.Categories, request.Budgets, request.Expenses);
            return Ok(new { message = "Datos generados correctamente.", request });
        }
    }
} 