namespace RevendaPedidos.Domain.Entities;

public class Contato
{
    public string Nome { get; private set; }
    public bool Principal { get; private set; }

    public Contato(string nome, bool principal)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("Nome de contato inválido.");

        Nome = nome;
        Principal = principal;
    }
}
