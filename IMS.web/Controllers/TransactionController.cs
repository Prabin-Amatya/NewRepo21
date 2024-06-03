using IMS.Infrastructure.IRepository;
using IMS.Modes.Entity;
using IMS.Modes.ViewModels;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Net.Sockets;

namespace IMS.web.Controllers
{
    public class TransactionInfoController : Controller
    {
        public readonly ICrudService<CategoryInfo> _categoryInfo;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly ICrudService<StockInfo> _stockInfo;
        public readonly ICrudService<StoreInfo> _storeInfo;
        public readonly ICrudService<RackInfo> _rackInfo;
        public readonly ICrudService<SupplierInfo> _supplierInfo;
        public readonly ICrudService<TransactionInfo> _transactionInfo;
        public readonly ICrudService<UnitInfo> _unitInfo;
        public readonly ICrudService<CustomerInfo> _customerInfo;
        public readonly ICrudService<ProductInfo> _productInfo;
        public readonly ICrudService<ProductInvoiceInfo> _productInvoiceInfo;
        public readonly ICrudService<ProductInvoiceDetailInfo> _productInvoiceDetailInfo;
        public readonly ICrudService<ProductRateInfo> _productRateInfo;

        public TransactionInfoController(ICrudService<StoreInfo> storeInfo, ICrudService<CategoryInfo> categoryInfo, UserManager<ApplicationUser> userManager, ICrudService<StockInfo> stockInfo, ICrudService<RackInfo> rackInfo, ICrudService<SupplierInfo> supplierInfo, ICrudService<TransactionInfo> transactionInfo, ICrudService<UnitInfo> unitInfo, ICrudService<ProductInfo> productInfo, ICrudService<ProductRateInfo> productRateInfo, ICrudService<CustomerInfo> customerInfo = null, ICrudService<ProductInvoiceInfo> productInvoiceInfo = null, ICrudService<ProductInvoiceDetailInfo> productInvoiceDetailInfo = null)
        {
            _categoryInfo = categoryInfo;
            _userManager = userManager;
            _stockInfo = stockInfo;

            _storeInfo = storeInfo;
            _rackInfo = rackInfo;
            _supplierInfo = supplierInfo;
            _transactionInfo = transactionInfo;
            _unitInfo = unitInfo;
            _productInfo = productInfo;
            _productRateInfo = productRateInfo;
            _customerInfo = customerInfo;
            _productInvoiceInfo = productInvoiceInfo;
            _productInvoiceDetailInfo = productInvoiceDetailInfo;
            
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var store = await _storeInfo.GetAsync(user.StoreId);

            ViewBag.CustomerInfos = await _customerInfo.GetAllAsync(p => p.StoreInfoId == store.Id);
            var productInvoice = await _productInvoiceInfo.GetAllAsync(p=>p.StoreInfoId == store.Id);

            return View(productInvoice);
        }
        public async Task<IActionResult> Transaction(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var store = await _storeInfo.GetAsync(user.StoreId);

            TransactionViewModel transactionViewModel = new TransactionViewModel();

            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == store.Id);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == store.Id);

            ViewBag.ProductInvoiceInfos = await _productInvoiceInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == store.Id);
            ViewBag.RackInfos = await _rackInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == store.Id);
            ViewBag.SupplierInfos = await _supplierInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == store.Id);
            ViewBag.CustomerInfos = await _customerInfo.GetAllAsync();
            return View(transactionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductRateInfo productRateInfo)
        {
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.RackInfos = await _rackInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.SupplierInfos = await _supplierInfo.GetAllAsync(p => p.IsActive == true);

            productRateInfo.PurchasedDate = DateTime.Now;
            productRateInfo.ExpiryDate = DateTime.Today;
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);
                    var product = await _productInfo.GetAsync(productRateInfo.ProductInfoId);
                    if (productRateInfo.Id == 0)
                    {

                        productRateInfo.IsActive = true;
                        productRateInfo.CreatedDate = DateTime.Now;
                        productRateInfo.CreatedBy = userId;
                        productRateInfo.StoreInfoId = user.StoreId;
                        var rateInfoId = await _productRateInfo.InsertAsync(productRateInfo);

                        TransactionInfo transactioninfo = new TransactionInfo();

                        transactioninfo.TransactionType = "Purchase";
                        transactioninfo.CategoryInfoId = productRateInfo.CategoryInfoId;
                        transactioninfo.ProductInfoId = productRateInfo.ProductInfoId;
                        transactioninfo.Rate = productRateInfo.CostPrice;
                        transactioninfo.Amount = productRateInfo.Quantity * productRateInfo.CostPrice;
                        transactioninfo.Quantity = productRateInfo.Quantity;
                        transactioninfo.UnitInfoId = product.UnitInfoId;
                        transactioninfo.StoreInfoId = productRateInfo.StoreInfoId;
                        transactioninfo.ProductRateInfoId = productRateInfo.Id;
                        if (productRateInfo.IsActive)
                            transactioninfo.IsActive = "true";
                        else
                            transactioninfo.IsActive = "false";
                        transactioninfo.CreatedDate = DateTime.Now;
                        transactioninfo.CreatedBy = user.Id;
                        transactioninfo.ModifiedBy = "";
                        transactioninfo.StoreInfoId = user.StoreId;
                        await _transactionInfo.InsertAsync(transactioninfo);

                        var stockdet = await _stockInfo.GetAsync(p => p.ProductInfoId == productRateInfo.ProductInfoId);
                        if (stockdet == null)
                        {

                            StockInfo stockInfo = new StockInfo();
                            stockInfo.CategoryInfoId = productRateInfo.CategoryInfoId;
                            stockInfo.ProductInfoId = productRateInfo.ProductInfoId;
                            stockInfo.ProductRateInfoId = rateInfoId;
                            stockInfo.Quantity = productRateInfo.Quantity;
                            stockInfo.IsActive = productRateInfo.IsActive;
                            stockInfo.CreatedDate = DateTime.Now;
                            stockInfo.CreatedBy = user.Id;
                            stockInfo.StoreInfoId = user.StoreId;
                            await _stockInfo.InsertAsync(stockInfo);
                        }
                        else
                        {
                            var qty = stockdet.Quantity + productRateInfo.Quantity;
                            stockdet.Quantity += qty;
                            stockdet.ModifiedDate = DateTime.Now;
                            stockdet.ModifiedBy = user.Id;
                            stockdet.StoreInfoId = user.StoreId;
                        }

                        TempData["success"] = "Data Added Successfully";

                    }
                    else
                    {
                        var OrgStoreInfo = await _productRateInfo.GetAsync(productRateInfo.Id);

                        var productId = OrgStoreInfo.ProductInfoId;
                        var newproductId = productRateInfo.ProductInfoId;

                        var changeqty = OrgStoreInfo.Quantity - productRateInfo.Quantity;
                        var orgqty = OrgStoreInfo.Quantity;
                        var newqty = productRateInfo.Quantity;
                        var remqty = OrgStoreInfo.RemainingQuantity - OrgStoreInfo.Quantity + productRateInfo.Quantity;

                        OrgStoreInfo.CategoryInfoId = productRateInfo.CategoryInfoId;
                        OrgStoreInfo.ProductInfoId = productRateInfo.ProductInfoId;
                        OrgStoreInfo.RackInfoId = productRateInfo.RackInfoId;
                        OrgStoreInfo.CostPrice = productRateInfo.CostPrice;
                        OrgStoreInfo.SellingPrice = productRateInfo.SellingPrice;
                        OrgStoreInfo.Quantity = productRateInfo.Quantity;
                        OrgStoreInfo.BatchNo = productRateInfo.BatchNo;
                        OrgStoreInfo.RemainingQuantity = remqty;
                        OrgStoreInfo.PurchasedDate = productRateInfo.PurchasedDate;
                        OrgStoreInfo.SupplierInfoId = productRateInfo.SupplierInfoId;
                        OrgStoreInfo.ExpiryDate = productRateInfo.ExpiryDate;
                        OrgStoreInfo.IsActive = productRateInfo.IsActive;
                        OrgStoreInfo.ModifiedDate = DateTime.Now;
                        OrgStoreInfo.ModifiedBy = user.Id;
                        OrgStoreInfo.StoreInfoId = user.StoreId;

                        await _productRateInfo.UpdateAsync(OrgStoreInfo);
                        TempData["success"] = "Data Updated Successfully";

                        TransactionInfo transactioninfo = new TransactionInfo();
                        transactioninfo.TransactionType = "Purchase";
                        transactioninfo.CategoryInfoId = productRateInfo.CategoryInfoId;
                        transactioninfo.ProductInfoId = productRateInfo.ProductInfoId;
                        transactioninfo.Rate = productRateInfo.CostPrice;
                        transactioninfo.Amount = productRateInfo.Quantity * productRateInfo.CostPrice;
                        transactioninfo.Quantity = productRateInfo.Quantity;
                        transactioninfo.UnitInfoId = productRateInfo.UnitInfoId;
                        transactioninfo.StoreInfoId = productRateInfo.StoreInfoId;
                        transactioninfo.ProductRateInfoId = productRateInfo.Id;
                        if (productRateInfo.IsActive)
                            transactioninfo.IsActive = "true";
                        else
                            transactioninfo.IsActive = "false";
                        transactioninfo.ModifiedDate = DateTime.Now;
                        transactioninfo.ModifiedBy = user.Id;
                        transactioninfo.StoreInfoId = user.StoreId;
                        await _transactionInfo.InsertAsync(transactioninfo);




                        if (productId == productRateInfo.ProductInfoId)
                        {
                            if (changeqty != 0)
                            {
                                var stockdet = await _stockInfo.GetAsync(p => p.ProductInfoId == productRateInfo.ProductInfoId);

                                var qty = stockdet.Quantity + changeqty;
                                stockdet.Quantity = qty;
                                stockdet.ModifiedDate = DateTime.Now;
                                stockdet.ModifiedBy = user.Id;
                                await _stockInfo.UpdateAsync(stockdet);
                            }
                        }
                        else
                        {
                            var ostockdet = await _stockInfo.GetAsync(p => p.ProductInfoId == productId);
                            var uqty = ostockdet.Quantity - orgqty;
                            ostockdet.Quantity = uqty;
                            ostockdet.ModifiedDate = DateTime.Now;
                            ostockdet.ModifiedBy = user.Id;
                            ostockdet.StoreInfoId = user.StoreId;


                            var stockInfo = await _stockInfo.GetAsync(p => p.ProductInfoId == newproductId);
                            if (stockInfo == null)
                            {
                                stockInfo.CategoryInfoId = productRateInfo.CategoryInfoId;
                                stockInfo.ProductInfoId = productRateInfo.ProductInfoId;
                                stockInfo.ProductRateInfoId = productRateInfo.Id;
                                stockInfo.Quantity = productRateInfo.Quantity;
                                stockInfo.IsActive = productRateInfo.IsActive;
                                stockInfo.ModifiedDate = DateTime.Now;
                                stockInfo.ModifiedBy = user.Id;
                                stockInfo.StoreInfoId = user.StoreId;
                                await _stockInfo.InsertAsync(stockInfo);
                            }
                            else
                            {

                                uqty = stockInfo.Quantity + orgqty;
                                stockInfo.Quantity = uqty;
                                stockInfo.ModifiedDate = DateTime.Now;
                                stockInfo.ModifiedBy = user.Id;
                                await _stockInfo.InsertAsync(stockInfo);
                            }


                        }


                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Something went wrong, Please try again later";
                    return RedirectToAction(nameof(AddEdit));
                }
            }
            TempData["error"] = "Please Input Valid Data";
            return RedirectToAction(nameof(AddEdit));
        }

        [HttpPost]
        [Route("/api/Transaction/getBatch")]
        public async Task<IActionResult> getBatch(int ProductId)
        {
            var BatchList = await _productRateInfo.GetAllAsync(p => p.ProductInfoId == ProductId);
            return Json(new { BatchList });

        }
        [HttpPost]
        [Route("/api/Transaction/getUnit")]
        public async Task<IActionResult> getUnit(int ProductId)
        {
            var product = await _productInfo.GetAsync(ProductId);
            return Json(new { product });
        }

        [HttpPost]
        [Route("/api/Transaction/getProduct")]
        public async Task<IActionResult> getProduct(int CategoryId)
        {
            var productList = await _productInfo.GetAllAsync(p => p.CategoryInfoId == CategoryId);
            return Json(new { productList });
        }

        [HttpPost]
        [Route("/api/Transaction/getProductRate")]
        public async Task<IActionResult> getProductRate(int ProductRateId)
        {
            var productDetail = await _productRateInfo.GetAsync(ProductRateId);
            return Json(new { productDetail });
        }

        [HttpPost]
        [Route("/api/Transaction/saveTransaction")]
        public async Task<int> saveTransaction(TransactionViewModel transactionViewModel)
        {
            try
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                var user = await _userManager.FindByIdAsync(userId);

                //var InvoiceNo = 0;
                //var productinvoiceinfo = await _productInvoiceInfo.GetAsync();
                transactionViewModel.ProductInvoiceInfo.CustomerInfoId = transactionViewModel.ProductInvoiceInfo.CustomerInfoId;
                transactionViewModel.ProductInvoiceInfo.CreatedBy = userId;
                transactionViewModel.ProductInvoiceInfo.TransactionDate = DateTime.Now;
                transactionViewModel.ProductInvoiceInfo.CreatedDate = DateTime.Now;
                transactionViewModel.ProductInvoiceInfo.StoreInfoId = user.StoreId;
                transactionViewModel.ProductInvoiceInfo.BillStatus = 1;
                transactionViewModel.ProductInvoiceInfo.IsActive = true;
                transactionViewModel.ProductInvoiceInfo.InvoiceNo = "1";
                var invoiceId = await _productInvoiceInfo.InsertAsync(transactionViewModel.ProductInvoiceInfo);

                if (transactionViewModel.ProductInvoiceDetailInfos.Count() > 0)
                {

                    foreach (var items in transactionViewModel.ProductInvoiceDetailInfos)
                    {
                        //ProductInvoiceDetail
                        ProductInvoiceDetailInfo productInvoiceDetailInfo = new ProductInvoiceDetailInfo();
                        productInvoiceDetailInfo.ProductInvoiceInfoId = invoiceId;
                        productInvoiceDetailInfo.ProductRateInfoId = items.ProductRateInfoId;
                        productInvoiceDetailInfo.Rate = items.Rate;
                        productInvoiceDetailInfo.Quantity = items.Quantity;
                        productInvoiceDetailInfo.Amount = items.Amount;
                        productInvoiceDetailInfo.CreatedBy = userId;
                        productInvoiceDetailInfo.CreatedDate = DateTime.Now;
                        await _productInvoiceDetailInfo.InsertAsync(productInvoiceDetailInfo);



                        //transactionInfo
                        var productRateInfo = await _productRateInfo.GetAsync(items.ProductRateInfoId);
                        var product = await _productInfo.GetAsync(productRateInfo.ProductInfoId);

                        TransactionInfo transactioninfo = new TransactionInfo();
                        transactioninfo.TransactionType = "Sell";
                        transactioninfo.CategoryInfoId = productRateInfo.CategoryInfoId;
                        transactioninfo.ProductInfoId = productRateInfo.ProductInfoId;
                        transactioninfo.Rate = items.Rate;
                        transactioninfo.Amount = items.Amount;
                        transactioninfo.Quantity = items.Quantity;
                        transactioninfo.UnitInfoId = product.UnitInfoId;
                        transactioninfo.StoreInfoId = productRateInfo.StoreInfoId;
                        transactioninfo.ProductRateInfoId = productRateInfo.Id;
                        if (productRateInfo.IsActive)
                            transactioninfo.IsActive = "true";
                        else
                            transactioninfo.IsActive = "false";
                        transactioninfo.CreatedDate = DateTime.Now;
                        transactioninfo.CreatedBy = user.Id;
                        transactioninfo.ModifiedBy = "";
                        transactioninfo.StoreInfoId = user.StoreId;
                        await _transactionInfo.InsertAsync(transactioninfo);



                        //Product Rate
                        productRateInfo.SoldQuantity += items.Quantity;
                        productRateInfo.RemainingQuantity -= items.Quantity;
                        productRateInfo.ModifiedBy = userId;
                        productRateInfo.ModifiedDate = DateTime.Now;
                        await _productRateInfo.UpdateAsync(productRateInfo);



                        //Stock
                        var stockdet = await _stockInfo.GetAsync(p => p.ProductRateInfoId == productRateInfo.ProductInfoId && p.StoreInfoId == user.StoreId);
                        var qty = stockdet.Quantity - items.Quantity;
                        stockdet.Quantity = qty;
                        stockdet.ModifiedBy = userId;
                        stockdet.ModifiedDate = DateTime.Now;
                        await _stockInfo.UpdateAsync(stockdet);
                    }
                }
                return invoiceId;
            }
            catch(Exception ex) {
                return 1;
            }   
            
         }

        [HttpGet]
        public async Task<IActionResult> PrintReport(int Id)
        {


            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync();
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync();
            ViewBag.CustomerInfos = await _customerInfo.GetAllAsync();
            ViewBag.ProductRateInfos = await _productRateInfo.GetAllAsync(p => p.IsActive == true);

           

            var invoiceInfo = await _productInvoiceInfo.GetAsync(Id);
            var user = await _userManager.FindByIdAsync(invoiceInfo.CreatedBy);
            ViewBag.TaxCreater = user.FirstName+' '+user.MiddleName+' '+user.LastName + ' ';
            

            TransactionViewModel transactionViewModel = new TransactionViewModel();
            transactionViewModel.ProductInvoiceInfo = invoiceInfo; 
            transactionViewModel.StoreInfo =await _storeInfo.GetAsync(user.StoreId);  
            transactionViewModel.ProductInvoiceDetailInfos = await _productInvoiceDetailInfo.GetAllAsync(p => p.ProductInvoiceInfoId == Id);
            ViewBag.TotalAmount = ConvertMoneyToWords((int)transactionViewModel.ProductInvoiceInfo.TotalAmount);


            return View(transactionViewModel);
        }
        public static string ConvertMoneyToWords(int amount)
        {
            if (amount == 0)
                return "zero rupees";

            string rupeeWords = ConvertNumberToWords(amount) + " rupee" + (amount == 1 ? "" : "s");
            return rupeeWords;
        }

        private static string ConvertNumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            string[] unitsMap = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tensMap = { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            if (number < 0)
                return "minus " + ConvertNumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 10000000) > 0)
            {
                words += ConvertNumberToWords(number / 10000000) + " crore ";
                number %= 10000000;
            }

            if ((number / 100000) > 0)
            {
                words += ConvertNumberToWords(number / 100000) + " lakh ";
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words += ConvertNumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += ConvertNumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words.Trim();
        }

        [HttpPost]
        [Route("/api/Transaction/CancelTransaction")]
        public async Task<IActionResult> CancelTransaction(int invoiceId, string cancellationRemarks)
        {

            var userId = _userManager.GetUserId(HttpContext.User);

            var productinvoiceinfo = await _productInvoiceInfo.GetAsync(invoiceId);

            productinvoiceinfo.BillStatus = 2;
            productinvoiceinfo.CancellationRemarks = cancellationRemarks;
            productinvoiceinfo.ModifiedDate = productinvoiceinfo.ModifiedDate = DateTime.Now;
            productinvoiceinfo.ModifiedBy = userId;
            await _productInvoiceInfo.UpdateAsync(productinvoiceinfo);
            return Json(1);
        }
    }


}



