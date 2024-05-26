using IMS.Infrastructure.IRepository;
using IMS.Modes.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IMS.web.Controllers
{
    public class RackInfoController : Controller
    {
        public readonly ICrudService<StoreInfo> _storeInfo;
        public readonly ICrudService<RackInfo> _rackInfo;
        public readonly UserManager<ApplicationUser> _userManager;

        public RackInfoController(ICrudService<StoreInfo> storeInfo, ICrudService<RackInfo> rackInfo, UserManager<ApplicationUser> userManager)
        {
            _storeInfo = storeInfo;
            _rackInfo = rackInfo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var storeInfo = await _storeInfo.GetAsync(user.StoreId);
            var categories = await _rackInfo.GetAllAsync(p => p.StoreInfoId == storeInfo.Id);
            return View(categories);
        }
        public async Task<IActionResult> AddEdit(int id)
        {
            RackInfo rackInfo = new RackInfo();
            rackInfo.IsActive = true;
            if (id > 0)
            {
                rackInfo = await _rackInfo.GetAsync(id);
            }
            return View(rackInfo);
        }

        [HttpPost]

        public async Task<IActionResult> AddEdit(RackInfo rackInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);
                    if (rackInfo.Id == 0)
                    {

                        rackInfo.IsActive = true;
                        rackInfo.CreatedDate = DateTime.Now;
                        rackInfo.CreatedBy = userId;
                        rackInfo.StoreInfoId = user.StoreId;
                        await _rackInfo.InsertAsync(rackInfo);
                        TempData["success"] = "Data Added Successfully";
                    }
                    else
                    {
                        var OrgStoreInfo = await _rackInfo.GetAsync(rackInfo.Id);
                        OrgStoreInfo.RackName = rackInfo.RackName;
                        OrgStoreInfo.IsActive = rackInfo.IsActive;
                        OrgStoreInfo.ModifiedDate = DateTime.Now;
                        OrgStoreInfo.ModifiedBy = "";
                        OrgStoreInfo.StoreInfoId = user.StoreId;

                        await _rackInfo.UpdateAsync(OrgStoreInfo);
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
            return View(await _rackInfo.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(RackInfo rackInfo)
        {
            _rackInfo.Delete(rackInfo);
            TempData["error"] = "Data Deleted Successfully";
            return RedirectToAction(nameof(Index));

        }
    }
}
    
