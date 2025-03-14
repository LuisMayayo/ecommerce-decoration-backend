public interface IProveedoresRepository
{
    Task<IEnumerable<Proveedor>> GetAllAsync();
    Task<Proveedor> GetByIdAsync(int id);
    Task<Proveedor> AddAsync(Proveedor proveedor);
    Task<bool> UpdateAsync(Proveedor proveedor);
    Task<bool> DeleteAsync(int id);
}
