using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Constants;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Category;
using Reshop.Application.Security;
using Reshop.Application.Security.Attribute;
using Reshop.Domain.DTOs.Category;
using Reshop.Domain.Entities.Category;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Web.Areas.ManagerPanel.Controllers;

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
    [Permission(PermissionsConstants.AddCategory)]
    public IActionResult AddCategory()
    {
        return View(new AddCategoryViewModel());
    }

    [HttpPost]
    [Permission(PermissionsConstants.AddCategory)]
    public async Task<IActionResult> AddCategory(AddCategoryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // in this section we check that all images are ok
        #region images security


        foreach (var image in model.Images)
        {
            if (!image.IsImage())
            {
                ModelState.AddModelError("", "لطفا تصاویر خود را به درستی انتخاب کنید");
                return View(model);
            }
        }

        #endregion

        // images could not be null
        if (model.Images.Count <= 2)
        {
            ModelState.AddModelError("", "تصاویر کالا باید حداقل 3 تصویر باشد.");
            return View(model);
        }

        // check urls of images
        if (model.Images.Count != (model.Urls.Count - 1))
        {
            ModelState.AddModelError("", "لطفا تصاویر خود را به درستی وارد کنید");
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
            await AddImg(model.Images, model.Urls, category.CategoryId);

        }
        else
        {
            ModelState.AddModelError("", "متاسفاه هنگام افزودن گروه به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");
            return View(model);
        }


        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Permission(PermissionsConstants.EditCategory)]
    public async Task<IActionResult> EditCategory(int categoryId)
    {
        var category = await _categoryService.GetCategoryDataAsync(categoryId);

        if (category is null)
            return NotFound();

        return View(category);
    }

    [HttpPost]
    [Permission(PermissionsConstants.EditCategory)]
    public async Task<IActionResult> EditCategory(EditCategoryViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // in this section we check that all images are ok
        #region images security

        #endregion

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


        // edit product images
        await EditImg(category.CategoryId, model.Images,
            model.SelectedImages as List<string>,
            model.ChangedImages as List<string>,
            model.DeletedImages as List<string>,
            model.Urls as List<string>,
            model.ChangedUrls as List<string>);



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

    private async Task EditImg(int categoryId, List<IFormFile> newImages, List<string> imagesName,
       List<string> editedImages, List<string> deletedImages, List<string> urls, List<string> editedUrls)
    {
        if (newImages == null && deletedImages == null && editedUrls == null) return;

        imagesName ??= new List<string>();

        if (editedUrls != null)
        {
            editedUrls = editedUrls.Distinct().ToList();
        }

        if (editedImages != null)
        {
            editedImages = editedImages.Distinct().ToList();
            editedImages = editedImages.ToListInt()
                .Where(c => c <= imagesName.Count)
                .ToList()
                .ToListString();

        }

        // update edited elements
        if (editedImages != null && deletedImages != null)
        {
            editedImages = editedImages.Distinct().ToList();


            foreach (var deletedImage in deletedImages)
            {
                if (editedImages.Any(c => c == deletedImage))
                {
                    editedImages.Remove(deletedImage);
                }
            }
        }

        if (editedUrls != null && deletedImages != null)
        {
            foreach (var deletedImage in deletedImages)
            {
                if (editedUrls.Any(c => c == deletedImage))
                {
                    editedUrls.Remove(deletedImage);
                }
            }
        }

        // delete selected images 
        if (deletedImages != null && deletedImages.Count > 0)
        {
            foreach (string deletedImage in deletedImages)
            {
                int num = int.Parse(deletedImage) - 1;

                var imageName = imagesName[num];

                var imageInDatabase = await _categoryService.GetCategoryGalleryAsync(categoryId, imageName);

                if (imageInDatabase != null)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images",
                        "categoryImages");

                    // delete last image
                    ImageConvertor.DeleteImage(filePath + "/" + imageName);
                    await _categoryService.DeleteCategoryGalleryAsync(imageInDatabase);
                }
            }
        }

        // edit selected Images
        if (editedImages != null && editedImages.Count > 0)
        {

            int editImageCounter = 0;

            foreach (string editedImage in editedImages)
            {
                int num = int.Parse(editedImage) - 1;

                var imageName = imagesName[num];
                var url = urls[num];
                IFormFile imageFile = newImages[editImageCounter];

                editImageCounter++;


                if (imageFile != null && imageFile.Length > 0)
                {
                    var imageInDatabase = await _categoryService.GetCategoryGalleryAsync(categoryId, imageName);

                    if (imageInDatabase != null)
                    {
                        int imageOrderBy = imageInDatabase.OrderBy;


                        string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot",
                            "images",
                            "categoryImages");

                        // delete last image
                        ImageConvertor.DeleteImage(filePath + "/" + imageName);

                        await _categoryService.DeleteCategoryGalleryAsync(imageInDatabase);
                        // create new image

                        var newImageName = await ImageConvertor.CreateNewImage(imageFile, filePath, 1600);

                        var newProductGallery = new CategoryGallery()
                        {
                            CategoryId = categoryId,
                            ImageName = newImageName,
                            OrderBy = imageOrderBy,
                            ImageUrl = url
                        };

                        await _categoryService.AddCategoryGalleryAsync(newProductGallery);
                    }

                }


                if (editedUrls != null)
                {
                    if (editedUrls.Any(c => c == editedImage))
                    {
                        editedUrls.Remove(editedImage);
                    }
                }
            }
        }

        // skip edited images and add new images
        var newUrls = urls.Skip(imagesName.Count).ToList();
        if (editedImages != null)
        {
            newImages = newImages.Skip(editedImages.Count).ToList();
        }

        int newImagesOrderByCounter = imagesName.Count + 1;

        int urlCounter = 0;
        if (newImages != null)
        {
            foreach (var image in newImages)
            {
                string url = newUrls[urlCounter];

                urlCounter++;

                if (image.Length > 0)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images",
                        "categoryImages");

                    string imgName = await ImageConvertor.CreateNewImage(image, path, 1600);

                    var productGallery = new CategoryGallery()
                    {
                        CategoryId = categoryId,
                        ImageName = imgName,
                        OrderBy = newImagesOrderByCounter,
                        ImageUrl = url
                    };

                    await _categoryService.AddCategoryGalleryAsync(productGallery);

                    newImagesOrderByCounter++;
                }
            }
        }


        // edit urls that have been edited
        if (editedUrls != null && editedUrls.Any())
        {
            foreach (var editedUrl in editedUrls)
            {
                int num = int.Parse(editedUrl) - 1;

                var imageName = imagesName[num];
                var url = urls[num];


                var imageInDatabase = await _categoryService.GetCategoryGalleryAsync(categoryId, imageName);

                if (imageInDatabase != null)
                {
                    imageInDatabase.ImageUrl = url;

                    await _categoryService.EditCategoryGalleryAsync(imageInDatabase);
                }
            }
        }
    }
}
