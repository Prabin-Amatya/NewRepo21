using IMS.Modes.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Modes.ViewModels
{
    public class TransactionViewModel:TransactionInfo
    { 
        public ProductInvoiceInfo ProductInvoiceInfo { get; set; }
        public IEnumerable<ProductInvoiceInfo> ProductInvoiceInfos {  get; set; }
        public ProductInvoiceDetailInfo ProductInvoiceDetailInfo { get; set; }
        public IEnumerable<ProductInvoiceDetailInfo> ProductInvoiceDetailInfos { get; set; }
        public CustomerInfo CustomerInfo { get; set; }

        [Required]
        public int PaymentMethod { get; set; }

        [Required]
        public string InvoiceNo { get; set; }

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
        public string CancellationRemarks { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }


        public string? ModifiedBy { get; set; }
    }
}
