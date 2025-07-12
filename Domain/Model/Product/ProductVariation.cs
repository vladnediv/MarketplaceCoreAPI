using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Product
{
    public class ProductVariation
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public string Name { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}