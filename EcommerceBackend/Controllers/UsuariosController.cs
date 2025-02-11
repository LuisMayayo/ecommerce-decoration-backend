using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private static List<Usuario> _usuarios = new List<Usuario>
    {
        new Usuario { Id = 1, Nombre = "Marcos Larraga", Email = "marcosl@example.com", PasswordHash = "12345ml" },
        new Usuario { Id = 2, Nombre = "Luis Mayayo", Email = "luism@example.com", PasswordHash = "12345ml" }
    };

    [HttpGet]
    public IActionResult GetUsuarios()
    {
        return Ok(_usuarios);
    }

    [HttpGet("{id}")]
    public IActionResult GetUsuario(int id)
    {
        var usuario = _usuarios.Find(u => u.Id == id);
        if (usuario == null) return NotFound();
        return Ok(usuario);
    }

    [HttpPost]
    public IActionResult CreateUsuario([FromBody] Usuario usuario)
    {
        if (usuario == null) return BadRequest();
        usuario.Id = _usuarios.Count + 1;
        _usuarios.Add(usuario);
        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUsuario(int id, [FromBody] Usuario usuarioActualizado)
    {
        var usuario = _usuarios.Find(u => u.Id == id);
        if (usuario == null) return NotFound();
        
        usuario.Nombre = usuarioActualizado.Nombre;
        usuario.Email = usuarioActualizado.Email;
        usuario.PasswordHash = usuarioActualizado.PasswordHash;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUsuario(int id)
    {
        var usuario = _usuarios.Find(u => u.Id == id);
        if (usuario == null) return NotFound();
        
        _usuarios.Remove(usuario);
        return NoContent();
    }
}
