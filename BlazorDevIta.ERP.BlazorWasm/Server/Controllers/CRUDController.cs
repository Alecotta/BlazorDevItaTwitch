using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorDevIta.ERP.Infrastructure;
using BlazorDevIta.ERP.Infrastructure.DataTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorDevIta.ERP.Server.Controllers
{
    [ApiController]
    // [Route("[controller]")]
    public class CRUDController<ListItemType, DetailsType, IdType, EntityType>
        : ControllerBase
        where ListItemType : BaseListItem<IdType>
        where DetailsType : BaseDetails<IdType>
        where EntityType : class, IEntity<IdType>, new()
    {
        protected readonly ILogger<CRUDController<ListItemType, DetailsType, IdType, EntityType>> _logger;
        protected IRepository<EntityType, IdType> _repository;
        protected readonly IMapper _mapper;

        public CRUDController(IMapper mapper, ILogger<CRUDController<ListItemType, DetailsType, IdType, EntityType>> logger, IRepository<EntityType, IdType> repository)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<IActionResult> Get([FromQuery] PageParameters pageParameters)
        {
            var result = _repository.GetAll();

            var sortedResult = await result
                .OrderBy(x => x.Id)
                //Questo metodo si occupa di mappare una lista di un tipo in un altro tipo. Esso lavora sull'IQueryable (non lavora su DB).
                .ProjectTo<ListItemType>(_mapper.ConfigurationProvider)
                .ToListAsync();

            /*.Select(x =>
                new ListItemType()
                {
                    Id = x.Id,
                    Date = x.Date,
                    TemperatureC = x.TemperatureC
                }).ToListAsync();*/

            var page = new Page<ListItemType, IdType>()
            {
                Items = sortedResult,
                OrderBy = pageParameters.OrderBy,
                OrderByDirection = pageParameters.OrderByDirection
            };

            return Ok(page);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(IdType id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null) return NotFound();

            var result = _mapper.Map<DetailsType>(entity);

            /*new WeatherForecastDetails()
            {
                Id = entity.Id,
                Date = entity.Date,
                TemperatureC = entity.TemperatureC,
                Summary = entity.Summary
            };*/

            return Ok(result);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post(DetailsType model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<EntityType>(model);

                /*new EntityType()
                {
                    Id = model.Id,
                    Date = model.Date,
                    TemperatureC = model.TemperatureC,
                    Summary = model.Summary
                };*/

                await _repository.CreateAsync(entity);
                //REST nella risposta di una POST chiede di restituire la URI che accede alla risorsa (nome metodo + parametro) ed entità stessa. 
                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(IdType id, DetailsType model)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null) return NotFound();

            if (ModelState.IsValid)
            {
                entity = _mapper.Map<EntityType>(model);
                /*entity.Date = model.Date;
                entity.TemperatureC = model.TemperatureC;
                entity.Summary = model.Summary;*/

                await _repository.UpdateAsync(entity);
                //Si ritorna OK se si restituisce l'entità modificata, oppure NoContent
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(IdType id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null) return NotFound();

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}