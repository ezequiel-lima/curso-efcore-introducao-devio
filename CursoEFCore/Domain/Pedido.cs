using CursoEFCore.ValueObjects;

namespace CursoEFCore.Domain;

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public virtual Cliente Cliente { get; set; }
    public DateTime IniciadoEm { get; set; }
    public DateTime FinalizadoEm { get; set; }
    public TipoFrete TipoFrete { get; set; }
    public StatusPedido StatusPedido { get; set; }
    public string Observacao { get; set; }
    public virtual ICollection<PedidoItem> Itens { get; set; }
}