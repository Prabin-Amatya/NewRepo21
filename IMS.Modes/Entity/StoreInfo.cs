using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Modes.Entity
{
    public class StoreInfo:BaseEntity
    {
        [Required(ErrorMessage = "Please Input Store Name")]
        [Display(Name="*Store Name")]
        public string StoreName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [Required]
        public string RegistrationNo {  get; set; }
        public string PanNo {  get; set; }
        public string IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

    }
}
