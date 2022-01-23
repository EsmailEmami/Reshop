using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Category;
using Reshop.Application.Security;
using Reshop.Domain.DTOs.Category;
using Reshop.Domain.Entities.Category;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Reshop.Application.Constants;
using Reshop.Application.Security.Attribute;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    [Permission(PermissionsConstants.CategoriesPage)]
    public class CategoryManagerController : Controller
    {
        #region constructor

        private readonly ICategoryService _categoryService;

        public CategoryManagerController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View(_categoryService.GetCategories());
        }

        [HttpGet]
        [Permission(PermissionsConstants.AddCategory, PermissionsConstants.EditCategory)]
        public async Task<IActionResult> AddOrEditCategory(int categoryId = 0)
        {
            if (categoryId == 0)
            {
                return View(new AddOrEditCategoryViewModel());
            }
            else
            {
                var category = await _categoryService.GetCategoryDataAsync(categoryId);

                if (category is null)
                    return NotFound();


                return View(category);
            }
        }

        [HttpPost]
        [Permission(PermissionsConstants.AddCategory, PermissionsConstants.EditCategory)]
        public async Task<IActionResult> AddOrEditCategory(AddOrEditCategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // in this section we check that all images are ok
            #region images security

            if (model.SelectedImage1 != null && !model.SelectedImage1.IsImage())
            {
                ModelState.AddModelError("SelectedImage1", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
                return View(model);
            }
            else if (model.SelectedImage2 != null && !model.SelectedImage2.IsImage())
            {
                ModelState.AddModelError("SelectedImage2", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
                return View(model);
            }
            else if (model.SelectedImage3 != null && !model.SelectedImage3.IsImage())
            {
                ModelState.AddModelError("SelectedImage3", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
                return View(model);
            }
            else if (model.SelectedImage4 != null && !model.SelectedImage4.IsImage())
            {
                ModelState.AddModelError("SelectedImage4", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
                return View(model);
            }
            else if (model.SelectedImage5 != null && !model.SelectedImage5.IsImage())
            {
                ModelState.AddModelError("SelectedImage5", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
                return View(model);
            }
            else if (model.SelectedImage6 != null && !model.SelectedImage6.IsImage())
            {
                ModelState.AddModelError("SelectedImage6", "ادمین عزیز لطفا تصاویر را درست انتخاب کنید.");
                return View(model);
            }

            #endregion

            var images = new List<IFormFile>()
            {
                model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                model.SelectedImage5, model.SelectedImage6
            };

            var urls = new List<string>()
            {
                model.SelectedImage1URL, model.SelectedImage2URL, model.SelectedImage3URL, model.SelectedImage4URL,
                model.SelectedImage5URL, model.SelectedImage6URL
            };


            if (model.CategoryId == 0)
            {
                // images could not be null
                if (model.SelectedImage1 == null || model.SelectedImage2 == null ||
                    model.SelectedImage3 == null || model.SelectedImage4 == null ||
                    model.SelectedImage5 == null || model.SelectedImage6 == null)
                {
                    ModelState.AddModelError("", "ادمین عزیز لطفا تمام عکس ها را وارد کنید.");
                    return View(model);
                }

                var category = new Category()
                {
                    CategoryTitle = model.CategoryTitle,
                    IsActive = model.IsActive
                };

                var res = await _categoryService.AddCategoryAsync(category);

                if (res == ResultTypes.Successful)
                {
                    // add images
                    await AddImg(images, urls, category.CategoryId);

                }
                else
                {
                    ModelState.AddModelError("", "متاسفاه هنگام افزودن گروه به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");
                    return View(model);
                }
            }
            else
            {
                var category = await _categoryService.GetCategoryByIdAsync(model.CategoryId);

                if (category == null)
                {
                    ModelState.AddModelError("", "متاسفاه هنگام ویرایش گروه به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");
                    return View(model);
                }


                // update category
                category.CategoryTitle = model.CategoryTitle;
                category.IsActive = model.IsActive;

                var res = await _categoryService.EditCategoryAsync(category);

                if (res != ResultTypes.Successful)
                {
                    ModelState.AddModelError("", "متاسفاه هنگام ویرایش گروه به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");
                    return View(model);
                }

                var imagesName = new List<string>()
                {
                    model.SelectedImage1IMG, model.SelectedImage2IMG, model.SelectedImage3IMG, model.SelectedImage4IMG,
                    model.SelectedImage5IMG, model.SelectedImage6IMG
                };

                // edit product images
                await EditImg(category.CategoryId, images, urls, imagesName);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Permission(PermissionsConstants.DeleteCategory)]
        public async Task<IActionResult> RemoveCategory(int categoryId)
        {
            if (!await _categoryService.IsCategoryExistAsync(categoryId)) return NotFound();

            await _categoryService.RemoveCategoryAsync(categoryId);

            return RedirectToAction("Index");
        }

        private async Task AddImg(List<IFormFile> images, List<string> urls, int categoryId)
        {
            for (int i = 0; i < images.Count; i++)
            {
                if (images[i].Length > 0)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images",
                        "categoryImages");

                    string imgName = await ImageConvertor.CreateNewImage(images[i], path, 250);

                    var categoryGallery = new CategoryGallery()
                    {
                        CategoryId = categoryId,
                        ImageName = imgName,
                        ImageUrl = urls[i],
                        OrderBy = i + 1,
                    };

                    await _categoryService.AddCategoryGalleryAsync(categoryGallery);
                }
            }
        }

        // توجه داشته باشید عکس های قبلی و جدید به ترتیب داده شده باشند
        private async Task EditImg(int categoryId, List<IFormFile> images, List<string> urls, List<string> imagesName)
        {
            for (int i = 0; i < images.Count; i++)
            {
                var imageInDatabase = await _categoryService.GetCategoryGalleryAsync(categoryId, imagesName[i]);


                if (images[i] != null && images.Count > 0)
                {
                    if (imageInDatabase != null)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot",
                            "images",
                            "categoryImages");

                        // delete last image
                        ImageConvertor.DeleteImage(filePath + "/" + imagesName[i]);

                        await _categoryService.DeleteCategoryGalleryAsync(imageInDatabase);
                        // create new image

                        var newImageName = await ImageConvertor.CreateNewImage(images[i], filePath, 250);

                        var newCategoryGallery = new CategoryGallery()
                        {
                            CategoryId = categoryId,
                            ImageName = newImageName,
                            ImageUrl = urls[i],
                            OrderBy = i + 1,
                        };

                        await _categoryService.AddCategoryGalleryAsync(newCategoryGallery);
                    }
                }
                else
                {
                    imageInDatabase.ImageUrl = urls[i];

                    await _categoryService.EditCategoryGalleryAsync(imageInDatabase);
                }
            }
        }
    }
}
