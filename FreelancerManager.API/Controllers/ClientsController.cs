using FreelancerManager.Application.DTOs;
using FreelancerManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ClientsController(IClientService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientRequestDto request, CancellationToken cancellationToken)
    {
        var client = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var client = await service.GetByIdAsync(id, cancellationToken);
        return client is null ? NotFound() : Ok(client);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var clients = await service.GetAllAsync(cancellationToken);
        return Ok(clients);
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}