namespace RevendaPedidos.Domain.Entities
{
    public class ItemPedido
    {
        public Guid Id { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public decimal PrecoUnitario { get; private set; }
        public int Quantidade { get; private set; }
        public decimal Total => PrecoUnitario * Quantidade;

        private ItemPedido() { }

        public ItemPedido(Guid produtoId, string produtoNome, decimal precoUnitario, int quantidade)
        {
            Id = Guid.NewGuid();
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            PrecoUnitario = precoUnitario;
            Quantidade = quantidade;
        }
    }

}
