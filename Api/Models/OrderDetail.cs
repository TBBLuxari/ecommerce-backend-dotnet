namespace Api.Models
{
    public class OrderDetail
    {
        public int Id { get; private set; }

        public string OrderId { get;  set; } = string.Empty;

        public int ProductId { get; set; } 

        public int Amount { get; set; }

        public decimal SinglePrice { get; set; }
    }
}
