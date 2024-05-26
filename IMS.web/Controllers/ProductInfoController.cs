using IMS.Infrastructure.IRepository;
using IMS.Modes.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;

namespace IMS.web.Controllers
{
    public class ProductInfoController : Controller
    {
        public readonly ICrudService<StoreInfo> _storeInfo;
        public readonly ICrudService<ProductInfo> _productInfo;
        public readonly ICrudService<CategoryInfo> _categoryInfo;
        public readonly ICrudService<UnitInfo> _unitInfo;
        public readonly UserManager<ApplicationUser> _userManager;

        public ProductInfoController(ICrudService<StoreInfo> storeInfo, ICrudService<ProductInfo> productInfo, UserManager<ApplicationUser> userManager, ICrudService<CategoryInfo> categoryInfo, ICrudService<UnitInfo> unitInfo)
        {
            _storeInfo = storeInfo;
            _productInfo = productInfo;
            _userManager = userManager;
            _unitInfo = unitInfo;
            _categoryInfo = categoryInfo;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p=>p.IsActive == true);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            var products = await _productInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            return View(products);
        }
        public async Task<IActionResult> AddEdit(int id)
        {

            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            ProductInfo productInfo = new ProductInfo();
            productInfo.IsActive = true;
            if (id > 0)
            {
                productInfo = await _productInfo.GetAsync(id);

            }
            return View(productInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductInfo productInfo)
        {

            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);

                    if (productInfo.ImageFile != null)
                    {
                        string FileDirectory = $"wwwroot/ProductImage";
                        if (!Directory.Exists(FileDirectory))
                        {
                            Directory.CreateDirectory(FileDirectory);
                        }
                        string UniqueName = Guid.NewGuid() + "_"+ productInfo.ImageFile.FileName;
                        string filepath = Path.Combine(Path.GetFullPath($"wwwroot/ProductImage"), UniqueName);

                        using (var filestream = new FileStream(filepath, FileMode.Create))
                        {
                            await productInfo.ImageFile.CopyToAsync(filestream);
                            productInfo.ImageUrl = $"ProductImage/" + UniqueName;
                        }
                    }

                    if (productInfo.Id == 0)
                    {
                        productInfo.IsActive = true;
                        productInfo.CreatedDate = DateTime.Now;
                        productInfo.CreatedBy = userId;
                        productInfo.StoreInfoId = user.StoreId;
                        await _productInfo.InsertAsync(productInfo);
                        TempData["success"] = "Data Added Successfully";
                    }
                    else
                    {
                        var OrgStoreInfo = await _productInfo.GetAsync(productInfo.Id);
                        OrgStoreInfo.ProductName = productInfo.ProductName;
                        OrgStoreInfo.ProductDescription = productInfo.ProductDescription;
                        OrgStoreInfo.IsActive = productInfo.IsActive;
                        OrgStoreInfo.ModifiedDate = DateTime.Now;
                        OrgStoreInfo.UnitInfoId = productInfo.UnitInfoId;
                        OrgStoreInfo.CategoryInfoId = productInfo.CategoryInfoId;
                        OrgStoreInfo.ModifiedBy = "";
                        OrgStoreInfo.StoreInfoId = user.StoreId;

                        if(productInfo.ImageUrl !=null)
                        {
                            OrgStoreInfo.ImageUrl = productInfo.ImageUrl;
                        }

                        
                        await _productInfo.UpdateAsync(OrgStoreInfo);
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
            _categoryInfo.Delete(categoryInfo);
            TempData["error"] = "Data Deleted Successfully";
            return RedirectToAction(nameof(Index));

        }
    }
}
    
