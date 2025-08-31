using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.Intrinsics.X86;
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


        [HttpGet("GetHero")]
        public async Task<Batman?> GetHero(Guid id)
        {
            return await GetHeroFromId(id);
        }

        [HttpGet("GetAll")]
        public async Task<IList<Batman>> GetAll()
        {
            return await GetHeros();
        }

        [HttpPost("Insert")]
        public async Task<ActionResult<Batman>> Insert(string nome)
        {
            var hero = new Batman { Name = nome };

            if (string.IsNullOrEmpty(nome))
                return BadRequest("Batman?");

            if (!nome.ToLower().Contains("batman"))
                 return BadRequest("Este não é um Batman!");

            if ((await GetHeros()).Where(h => h.Name?.ToLower() == nome.ToLower()).Any())
                return BadRequest("Bat-Sinal: Esse Batman ja existe!");

            _dbContext.Batmans.Add(hero);
            await _dbContext.SaveChangesAsync();

            return hero;
        }

        [HttpPut("Update")]
        public async Task<ActionResult<Batman>> Update(Guid id, Batman hero)
        {
            if (hero is null || string.IsNullOrEmpty(hero.Name))
                return BadRequest("Batman?");

            var heroExists = await GetHeroFromId(id);

            if(heroExists == null)
                return BadRequest("Bat-Sinal não encontrado: Esse Batman não existe!");

            if ((await GetHeros()).Where(h => h.Name?.ToLower() == hero.Name.ToLower() && h.Id != id).Any())
                return BadRequest("Bat-Sinal: Esse Batman ja existe!");

            hero.Id = id;

            _dbContext.Batmans.Update(hero);
            await _dbContext.SaveChangesAsync();

            return hero;
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<int>> Delete(Guid id)
        {
            var heroExists = await GetHeroFromId(id);

            if (heroExists == null)
                return BadRequest("Bat-Sinal não encontrado: Esse Batman não existe!");

            _dbContext.Batmans.Remove(heroExists);
            return await _dbContext.SaveChangesAsync();
        }

        private async Task<Batman?> GetHeroFromId(Guid id)
        {
            var hero = await _dbContext.Batmans.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
            return hero;
        }

        private async Task<IList<Batman>> GetHeros()
        {
            return await _dbContext.Batmans.AsNoTracking().ToListAsync();
        }

    }
}
