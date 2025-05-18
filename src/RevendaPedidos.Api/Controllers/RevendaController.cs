using Microsoft.AspNetCore.Mvc;
using RevendaPedidos.Api.Models.Requests;
using RevendaPedidos.Api.Mappers;
using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Application.Interfaces.Services;

namespace RevendaPedidos.Api.Controllers;

[ApiController]
[Route("api/revendas")]
public class RevendaController : ControllerBase
{
    private readonly IRevendaService _service;

    public RevendaController(IRevendaService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CadastrarRevenda([FromBody] RevendaRequest request)
    {
        var id = await _service.CadastrarRevendaAsync(request.Map());
        return CreatedAtAction(nameof(ObterPorId), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodas()
    {
        var revendas = await _service.ObterTodasAsync();
        return Ok(revendas);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var revenda = await _service.ObterPorIdAsync(id);
        if (revenda == null)
            return NotFound();

        return Ok(revenda);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] RevendaRequest request)
    {
        var sucesso = await _service.AtualizarRevendaAsync(id, request.Map());
        if (!sucesso)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        var sucesso = await _service.RemoverRevendaAsync(id);
        if (!sucesso)
            return NotFound();

        return NoContent();
    }
}
