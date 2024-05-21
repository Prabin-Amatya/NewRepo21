using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Modes.Entity
{
    public class ProductInvoiceInfo:BaseEntity
    {

        [Required]
        public int PaymentMethod {  get; set; }

        [Required]
        public string InvoiceNo {  get; set; }

        [Required]
        public int StoreInfoId { get; set; }

        public int CustomerInfoId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public float NetAmount { get; set; }

        [Required]
        public float DiscountAmount { get; set; }

        [Required]
        public float TotalAmount { get; set; }
        public string Remarks { get; set; }

        public int BillStatus { get; set; }
        public string CancellationRemarks {  get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }


        public string? ModifiedBy { get; set; }
        public virtual ICollection<ProductInvoiceDetailInfo> ProductInvoiceDetailInfos { get; set; }
        public virtual StoreInfo StoreInfo { get; set; }
        public virtual CustomerInfo CustomerInfo { get; set; }
    }
}
