using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Modes.Entity
{
    public class TransactionInfo:BaseEntity
    {
        public string TransactionType { get; set; }
        public int CategoryInfoId {  get; set; }
        public int ProductInfoId { get; set; }
        public int UnitInfoId { get; set; }
        public int StoreInfoId { get; set; }
        public float Rate {  get; set; }
        public double Amount {  get; set; }
        public float Qualtity { get; set; }
        public virtual ICollection<ProductRateInfo> ProductRateInfos { get; set; }
        public virtual CategoryInfo CategoryInfo { get; set; }
        public virtual ProductInfo ProductInfo { get; set; }
        public virtual UnitInfo UnitInfo { get; set; }
        public virtual StoreInfo StoreInfo { get; set; }
    }
}
