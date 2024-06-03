using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Product : BaseEntity
    {
        public int ProductCategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public ProductCategory Category { get; set; }
        public virtual IList<ReceiptDetail> ReceiptDetails { get; set; }

        public Product()
        {
            ProductName = string.Empty;
        }
        public Product(int productCategoryId, string productName, decimal price)
        {
            ProductCategoryId = productCategoryId;
            ProductName = productName;
            Price = price;
        }
    }
}
