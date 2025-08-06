using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model.Product
{
    public class DeliveryOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<ProductDeliveryOption>? ProductDeliveryOptions { get; set; }
    }
}