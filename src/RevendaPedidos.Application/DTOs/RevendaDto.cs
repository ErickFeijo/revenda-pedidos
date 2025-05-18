namespace RevendaPedidos.Application.DTOs;

public class RevendaDto
{
    public string Cnpj { get; set; } = string.Empty;
    public string RazaoSocial { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Telefones { get; set; } = new();
    public List<ContatoDto> Contatos { get; set; } = new();
    public List<EnderecoDto> EnderecosEntrega { get; set; } = new();
}
