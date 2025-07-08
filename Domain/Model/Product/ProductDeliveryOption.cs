using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Product
{
    public class ProductDeliveryOption
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int DeliveryOptionId { get; set; }
        public DeliveryOption DeliveryOption { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}
