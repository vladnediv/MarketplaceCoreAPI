using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model.Product
{
    public class ProductCharacteristic
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        public int? GroupId { get; set; }

        public string? Name { get; set; }
        public IEnumerable<KeyValue> Characteristics { get; set; }
    }
}