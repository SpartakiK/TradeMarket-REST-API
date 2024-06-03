using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ProductCategory : BaseEntity
    {
        public string CategoryName { get; set; }

        public virtual IList<Product> Products { get; set; }

        public ProductCategory() 
        {
            CategoryName = string.Empty;
        }
        public ProductCategory(string categoryName)
        {
            CategoryName = categoryName;
        }
    }
}
