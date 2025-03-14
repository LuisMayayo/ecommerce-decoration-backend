public class ProveedoresService : IProveedoresService
{
    private readonly IProveedoresRepository _repository;

    public ProveedoresService(IProveedoresRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Proveedor>> GetAllProveedoresAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Proveedor> GetProveedorByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Proveedor> AddProveedorAsync(Proveedor proveedor)
    {
        return await _repository.AddAsync(proveedor);
    }

    public async Task<bool> UpdateProveedorAsync(Proveedor proveedor)
    {
        return await _repository.UpdateAsync(proveedor);
    }

    public async Task<bool> DeleteProveedorAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
