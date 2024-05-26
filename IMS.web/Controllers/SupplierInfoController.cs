using IMS.Infrastructure.IRepository;
using IMS.Modes.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security;

namespace IMS.web.Controllers
{
    public class SupplierInfoController : Controller
    {
        public readonly ICrudService<StoreInfo> _storeInfo;
        public readonly ICrudService<SupplierInfo> _supplierInfo;
        public readonly UserManager<ApplicationUser> _userManager;

        public SupplierInfoController(ICrudService<StoreInfo> storeInfo, ICrudService<SupplierInfo> supplierInfo, UserManager<ApplicationUser> userManager)
        {
            _storeInfo = storeInfo;
            _supplierInfo = supplierInfo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var storeInfo = await _storeInfo.GetAsync(user.StoreId);
            var categories = await _supplierInfo.GetAllAsync(p => p.StoreInfoId == storeInfo.Id);
            return View(categories);
        }
        public async Task<IActionResult> AddEdit(int id)
        {
            SupplierInfo supplierInfo = new SupplierInfo();
            supplierInfo.IsActive = true;
            if (id > 0)
            {
                supplierInfo = await _supplierInfo.GetAsync(id);
            }
            return View(supplierInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(SupplierInfo supplierInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);
                    if (supplierInfo.Id == 0)
                    {

                        supplierInfo.IsActive = true;
                        supplierInfo.CreatedDate = DateTime.Now;
                        supplierInfo.CreatedBy = userId;
                        supplierInfo.StoreInfoId = user.StoreId;
                        await _supplierInfo.InsertAsync(supplierInfo);
                        TempData["success"] = "Data Added Successfully";
                    }
                    else
                    {
                        var OrgStoreInfo = await _supplierInfo.GetAsync(supplierInfo.Id);
                        OrgStoreInfo.SupplierName = supplierInfo.SupplierName;
                        OrgStoreInfo.ContactPerson = supplierInfo.ContactPerson;
                        OrgStoreInfo.PhoneNumber = supplierInfo.PhoneNumber;
                        OrgStoreInfo.Email = supplierInfo.Email;
                        OrgStoreInfo.Address = supplierInfo.Address;
                        OrgStoreInfo.IsActive = supplierInfo.IsActive;
                        OrgStoreInfo.ModifiedDate = DateTime.Now;
                        OrgStoreInfo.ModifiedBy = "";
                        OrgStoreInfo.StoreInfoId = user.StoreId;

                        await _supplierInfo.UpdateAsync(OrgStoreInfo);
                        TempData["success"] = "Data Updated Successfully";

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

        public async Task<IActionResult> Remove(int id)
        {
            return View(await _supplierInfo.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(SupplierInfo supplierInfo)
        {
            _supplierInfo.Delete(supplierInfo);
            TempData["error"] = "Data Deleted Successfully";
            return RedirectToAction(nameof(Index));

        }
    }
}
    
