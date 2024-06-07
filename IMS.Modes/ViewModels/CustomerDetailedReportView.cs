using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Modes.ViewModels
{
    [Keyless]
    public class CustomerDetailedReportViewModel
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int PaymentMethod { get; set; }
        public string CustomerName { get; set; }
        public double Amount { get; set; }

        public string CategoryName { get; set; }
        public string ProductName { get; set; }

        public double Quantity { get; set; }

        public string UnitName { get; set; }


        public string SupplierName { get; set; }
    }
}
