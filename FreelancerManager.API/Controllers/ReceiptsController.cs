using FreelancerManager.Application.DTOs;
using FreelancerManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReceiptsController(IReceiptService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReceiptRequestDto request, CancellationToken cancellationToken)
    {
        var receipt = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = receipt.Id }, receipt);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var receipt = await service.GetByIdAsync(id, cancellationToken);
        return receipt is null ? NotFound() : Ok(receipt);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var receipts = await service.GetAllAsync(cancellationToken);
        return Ok(receipts);
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReceiptRequestDto request, CancellationToken cancellationToken)
    {
        await service.UpdateAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")] // ESTA LINHA É A CHAVE PARA O 405 SUMIR
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}