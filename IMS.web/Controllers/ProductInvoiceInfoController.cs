using IMS.Infrastructure.IRepository;
using IMS.Modes.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IMS.web.Controllers
{
    public class ProductInvoiceInfoController : Controller
    {
        public readonly ICrudService<StoreInfo> _storeInfo;
        public readonly ICrudService<CategoryInfo> _categoryInfo;
        public readonly UserManager<ApplicationUser> _userManager;

        public ProductInvoiceInfoController(ICrudService<StoreInfo> storeInfo, ICrudService<CategoryInfo> categoryInfo, UserManager<ApplicationUser> userManager)
        {
            _storeInfo = storeInfo;
            _categoryInfo = categoryInfo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var storeInfo = await _storeInfo.GetAsync(user.StoreId);
            var categories = await _categoryInfo.GetAllAsync(p => p.StoreInfoId == storeInfo.Id);
            return View(categories);
        }
        public async Task<IActionResult> AddEdit(int id)
        {
            CategoryInfo categoryInfo = new CategoryInfo();
            categoryInfo.IsActive = true;
            if (id > 0)
            {
                categoryInfo = await _categoryInfo.GetAsync(id);
            }
            return View(categoryInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(CategoryInfo categoryInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);
                    if (categoryInfo.Id == 0)
                    {

                        categoryInfo.IsActive = true;
                        categoryInfo.CreatedDate = DateTime.Now;
                        categoryInfo.CreatedBy = userId;
                        categoryInfo.StoreInfoId = user.StoreId;
                        await _categoryInfo.InsertAsync(categoryInfo);
                        TempData["success"] = "Data Added Successfully";
                    }
                    else
                    {
                        var OrgStoreInfo = await _categoryInfo.GetAsync(categoryInfo.Id);
                        OrgStoreInfo.CategoryName = categoryInfo.CategoryName;
                        OrgStoreInfo.CategoryDescription = categoryInfo.CategoryDescription;
                        OrgStoreInfo.IsActive = categoryInfo.IsActive;
                        OrgStoreInfo.ModifiedDate = DateTime.Now;
                        OrgStoreInfo.ModifiedBy = "";
                        OrgStoreInfo.StoreInfoId = user.StoreId;

                        await _categoryInfo.UpdateAsync(OrgStoreInfo);
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
            return View(await _categoryInfo.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(CategoryInfo categoryInfo)
        {
            await _categoryInfo.DeleteAsync(categoryInfo);
            TempData["error"] = "Data Deleted Successfully";
            return RedirectToAction(nameof(Index));

        }
    }
}
    
