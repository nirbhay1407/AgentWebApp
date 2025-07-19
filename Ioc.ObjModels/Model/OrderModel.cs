using Ioc.Core.DbModel.Models;
using Ioc.Core.DbModel;
using Ioc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ioc.ObjModels.Model.CommonModel;

namespace Ioc.ObjModels.Model
{
    public class PaymentModel : PublicBaseModel
    {
        public int PaymentId { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    // Define a class for shipping details
    public class ShippingModel : PublicBaseModel
    {
        public int ShippingId { get; set; }
        public string? ShippingMethod { get; set; }
        public decimal ShippingCost { get; set; }
        public string? EstimatedDeliveryDate { get; set; }
        public string? ShippingAddress { get; set; }
    }


    // Define a class for order details
    public class OrderModel : PublicBaseModel
    {
        public int OrderId { get; set; }
        public CustomerModel? CustomerDetails { get; set; }
        public List<ProductModel> Products { get; set; }
        public PaymentModel? PaymentDetails { get; set; }
        public ShippingModel? ShippingDetails { get; set; }
        public AddressModel? BillingDetails { get; set; }
        public string? OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public string? OrderNotes { get; set; }

    }
}
