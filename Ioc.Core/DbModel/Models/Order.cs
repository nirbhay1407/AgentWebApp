namespace Ioc.Core.DbModel.Models
{

    public class Payment : PublicBaseEntity
    {
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    // Define a class for shipping details
    public class Shipping : PublicBaseEntity
    {
        public int ShippingId { get; set; }
        public string ShippingMethod { get; set; }
        public decimal ShippingCost { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public string ShippingAddress { get; set; }
    }

    
    // Define a class for order details
    public class Order : PublicBaseEntity
    {
        public int OrderId { get; set; }
        public Customer CustomerDetails { get; set; }
        public List<Product> Products { get; set; }
        public Payment PaymentDetails { get; set; }
        public Shipping ShippingDetails { get; set; }
        public Address BillingDetails { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderNotes { get; set; }

    }
}
