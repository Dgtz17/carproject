namespace Domain;
public class Venta
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public Guid ProductoId { get; set; }
    public string Concepto { get; set; }

    public virtual Producto Producto { get; set; }
    public virtual Cliente Cliente { get; set; }
}