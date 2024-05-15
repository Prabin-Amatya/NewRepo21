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
        public int CatergoryInfoId {  get; set; }
        public int ProductInfoId { get; set; }
        public int UnitInfoId { get; set; }
        public int StoreInfoId { get; set; }
        public float rate {  get; set; }
        public double Amount {  get; set; }
        public float Qualtity { get; set; }
        

    }
}
