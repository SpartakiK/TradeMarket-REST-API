using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class CustomerActivityModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal ReceiptSum { get; set; }

        public CustomerActivityModel()
        {
        }
        public CustomerActivityModel(int customerid, string customerName, decimal recieptSum)
        {
            CustomerId = customerid;
            CustomerName = customerName;
            ReceiptSum = recieptSum;
        }
    }
}
