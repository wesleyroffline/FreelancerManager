using FreelancerManager.Application.DTOs;
using FreelancerManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ContractsController(IContractService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContractRequestDto request, CancellationToken cancellationToken)
    {
        var contract = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var contract = await service.GetByIdAsync(id, cancellationToken);
        return contract is null ? NotFound() : Ok(contract);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var contracts = await service.GetAllAsync(cancellationToken);
        return Ok(contracts);
    }
}