using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EcommerceBackend.Data;
using EcommerceBackend.Models;

namespace EcommerceBackend.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly EcommerceDbContext _context;

        public UsuarioRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await GetByIdAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
