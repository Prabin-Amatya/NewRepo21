using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Modes.Entity
{
    public class ProductInvoiceDetailInfo : BaseEntity
    {

        public int ProductInvoiceInfoId { get; set; }
        public int ProductRateInfoId { get; set; }

        [Required]
        public float Rate { get; set; }

        [Required]
        public float Quantity { get; set; }

        [Required]
        public float Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public virtual ProductInvoiceInfo ProductInvoiceInfo { get; set; }
        public virtual ProductRateInfo ProductRateInfo{get; set;}
    }
}
