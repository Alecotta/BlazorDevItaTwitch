using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorDevIta.ERP.Infrastructure;
using BlazorDevIta.ERP.Infrastructure.DataTypes;
using BlazorDevIta.ERP.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        protected readonly int pageSize = 2;

        public CRUDController(IMapper mapper, ILogger<CRUDController<ListItemType, DetailsType, IdType, EntityType>> logger, IRepository<EntityType, IdType> repository)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        protected virtual Expression<Func<EntityType, bool>>? Filter(string filterText)
        {
            return null;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<IActionResult> Get([FromQuery] PageParameters pageParameters)
        {
            var result = _repository.GetAll();

            if (!string.IsNullOrEmpty(pageParameters.FilterText))
            {
                var predicate = Filter(pageParameters.FilterText);
                if (predicate != null)
                {
                    result = result.Where(predicate);
                }
            }

            var itemCount = result.Count();
            var pageCount = (itemCount + pageSize - 1) / pageSize;

            if (pageParameters.Page > pageCount)
            {
                pageParameters.Page = pageCount;
            }
            if (pageParameters.Page < 1)
            {
                pageParameters.Page = 1;
            }


            if (!string.IsNullOrEmpty(pageParameters.OrderBy))
            {
                try
                {
                    if (pageParameters.OrderByDirection == OrderDirection.Ascendent)
                    {
                        result = result.OrderByProperty(pageParameters.OrderBy);
                    }
                    else
                    {
                        result = result.OrderByPropertyDescending(pageParameters.OrderBy);
                    }
                }
                catch
                {
                    pageParameters.OrderBy = null;
                    pageParameters.OrderByDirection = OrderDirection.Ascendent;
                }
            }

            /*.Select(x =>
                new ListItemType()
                {
                    Id = x.Id,
                    Date = x.Date,
                    TemperatureC = x.TemperatureC
                }).ToListAsync();*/

            var page = new Page<ListItemType, IdType>()
            {
                Items = await result
                    .Skip((pageParameters.Page - 1) * pageSize)
                    .Take(pageSize)
                    //Questo metodo si occupa di mappare una lista di un tipo in un altro tipo. Esso lavora sull'IQueryable (non lavora su DB).
                    .ProjectTo<ListItemType>(_mapper.ConfigurationProvider)
                    .ToListAsync(),
                OrderBy = pageParameters.OrderBy,
                OrderByDirection = pageParameters.OrderByDirection,
                ItemCount = itemCount,
                PageCount = pageCount,
                CurrentPage = pageParameters.Page
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
                //REST nella risposta di una POST chiede di restituire la URI che accede alla risorsa (nome metodo + parametro) ed entit? stessa. 
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
                //Si ritorna OK se si restituisce l'entit? modificata, oppure NoContent
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