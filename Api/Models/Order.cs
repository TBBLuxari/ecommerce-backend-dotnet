namespace Api.Models
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public DateTime Date { get;  set; } 
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pendiente;
    }

    public enum OrderStatus
    {
        Pendiente,
        Pagada,
        Cancelada
    }
}
