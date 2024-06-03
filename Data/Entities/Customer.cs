using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Customer : BaseEntity
    {
        public int PersonId { get; set; }
        public int DiscountValue { get; set; }

        public Person Person { get; set; }
        public IList<Receipt> Receipts { get; set; }

        public Customer() { }
        public Customer(int personId, int discountValue)
        {
            PersonId = personId;
            DiscountValue = discountValue;
        }
    }
}
