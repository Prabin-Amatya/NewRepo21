using IMS.Infrastructure.IRepository;
using IMS.Modes.Entity;
using IMS.Modes.ViewModels;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
    public class ReportController : Controller
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly ICrudService<SupplierInfo> _supplierInfo;
        public readonly ICrudService<CustomerInfo> _customerInfo;
        public readonly IRawSqlRepository _rawSqlRepository;

        public ReportController(UserManager<ApplicationUser> userManager, ICrudService<SupplierInfo> supplierInfo, ICrudService<CustomerInfo> customerInfo, IRawSqlRepository rawSqlRepository)
        {
            _userManager = userManager;
            _supplierInfo = supplierInfo;
            _customerInfo = customerInfo;
            _rawSqlRepository = rawSqlRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.supplierInfo =await _supplierInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);

            ViewBag.customerInfo =await _customerInfo.GetAllAsync(p=>p.StoreInfoId == user.StoreId);

            ReportViewModel reportviewModel = new ReportViewModel();
            reportviewModel.search = new SearchCriteria();
            return View(reportviewModel);
        }
    }
}
