using Microsoft.AspNetCore.Mvc;
using RevendaPedidos.Api.Models.Requests;
using RevendaPedidos.Api.Mappers;
using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Application.Interfaces.Services;

namespace RevendaPedidos.Api.Controllers;

[ApiController]
[Route("api/revendas/{revendaId:guid}/pedidos")]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidoController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarPedido(Guid revendaId, [FromBody] PedidoRequest request)
    {
        var pedidoDto = request.Map();
        pedidoDto.RevendaId = revendaId;

        var pedidoId = await _pedidoService.RegistrarPedidoAsync(pedidoDto);

        return CreatedAtAction(nameof(ObterPorId), new { revendaId, pedidoId }, new { pedidoId });
    }

    [HttpGet]
    public async Task<IActionResult> ObterPedidosPorRevenda(Guid revendaId)
    {
        var pedidos = await _pedidoService.ObterPedidosPorRevendaAsync(revendaId);
        return Ok(pedidos);
    }

    [HttpGet("{pedidoId:guid}")]
    public async Task<IActionResult> ObterPorId(Guid revendaId, Guid pedidoId)
    {
        var pedido = await _pedidoService.ObterPorIdAsync(revendaId, pedidoId);
        if (pedido == null)
            return NotFound();

        return Ok(pedido);
    }

    [HttpPost("emitir")]
    public async Task<IActionResult> EmitirPedidosParaFornecedor(
        Guid revendaId,
        [FromBody] EmitirPedidoRequest request)
    {
        if (request.PedidoIds == null || !request.PedidoIds.Any())
            return BadRequest("Deve informar ao menos um pedido.");

        try
        {
            var pedidosAtualizados = await _pedidoService.EmitirPedidosParaFornecedorAsync(revendaId, request.PedidoIds);

            return Ok(new
            {
                PedidosEmitidos = pedidosAtualizados
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro inesperado ao emitir os pedidos: " + ex.Message);
        }
    }
}
