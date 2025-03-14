using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class ProveedoresController : ControllerBase
{
    private readonly IProveedoresService _service;

    public ProveedoresController(IProveedoresService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedores()
    {
        var proveedores = await _service.GetAllProveedoresAsync();
        return Ok(proveedores);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Proveedor>> GetProveedor(int id)
    {
        var proveedor = await _service.GetProveedorByIdAsync(id);
        
        if (proveedor == null)
            return NotFound();
            
        return Ok(proveedor);
    }

    [HttpPost]
    public async Task<ActionResult<Proveedor>> CreateProveedor(Proveedor proveedor)
    {
        var newProveedor = await _service.AddProveedorAsync(proveedor);
        return CreatedAtAction(nameof(GetProveedor), new { id = newProveedor.Id }, newProveedor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProveedor(int id, Proveedor proveedor)
    {
        if (id != proveedor.Id)
            return BadRequest();
            
        var result = await _service.UpdateProveedorAsync(proveedor);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProveedor(int id)
    {
        var result = await _service.DeleteProveedorAsync(id);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}
