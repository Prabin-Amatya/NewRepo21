using IMS.Infrastructure.IRepository;
using IMS.Modes.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IMS.web.Controllers
{
    public class UnitInfoController : Controller
    {
        public readonly ICrudService<StoreInfo> _storeInfo;
        public readonly ICrudService<UnitInfo> _unitInfo;
        public readonly ICrudService<ProductInfo> _productInfo;
        public readonly UserManager<ApplicationUser> _userManager;

        public UnitInfoController(ICrudService<StoreInfo> storeInfo, ICrudService<UnitInfo> unitInfo, UserManager<ApplicationUser> userManager, ICrudService<ProductInfo> productInfo)
        {
            _storeInfo = storeInfo;
            _unitInfo = unitInfo;
            _userManager = userManager;
            _productInfo = productInfo;
        }

        public async Task<IActionResult> Index()
        {
            var units = await _unitInfo.GetAllAsync();
            return View(units);
        }
        public async Task<IActionResult> AddEdit(int id)
        {
            UnitInfo unitInfo = new UnitInfo();
            unitInfo.IsActive = true;
            if (id > 0)
            {
                unitInfo = await _unitInfo.GetAsync(id);
            }
            return View(unitInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(UnitInfo unitInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);
                    if (unitInfo.Id == 0)
                    {

                        unitInfo.IsActive = true;
                        unitInfo.CreatedDate = DateTime.Now;
                        unitInfo.CreatedBy = userId;
                        await _unitInfo.InsertAsync(unitInfo);
                        TempData["success"] = "Data Added Successfully";
                    }
                    else
                    {
                        var OrgStoreInfo = await _unitInfo.GetAsync(unitInfo.Id);
                        OrgStoreInfo.UnitName = unitInfo.UnitName;
                        OrgStoreInfo.IsActive = unitInfo.IsActive;
                        OrgStoreInfo.ModifiedDate = DateTime.Now;
                        OrgStoreInfo.ModifiedBy = "";

                        await _unitInfo.UpdateAsync(OrgStoreInfo);
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
            return View(await _unitInfo.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(UnitInfo unitInfo)
        {
            _unitInfo.Delete(unitInfo);
            TempData["error"] = "Data Deleted Successfully";
            return RedirectToAction(nameof(Index));

        }
    }
}
    
