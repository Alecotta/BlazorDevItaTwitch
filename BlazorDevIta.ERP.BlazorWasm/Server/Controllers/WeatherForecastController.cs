using BlazorDevIta.ERP.Business.Data;
using BlazorDevIta.ERP.Infrastructure;
using BlazorDevIta.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorDevIta.ERP.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private IRepository<WeatherForecast, int> _repository;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRepository<WeatherForecast, int> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _repository.GetAll()
                .Select(x =>
                    new WeatherForecastListItem()
                    {
                        Id = x.Id,
                        Date = x.Date,
                        TemperatureC = x.TemperatureC
                    }).ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null) return NotFound();

            var result = new WeatherForecastDetails()
            {
                Id = entity.Id,
                Date = entity.Date,
                TemperatureC = entity.TemperatureC,
                Summary = entity.Summary
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(WeatherForecastDetails model)
        {
            if (ModelState.IsValid)
            {
                var entity = new WeatherForecast()
                {
                    Id = model.Id,
                    Date = model.Date,
                    TemperatureC = model.TemperatureC,
                    Summary = model.Summary
                };

                await _repository.CreateAsync(entity);
                //REST nella risposta di una POST chiede di restituire la URI che accede alla risorsa (nome metodo + parametro) ed entit� stessa. 
                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, WeatherForecastDetails model)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null) return NotFound();

            if (ModelState.IsValid)
            {
                entity.Date = model.Date;
                entity.TemperatureC = model.TemperatureC;
                entity.Summary = model.Summary;

                await _repository.UpdateAsync(entity);
                //Si ritorna OK se si restituisce l'entit� modificata, oppure NoContent
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null) return NotFound();

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}