namespace RevendaPedidos.Api.Models.Requests;

public class RevendaRequest
{
    public string Cnpj { get; set; } = string.Empty;
    public string RazaoSocial { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Telefones { get; set; } = new();
    public List<ContatoRequest> Contatos { get; set; } = new();
    public List<EnderecoRequest> EnderecosEntrega { get; set; } = new();
}

public class ContatoRequest
{
    public string Nome { get; set; } = string.Empty;
    public bool Principal { get; set; }
}

public class EnderecoRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Rua { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
}