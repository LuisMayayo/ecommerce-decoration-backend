public interface IProveedoresService
{
    Task<IEnumerable<Proveedor>> GetAllProveedoresAsync();
    Task<Proveedor> GetProveedorByIdAsync(int id);
    Task<Proveedor> AddProveedorAsync(Proveedor proveedor);
    Task<bool> UpdateProveedorAsync(Proveedor proveedor);
    Task<bool> DeleteProveedorAsync(int id);
}
