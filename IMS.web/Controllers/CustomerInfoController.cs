using IMS.Infrastructure.IRepository;
using IMS.Modes.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Collections.Generic;

namespace IMS.web.Controllers
{
    public class CustomerInfoController : Controller
    {
        public readonly ICrudService<StoreInfo> _storeInfo;
        public readonly ICrudService<CustomerInfo> _customerInfo;
        public readonly UserManager<ApplicationUser> _userManager;

        public CustomerInfoController(ICrudService<StoreInfo> storeInfo, ICrudService<CustomerInfo> customerInfo, UserManager<ApplicationUser> userManager)
        {
            _storeInfo = storeInfo;
            _customerInfo = customerInfo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var storeInfo = await _storeInfo.GetAsync(user.StoreId);
            var categories = await _customerInfo.GetAllAsync(p => p.StoreInfoId == storeInfo.Id);
            return View(categories);
        }
        public async Task<IActionResult> AddEdit(int id)
        {
            CustomerInfo customerInfo = new CustomerInfo();
            if (id > 0)
            {
                customerInfo = await _customerInfo.GetAsync(id);
            }
            return View(customerInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(CustomerInfo customerInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);
                    if (customerInfo.Id == 0)
                    {
                        customerInfo.CreatedDate = DateTime.Now;
                        customerInfo.CreatedBy = userId;
                        customerInfo.StoreInfoId = user.StoreId;
                        await _customerInfo.InsertAsync(customerInfo);
                        TempData["success"] = "Data Added Successfully";
                    }
                    else
                    {
                        var OrgStoreInfo = await _customerInfo.GetAsync(customerInfo.Id);
                        OrgStoreInfo.CustomerName = customerInfo.CustomerName;
                        OrgStoreInfo.Email = customerInfo.Email;
                        OrgStoreInfo.PhoneNumber = customerInfo.PhoneNumber;
                        OrgStoreInfo.Address = customerInfo.Address;
                        OrgStoreInfo.PanNo = customerInfo.PanNo;


                        OrgStoreInfo.StoreInfoId = user.StoreId;

                        await _customerInfo.UpdateAsync(OrgStoreInfo);
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

        [HttpPost]
        public async Task<IActionResult> Remove(CustomerInfo customerInfo)
        {
            _customerInfo.Delete(customerInfo);
            TempData["error"] = "Data Deleted Successfully";
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [Route("/api/CustomerInfo/addCustomer")]
        public async Task<IActionResult> addCustomer(CustomerInfo model)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await  _userManager.FindByIdAsync(userId);

            model.CreatedBy = user.Id;
            model.CreatedDate = DateTime.Now;
            model.StoreInfoId = user.StoreId;

            var result = await _customerInfo.InsertAsync(model);
            model.Id = result;
            return Json(model);
        }

        [HttpPost]
        [Route("/api/CustomerInfo/getCustomer")]
        public async Task<IActionResult> getCustomer()
        {
            var customerListAll = await _customerInfo.GetAllAsync();
            return Json(customerListAll);
        }


    }
}
    
