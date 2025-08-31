using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using WebAppDockerTeste.Data;
using WebAppDockerTeste.Models;

namespace WebAppDockerTeste.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BatmanController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private AppDbContext _dbContext;

        public BatmanController(ILogger<WeatherForecastController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        [HttpGet("GetAll")]
        public async Task<IList<Batman>> GetAll()
        {
            return await _dbContext.Batmans.ToListAsync();
        }

    }
}
