namespace RevendaPedidos.Domain.Entities;

public class Telefone
{
    public string Numero { get; private set; }

    public Telefone(string numero)
    {
        Numero = numero;
    }
}
