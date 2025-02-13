public interface IUsuarioService
{
    Task<Usuario> AddAsync(Usuario usuario);
    Task<Usuario> GetByIdAsync(int id);
    Task<Usuario> GetByEmailAsync(string email);
}
