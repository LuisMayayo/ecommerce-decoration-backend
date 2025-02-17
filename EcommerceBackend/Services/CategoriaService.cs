using System.Collections.Generic;
using System.Threading.Tasks;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<List<Categoria>> GetAllAsync()
    {
        return await _categoriaRepository.GetAllAsync();
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        return await _categoriaRepository.GetByIdAsync(id);
    }
}
