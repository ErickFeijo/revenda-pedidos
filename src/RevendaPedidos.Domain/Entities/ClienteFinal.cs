namespace RevendaPedidos.Domain.Entities
{
    public class ClienteFinal
    {
        public string Nome { get; private set; }
        public string? Documento { get; private set; }

        private ClienteFinal() { }

        public ClienteFinal(string nome, string? documento = null)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do cliente final é obrigatório.");
            Nome = nome;
            Documento = string.IsNullOrWhiteSpace(documento) ? null : documento;
        }
    }

}
