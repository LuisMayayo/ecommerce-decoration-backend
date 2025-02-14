public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Usuario> AddAsync(Usuario usuario)
    {
        // Validaciones adicionales para el correo electrónico
        if (string.IsNullOrWhiteSpace(usuario.Email))
        {
            throw new ArgumentException("El correo electrónico no puede estar vacío.");
        }

        return await _usuarioRepository.AddAsync(usuario);
    }

    public async Task<Usuario> GetByIdAsync(int id)
    {
        return await _usuarioRepository.GetByIdAsync(id);
    }

    public async Task<Usuario> GetByEmailAsync(string email)
    {
        return await _usuarioRepository.GetByEmailAsync(email);
    }
}
