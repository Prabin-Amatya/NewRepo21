using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Modes.Entity
{
    public class SupplierInfo:BaseEntity
    {

        [Required]
        public string SupplierName { get; set; }

        [Required]

        public string ContactPerson { get; set; }

        [Required]

        public string PhoneNumber { get; set; }

        [Required]

        public string Address { get; set; }

        [Required]
        public string Email { get; set; }
        public int StoreInfoId { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }


        public string? ModifiedBy { get; set; }
        public virtual ICollection<ProductRateInfo> ProductRateInfos { get; set; }

        public virtual StoreInfo StoreInfo { get; set; }
    }

}
