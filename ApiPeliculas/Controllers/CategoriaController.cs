using ApiPeliculas.Model;
using ApiPeliculas.Model.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriasRepositorio _ctRepo;
        private readonly IMapper _mapper;

        public CategoriaController(ICategoriasRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetCategorias()
        {
            var listaCategorias = _ctRepo.GetCategorias();

            var listaCategoriasDto = new List<CategoriaDto>();

            foreach (var lista in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(lista));
            }
            return Ok(listaCategoriasDto);
        }

        [HttpGet("{categoriaId:int}", Name = "GetCategoria")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int categoriaId)
        {
            var itemCategoria = _ctRepo.GetCategoria(categoriaId);

            if (itemCategoria == null)
            {
                return NotFound();
            }
            var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);


            return Ok(itemCategoriaDto);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult AddCategoria([FromBody]AddCategoriaDto addCategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (addCategoriaDto == null)
            {
                return NotFound();
            }

            if (_ctRepo.ExistCategoria(addCategoriaDto.Name))
            {
                ModelState.AddModelError("","La categoria ya existe");
                return StatusCode(404,ModelState);
            }
            var categoria = _mapper.Map<Categoria>(addCategoriaDto);

            if (!_ctRepo.AddCategoria(categoria))
            {
                ModelState.AddModelError("",$"Algo salio mal guardando el registro {categoria.Name}");
                return StatusCode(404,ModelState);
            }
            // Your get implementation here.
            //Ok(new CategoriaDto { Id = categoria.Id });
            return CreatedAtAction(nameof(GetCategoria), new { categoriaId = categoria.Id }, categoria);
            //return CreatedAtRoute((nameof(GetCategoria), new { categoriaId = categoria.Id });
        }

        [HttpPut("{categoriaId:int}",Name = "UpdateCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult UpdateCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoriaDto == null || categoriaId != categoriaDto.Id)
            {
                return NotFound();
            }
            var categoriaExistente = _ctRepo.GetCategoria(categoriaId);

            if (categoriaExistente == null)
            {
                return NotFound($"No se encontro la categoria con Id {categoriaId}");

            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            if (!_ctRepo.UpdateCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro {categoria.Name}");
                return StatusCode(500, ModelState);
            }
            // Your get implementation here.
            //Ok(new CategoriaDto { Id = categoria.Id });
            return NoContent();
            //return CreatedAtRoute((nameof(GetCategoria), new { categoriaId = categoria.Id });

        }
        
        
        [HttpDelete("{categoriaId:int}", Name = "DeleteCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategoria(int categoriaId)
        {

            if (!_ctRepo.ExistCategoria(categoriaId)) {
                return NotFound();
                    
                    }

            var categoria = _ctRepo.GetCategoria(categoriaId);

            if (!_ctRepo.DeleteCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {categoria.Name}");
                return StatusCode(500, ModelState);
            }
            // Your get implementation here.
            //Ok(new CategoriaDto { Id = categoria.Id });
            return NoContent();
            //return CreatedAtRoute((nameof(GetCategoria), new { categoriaId = categoria.Id });

        }
    }
}
