using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeuroPulse.Abstracions.Interface;
using NeuroPulse.Data.Context;
using NeuroPulse.Data.Data;
using NeuroPulse.Domain.Dto;

namespace NeuroPulseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsuariosController(
         IUsuarioServices usuarioServices,               
         NeuroPulseContext db
     ) : ControllerBase
    {
       
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuarioDto usuarioDto)
        {
            var creado = await usuarioServices.RegisterAsync(usuarioDto);

            if (!creado)
                return BadRequest("El nombre de usuario ya está en uso.");

            return Ok(new { mensaje = "Usuario registrado con éxito." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] PeticionLogin request)
        {
            var usuario = await usuarioServices.LoginAsync(request);

            if (usuario is null)
                return Unauthorized("Usuario o contraseña incorrectos.");

            // Token falso (para sesión local en la app)
            usuario.Token = Guid.NewGuid().ToString();

            return Ok(usuario);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> Get()
            => await usuarioServices
                   .Listar(_ => true);   

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> Get(int id)
        {
            var usuario = await usuarioServices.Buscar(id);
            return usuario is null ? NotFound() : Ok(usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UsuarioDto dto)
        {
            if (id != dto.UsuarioId) return BadRequest();

            var actualizado = await usuarioServices.Update(dto);
            return actualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await db.Usuarios.FindAsync(id);
            if (usuario is null) return NotFound();

            db.Remove(usuario);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }

}

