namespace RevendaPedidos.Domain.Entities;

public class EnderecoEntrega
{
    public string Nome { get; private set; }
    public string Rua { get; private set; }
    public string Numero { get; private set; }
    public string Complemento { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string Cep { get; private set; }

    public EnderecoEntrega(string nome, string rua, string numero, string complemento, string cidade, string estado, string cep)
    {
        Nome = string.IsNullOrWhiteSpace(nome) ? throw new Exception("Nome inválido.") : nome;
        Rua = string.IsNullOrWhiteSpace(rua) ? throw new Exception("Rua inválida.") : rua;
        Numero = numero;
        Complemento = complemento;
        Cidade = cidade;
        Estado = estado;
        Cep = cep;
    }
}
