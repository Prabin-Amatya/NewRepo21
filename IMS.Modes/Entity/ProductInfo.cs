﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Modes.Entity
{
    public class ProductInfo:BaseEntity
    {
        public int CategoryInfoId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public int UnitInfoId { get; set; }

        public int StoreInfoId { get; set; }

        public float Quantity { get; set; }
        public float SoldQuantity { get; set; }

        public float RemainingQuantity { get; set; }

        public string BatchNo { get; set; }
        public DateTime PurchasedDate { get; set; }

        public DateTime ExpiryDate { get; set; }
        public int SupplierInfoId { get; set; }


        public int RackInfoId { get; set; }
        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual UnitInfo UnitInfo { get; set; }
        public virtual StoreInfo StoreInfo { get; set; }
        public virtual CategoryInfo CategoryInfo { get; set; }
         
        public virtual SupplierInfo SupplierInfo { get; set; }

        public virtual RackInfo RackInfo { get; set; }
        public virtual ICollection<ProductRateInfo> ProductRateInfos { get; set; }
        public virtual ICollection<TransactionInfo> TransactionInfos { get; set; }

        public virtual ICollection<StockInfo> StockInfos { get; set; }


    }
}