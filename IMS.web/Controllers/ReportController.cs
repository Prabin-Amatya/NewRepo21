using IMS.Infrastructure.IRepository;
using IMS.Modes.Entity;
using IMS.Modes.ViewModels;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NuGet.DependencyResolver;

namespace IMS.web.Controllers
{
    public class ReportController : Controller
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly ICrudService<SupplierInfo> _supplierInfo;
        public readonly ICrudService<CustomerInfo> _customerInfo;
        public readonly ICrudService<ProductInfo> _productInfo;
        public readonly ICrudService<CategoryInfo> _categoryInfo;
        public readonly ICrudService<StockInfo> _stockInfo;
        public readonly IRawSqlRepository _rawSqlRepository;

        public ReportController(UserManager<ApplicationUser> userManager, ICrudService<SupplierInfo> supplierInfo, ICrudService<CustomerInfo> customerInfo, IRawSqlRepository rawSqlRepository, ICrudService<ProductInfo> productInfo, ICrudService<CategoryInfo> categoryInfo, ICrudService<StockInfo> stockInfo)
        {
            _userManager = userManager;
            _supplierInfo = supplierInfo;
            _customerInfo = customerInfo;
            _rawSqlRepository = rawSqlRepository;
            _productInfo = productInfo;
            _categoryInfo = categoryInfo;
            _stockInfo = stockInfo;
        }

        public async Task<IActionResult> Index(ReportViewModel reportviewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.supplierInfo =await _supplierInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.customerInfo =await _customerInfo.GetAllAsync(p=>p.StoreInfoId == user.StoreId);


            reportviewModel.search = new SearchCriteria();
            return View(reportviewModel);
        }


        public async Task<IActionResult> Search(ReportViewModel reportviewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.supplierInfo = await _supplierInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.customerInfo = await _customerInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);

            var result = _rawSqlRepository.FromSql<CustomerReportViewModel>($"usp_GetTransactionInfo @customerId, @PaymentMethodId, @startDate, @endDate, @storeId",
              new SqlParameter("@customerId", reportviewModel.search.CustomerId == null ? (object)DBNull.Value : reportviewModel.search.CustomerId),
              new SqlParameter("@PaymentMethodId", reportviewModel.search.PaymentMethod == null ? (object)DBNull.Value : reportviewModel.search.PaymentMethod),
              new SqlParameter("@startDate", reportviewModel.search.StartDate == null ? (object)DBNull.Value : reportviewModel.search.StartDate),
              new SqlParameter("@endDate", reportviewModel.search.EndDate == null ? (object)DBNull.Value : reportviewModel.search.EndDate),
              new SqlParameter("@storeId", user.StoreId)
              ).ToList();

            reportviewModel.CustomerReportViewModels = result;
            return View(nameof(Index), reportviewModel);
        }

       

        public async Task<IActionResult> DetailedReport(ReportViewModel reportviewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.supplierInfo = await _supplierInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.customerInfo = await _customerInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            ViewBag.productInfo = await _productInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.categoryInfo = await _categoryInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);

            reportviewModel.search = new SearchCriteria();
            return View(reportviewModel);
        }

        


        public async Task<IActionResult> SearchDetailed(ReportViewModel reportviewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.supplierInfo = await _supplierInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.customerInfo = await _customerInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            ViewBag.productInfo = await _productInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.categoryInfo = await _categoryInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);

            var result = _rawSqlRepository.FromSql<CustomerDetailedReportViewModel>($"usp_GetDetailTransactionInfo @customerId, @supplierId, @productId, @categoryId, @startDate, @endDate, @PaymentMethodId, @storeId",
              new SqlParameter("@customerId", reportviewModel.search.CustomerId == null ? (object)DBNull.Value : reportviewModel.search.CustomerId),
              new SqlParameter("@supplierId", reportviewModel.search.SupplierId == null ? (object)DBNull.Value : reportviewModel.search.SupplierId),
              new SqlParameter("@productId", reportviewModel.search.ProductId == null ? (object)DBNull.Value : reportviewModel.search.ProductId),
              new SqlParameter("@categoryId", reportviewModel.search.CategoryId == null ? (object)DBNull.Value : reportviewModel.search.CategoryId),
              new SqlParameter("@startDate", reportviewModel.search.StartDate == null ? (object)DBNull.Value : reportviewModel.search.StartDate),
              new SqlParameter("@endDate", reportviewModel.search.EndDate == null ? (object)DBNull.Value : reportviewModel.search.EndDate),
              new SqlParameter("@PaymentMethodId", reportviewModel.search.PaymentMethod == null ? (object)DBNull.Value : reportviewModel.search.PaymentMethod),
              new SqlParameter("@storeId", user.StoreId)
              ).ToList();

            reportviewModel.CustomerDetailedReportViewModels = result;
            return View(nameof(DetailedReport), reportviewModel);
        }

        public async Task<IActionResult> PrintDetailedReport(int? customerId, int? supplierId, int? productId, int? categoryId, DateTime? startDate, DateTime? endDate, int? PaymentMethodId)
        {
            ReportViewModel reportViewModel = new ReportViewModel();

            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);

            var result = _rawSqlRepository.FromSql<CustomerDetailedReportViewModel>($"usp_GetDetailTransactionInfo @customerId, @supplierId, @productId, @categoryId, @startDate, @endDate, @PaymentMethodId, @storeId",
              new SqlParameter("@customerId", customerId == null ? (object)DBNull.Value : customerId),
              new SqlParameter("@supplierId", supplierId == null ? (object)DBNull.Value : supplierId),
              new SqlParameter("@productId", productId == null ? (object)DBNull.Value : productId),
              new SqlParameter("@categoryId", categoryId == null ? (object)DBNull.Value : categoryId),
              new SqlParameter("@startDate", startDate == null ? (object)DBNull.Value : startDate),
              new SqlParameter("@endDate", endDate == null ? (object)DBNull.Value : endDate),
              new SqlParameter("@PaymentMethodId", PaymentMethodId == null ? (object)DBNull.Value : PaymentMethodId),
              new SqlParameter("@storeId", user.StoreId)
              ).ToList();

            reportViewModel.CustomerDetailedReportViewModels = result;
            return View(result);
        }
        public async Task<IActionResult> Stock(StockViewModel stockViewModel)
        {

            
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);

            ViewBag.productInfo = await _productInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.categoryInfo = await _categoryInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            var stock = await _stockInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);

            if (stockViewModel.categoryId != null)
            {
                stock = stock.Where(p => p.CategoryInfoId == stockViewModel.categoryId).ToList();
            }
            if (stockViewModel.productId != null)
            {
                stock = stock.Where(p => p.ProductInfoId == stockViewModel.productId).ToList();
            }

            stockViewModel.StockInfos = stock;

            return View(stockViewModel);
        }

        public async Task<IActionResult> StockReport(int? categoryId, int? productId)
        {


            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);

            ViewBag.productInfo = await _productInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.categoryInfo = await _categoryInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            var stock = await _stockInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);

            if (categoryId != null)
            {
                stock = stock.Where(p => p.CategoryInfoId == categoryId).ToList();
            }
            if (productId != null)
            {
                stock = stock.Where(p => p.ProductInfoId == productId).ToList();
            }

            

            return View(stock);
        }



    }
}
