using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly List<Usuario> _usuarios = new List<Usuario>
    {
        new Usuario { Id = 1, Nombre = "Juan Pérez", Email = "juan@example.com", PasswordHash = "123456" },
        new Usuario { Id = 2, Nombre = "Ana López", Email = "ana@example.com", PasswordHash = "abcdef" }
    };

    public async Task<IEnumerable<Usuario>> GetAll()
    {
        return await Task.FromResult(_usuarios);
    }

    public async Task<Usuario> GetById(int id)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        return await Task.FromResult(usuario);
    }

    public async Task Add(Usuario usuario)
    {
        usuario.Id = _usuarios.Count + 1;
        _usuarios.Add(usuario);
        await Task.CompletedTask;
    }

    public async Task Update(Usuario usuario)
    {
        var index = _usuarios.FindIndex(u => u.Id == usuario.Id);
        if (index != -1)
        {
            _usuarios[index] = usuario;
        }
        await Task.CompletedTask;
    }

    public async Task Delete(int id)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        if (usuario != null)
        {
            _usuarios.Remove(usuario);
        }
        await Task.CompletedTask;
    }
}
