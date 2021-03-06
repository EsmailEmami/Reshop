using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Constants;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Enums.Product;
using Reshop.Application.Interfaces.Discount;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security;
using Reshop.Application.Security.Attribute;
using Reshop.Domain.DTOs.Discount;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Product.ProductDetail;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reshop.Web.Areas.ManagerPanel.Controllers;

[Area("ManagerPanel")]
[Permission(PermissionsConstants.ProductsPage)]
public class ProductManagerController : Controller
{
    #region constructor

    private readonly IProductService _productService;
    private readonly IShopperService _shopperService;
    private readonly IBrandService _brandService;
    private readonly IColorService _colorService;
    private readonly IDiscountService _discountService;
    private readonly IPermissionService _permissionService;

    public ProductManagerController(IProductService productService, IShopperService shopperService,
        IBrandService brandService, IColorService colorService, IDiscountService discountService,
        IPermissionService permissionService)
    {
        _productService = productService;
        _shopperService = shopperService;
        _brandService = brandService;
        _colorService = colorService;
        _discountService = discountService;
        _permissionService = permissionService;
    }

    #endregion

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await _productService.GetProductsGeneralDataForAdminAsync();
        return View(model);
    }

    [HttpGet]
    [NoDirectAccess]
    public IActionResult ProductsList(string type, int pageId, string filter)
    {
        return ViewComponent("ProductsListForAdminComponent", new { type, pageId, filter });
    }

    [HttpGet]
    [Permission(PermissionsConstants.ProductDetail)]
    public async Task<IActionResult> ProductDetail(int productId)
    {
        if (productId == 0)
            return NotFound();

        var product = await _productService.GetProductDetailForAdminAsync(productId);

        if (product == null)
            return NotFound();


        return View(product);
    }

    [HttpGet]
    [NoDirectAccess]
    public async Task<IActionResult> ProductColorDetail(int productId, int colorId)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return NotFound();

        var permissionValid =
            await _permissionService.PermissionCheckerAsync(userId, PermissionsConstants.ColorDetailOfProduct);
        ViewBag.IsValid = true;

        if (!permissionValid)
        {
            ViewBag.IsValid = false;

            return View(new ProductColorDetailViewModel()
            {
                ColorId = colorId,
                ProductId = productId
            });
        }

        var data = await _colorService.GetProductColorDetailAsync(productId, colorId);

        if (data == null)
            return NotFound();

        return View(data);
    }

    [HttpGet]
    [NoDirectAccess]
    public async Task<IActionResult> ProductColorDiscountsDetail(int productId, int colorId)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return NotFound();

        var permissionValid = await _permissionService.PermissionCheckerAsync(userId, PermissionsConstants.DiscountDetailOfProduct);
        ViewBag.IsValid = true;

        if (!permissionValid)
        {
            ViewBag.IsValid = false;

            return View(new DiscountsGeneralDataViewModel()
            {
                ColorId = colorId,
                ProductId = productId
            });
        }

        var data = await _discountService.GetProductColorDiscountsGeneralDataAsync(productId, colorId);

        if (data == null)
            return NotFound();

        return View(data);
    }

    [HttpGet]
    [NoDirectAccess]
    public IActionResult ShoppersOfProduct(int productId, string type, int pageId, string filter)
    {
        if (productId == 0)
            return NotFound();


        if (filter.ToLower() == "undefined")
        {
            filter = "";
        }

        if (type.ToLower() == "undefined")
        {
            type = "all";
        }


        return ViewComponent("ShoppersListOfProductComponent", new { type, pageId, filter });
    }


    #region ProductType

    [HttpGet]
    public async Task<IActionResult> EditProduct(int productId)
    {
        var productType = await _productService.GetProductTypeByIdAsync(productId);

        return productType switch
        {
            ProductTypes.Mobile => RedirectToAction("EditMobile", "ProductManager",
                new { productId }),

            ProductTypes.Laptop => RedirectToAction("AddOrEditLaptop", "ProductManager",
                new { productId }),

            ProductTypes.Tablet => RedirectToAction("AddOrEditTablet", "ProductManager",
                new { productId }),

            ProductTypes.MobileCover => RedirectToAction("AddOrEditMobileCover", "ProductManager",
                new { productId }),

            ProductTypes.LaptopCover => RedirectToAction("AddOrEditLaptopCover", "ProductManager",
                new { productId }),

            ProductTypes.Speaker => RedirectToAction("AddOrEditSpeaker", "ProductManager",
                new { productId }),

            ProductTypes.PowerBank => RedirectToAction("AddOrEditPowerBank", "ProductManager",
                new { productId }),

            ProductTypes.WristWatch => RedirectToAction("AddOrEditWristWatch", "ProductManager",
                new { productId }),

            ProductTypes.SmartWatch => RedirectToAction("AddOrEditSmartWatch", "ProductManager",
                new { productId }),

            ProductTypes.FlashMemory => RedirectToAction("AddOrEditFlashMemory", "ProductManager",
                new { productId }),

            ProductTypes.AUX => RedirectToAction("EditAux", "ProductManager",
                new { productId }),

            ProductTypes.NotFound => NotFound(),

            _ => RedirectToAction("Index")
        };
    }

    #endregion

    #region mobile

    [HttpGet]
    public IActionResult AddMobile()
    {
        var newModel = new AddMobileProductViewModel()
        {
            StoreTitles = _shopperService.GetStoreTitles(),
            Chipsets = _productService.GetChipsets(),
            CpuArches = _productService.GetCpuArches(),
            OperatingSystems = _productService.GetOperatingSystems()
        };

        return View(newModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddMobile(AddMobileProductViewModel model)
    {
        // data for select Product
        model.StoreTitles = _shopperService.GetStoreTitles();
        model.Brands = _brandService.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
        model.OfficialProducts = _brandService.GetBrandOfficialProducts(model.SelectedBrand);
        model.ChildCategories = _brandService.GetChildCategoriesOfBrand(model.SelectedBrand);

        // data for mobile detail
        model.Chipsets = _productService.GetChipsets();
        model.CpuArches = _productService.GetCpuArches();
        model.OperatingSystems = _productService.GetOperatingSystems();
        model.Cpus = _productService.GetCpusOfChipset(model.SelectedChipset);
        model.Gpus = _productService.GetGpusOfChipset(model.SelectedChipset);
        model.OperatingSystemVersions = _productService.GetOperatingSystemVersionsOfOperatingSystem(model.SelectedOS);


        if (!ModelState.IsValid) return View(model);

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

        if (model.Images == null)
        {
            ModelState.AddModelError("", "لطفا تصاویر کالا را انتخاب کنید.");
            return View(model);
        }
        else if (model.Images.Count <= 2)
        {
            ModelState.AddModelError("", "تصاویر کالا باید حداقل 3 تصویر باشد.");
            return View(model);
        }


        var product = new Product()
        {
            ProductTitle = model.ProductTitle,
            Description = model.Description,
            ProductType = ProductTypes.Mobile.ToString(),
            OfficialBrandProductId = model.OfficialBrandProductId,
            IsActive = model.IsActive,
            ChildCategoryId = model.SelectedChildCategory
        };

        var mobileDetail = new MobileDetail()
        {
            Length = model.Length,
            Width = model.Width,
            Height = model.Height,
            Weight = model.Weight,
            SimCardQuantity = model.SimCardQuantity,
            SimCardInput = model.SimCardInput,
            SeparateSlotMemoryCard = model.SeparateSlotMemoryCard,
            Announced = model.Announced.ConvertPersianDateToEnglishDate(),
            ChipsetId = model.SelectedChipset,
            CpuId = model.SelectedCpu,
            CpuAndFrequency = model.CpuAndFrequency,
            CpuArchId = model.SelectedCpuArch,
            GpuId = model.SelectedGpu,
            InternalStorage = model.InternalStorage,
            Ram = model.Ram,
            SdCard = model.SdCard,
            SdCardStandard = model.SdCardStandard,
            ColorDisplay = model.ColorDisplay,
            TouchDisplay = model.TouchDisplay,
            DisplayTechnology = model.DisplayTechnology,
            DisplaySize = model.DisplaySize,
            Resolution = model.Resolution,
            PixelDensity = model.PixelDensity,
            ScreenToBodyRatio = model.ScreenToBodyRatio,
            ImageRatio = model.ImageRatio,
            DisplayProtection = model.DisplayProtection,
            DisplayMoreInformation = model.DisplayMoreInformation,
            ConnectionsNetwork = model.ConnectionsNetwork.ListToString("&"),
            GsmNetwork = model.GsmNetwork.ListToString("&"),
            HspaNetwork = model.HspaNetwork.ListToString("&"),
            LteNetwork = model.LteNetwork.ListToString("&"),
            FiveGNetwork = model.FiveGNetwork.ListToString("&"),
            CommunicationTechnology = model.CommunicationTechnology,
            WiFi = model.WiFi.ListToString("&"),
            Radio = model.Radio,
            Bluetooth = model.Bluetooth.ListToString("&"),
            GpsInformation = model.GpsInformation.ListToString("&"),
            ConnectionPort = model.ConnectionPort,
            ConnectionsMoreInformation = model.ConnectionsMoreInformation,
            Cameras = model.Cameras.ListToString("&"),
            PhotoResolution = model.PhotoResolution,
            SelfiCameraResolution = model.SelfiCameraResolution,
            MacroCameraResolution = model.MacroCameraResolution,
            WideCameraResolution = model.WideCameraResolution,
            DepthCameraResolution = model.DepthCameraResolution,
            CameraCapabilities = model.CameraCapabilities,
            SelfiCameraCapabilities = model.SelfiCameraCapabilities,
            MacroCameraCapabilities = model.MacroCameraCapabilities,
            WideCameraCapabilities = model.WideCameraCapabilities,
            DepthCameraCapabilities = model.DepthCameraCapabilities,
            PhotoCameraVideo = model.PhotoCameraVideo.ListToString("&"),
            SelfiCameraVideo = model.SelfiCameraVideo.ListToString("&"),
            MacroCameraVideo = model.MacroCameraVideo.ListToString("&"),
            WideCameraVideo = model.WideCameraVideo.ListToString("&"),
            DepthCameraVideo = model.DepthCameraVideo.ListToString("&"),
            CameraMoreInformation = model.CameraMoreInformation,
            Speakers = model.Speakers,
            OutputAudio = model.OutputAudio,
            AudioMoreInformation = model.AudioMoreInformation,
            OperatingSystemId = model.SelectedOS,
            OperatingSystemVersionId = model.SelectedOsVersion,
            UiVersion = model.UiVersion,
            SoftWareMoreInformation = model.SoftWareMoreInformation,
            BatteryMaterial = model.BatteryMaterial,
            BatteryCapacity = model.BatteryCapacity,
            Removable‌Battery = model.Removable‌Battery,
            Sensors = model.Sensors,
            ItemsInBox = model.ItemsInBox
        };

        var result = await _productService.AddMobileAsync(product, mobileDetail);

        if (result == ResultTypes.Successful)
        {
            // add product images
            await AddImg(model.Images, product.ProductId);


            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("",
            $"ادمین عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditMobile(int productId)
    {
        var data = await _productService.GetTypeMobileProductDataAsync(productId);

        if (data == null)
            return NotFound();


        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> EditMobile(EditMobileProductViewModel model)
    {
        // data for select Product
        model.StoreTitles = _shopperService.GetStoreTitles();
        model.Brands = _brandService.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
        model.OfficialProducts = _brandService.GetBrandOfficialProducts(model.SelectedBrand);
        model.ChildCategories = _brandService.GetChildCategoriesOfBrand(model.SelectedBrand);

        // data for mobile detail
        model.Chipsets = _productService.GetChipsets();
        model.CpuArches = _productService.GetCpuArches();
        model.OperatingSystems = _productService.GetOperatingSystems();
        model.Cpus = _productService.GetCpusOfChipset(model.SelectedChipset);
        model.Gpus = _productService.GetGpusOfChipset(model.SelectedChipset);
        model.OperatingSystemVersions = _productService.GetOperatingSystemVersionsOfOperatingSystem(model.SelectedOS);

        if (!ModelState.IsValid) return View(model);

        // in this section we check that all images are ok

        #region images security

        if (model.Images != null)
        {
            foreach (var image in model.Images)
            {
                if (!image.IsImage())
                {
                    ModelState.AddModelError("", "لطفا تصاویر خود را به درستی انتخاب کنید");
                    return View(model);
                }
            }
        }

        #endregion

        var product = await _productService.GetProductByIdAsync(model.ProductId);

        if (product?.MobileDetailId == null)
            return NotFound();


        var mobileDetail = await _productService.GetMobileDetailByIdAsync(product.MobileDetailId.Value);

        if (mobileDetail == null)
            return NotFound();




        //  update product
        product.ProductTitle = model.ProductTitle;
        product.Description = model.Description;
        product.OfficialBrandProductId = model.OfficialBrandProductId;
        product.IsActive = model.IsActive;
        product.ChildCategoryId = model.SelectedChildCategory;


        // update mobile detail
        mobileDetail.Length = model.Length;
        mobileDetail.Width = model.Width;
        mobileDetail.Height = model.Height;
        mobileDetail.Weight = model.Weight;
        mobileDetail.SimCardQuantity = model.SimCardQuantity;
        mobileDetail.SimCardInput = model.SimCardInput;
        mobileDetail.SeparateSlotMemoryCard = model.SeparateSlotMemoryCard;
        mobileDetail.Announced = model.Announced.ConvertPersianDateToEnglishDate();
        mobileDetail.ChipsetId = model.SelectedChipset;
        mobileDetail.CpuId = model.SelectedCpu;
        mobileDetail.CpuAndFrequency = model.CpuAndFrequency;
        mobileDetail.CpuArchId = model.SelectedCpuArch;
        mobileDetail.GpuId = model.SelectedGpu;
        mobileDetail.InternalStorage = model.InternalStorage;
        mobileDetail.Ram = model.Ram;
        mobileDetail.SdCard = model.SdCard;
        mobileDetail.SdCardStandard = model.SdCardStandard;
        mobileDetail.ColorDisplay = model.ColorDisplay;
        mobileDetail.TouchDisplay = model.TouchDisplay;
        mobileDetail.DisplayTechnology = model.DisplayTechnology;
        mobileDetail.DisplaySize = model.DisplaySize;
        mobileDetail.Resolution = model.Resolution;
        mobileDetail.PixelDensity = model.PixelDensity;
        mobileDetail.ScreenToBodyRatio = model.ScreenToBodyRatio;
        mobileDetail.ImageRatio = model.ImageRatio;
        mobileDetail.DisplayProtection = model.DisplayProtection;
        mobileDetail.DisplayMoreInformation = model.DisplayMoreInformation;
        mobileDetail.ConnectionsNetwork = model.ConnectionsNetwork.ListToString("&");
        mobileDetail.GsmNetwork = model.GsmNetwork.ListToString("&");
        mobileDetail.HspaNetwork = model.HspaNetwork.ListToString("&");
        mobileDetail.LteNetwork = model.LteNetwork.ListToString("&");
        mobileDetail.FiveGNetwork = model.FiveGNetwork.ListToString("&");
        mobileDetail.CommunicationTechnology = model.CommunicationTechnology;
        mobileDetail.WiFi = model.WiFi.ListToString("&");
        mobileDetail.Radio = model.Radio;
        mobileDetail.Bluetooth = model.Bluetooth.ListToString("&");
        mobileDetail.GpsInformation = model.GpsInformation.ListToString("&");
        mobileDetail.ConnectionPort = model.ConnectionPort;
        mobileDetail.ConnectionsMoreInformation = model.ConnectionsMoreInformation;
        mobileDetail.Cameras = model.Cameras.ListToString("&");
        mobileDetail.PhotoResolution = model.PhotoResolution;
        mobileDetail.SelfiCameraResolution = model.SelfiCameraResolution;
        mobileDetail.MacroCameraResolution = model.MacroCameraResolution;
        mobileDetail.WideCameraResolution = model.WideCameraResolution;
        mobileDetail.DepthCameraResolution = model.DepthCameraResolution;
        mobileDetail.CameraCapabilities = model.CameraCapabilities;
        mobileDetail.SelfiCameraCapabilities = model.SelfiCameraCapabilities;
        mobileDetail.MacroCameraCapabilities = model.MacroCameraCapabilities;
        mobileDetail.WideCameraCapabilities = model.WideCameraCapabilities;
        mobileDetail.DepthCameraCapabilities = model.DepthCameraCapabilities;
        mobileDetail.PhotoCameraVideo = model.PhotoCameraVideo.ListToString("&");
        mobileDetail.SelfiCameraVideo = model.SelfiCameraVideo.ListToString("&");
        mobileDetail.MacroCameraVideo = model.MacroCameraVideo.ListToString("&");
        mobileDetail.WideCameraVideo = model.WideCameraVideo.ListToString("&");
        mobileDetail.DepthCameraVideo = model.DepthCameraVideo.ListToString("&");
        mobileDetail.CameraMoreInformation = model.CameraMoreInformation;
        mobileDetail.Speakers = model.Speakers;
        mobileDetail.OutputAudio = model.OutputAudio;
        mobileDetail.AudioMoreInformation = model.AudioMoreInformation;
        mobileDetail.OperatingSystemId = model.SelectedOS;
        mobileDetail.OperatingSystemVersionId = model.SelectedOsVersion;
        mobileDetail.UiVersion = model.UiVersion;
        mobileDetail.SoftWareMoreInformation = model.SoftWareMoreInformation;
        mobileDetail.BatteryMaterial = model.BatteryMaterial;
        mobileDetail.BatteryCapacity = model.BatteryCapacity;
        mobileDetail.Removable‌Battery = model.Removable‌Battery;
        mobileDetail.Sensors = model.Sensors;
        mobileDetail.ItemsInBox = model.ItemsInBox;



        var result = await _productService.EditMobileAsync(product, mobileDetail);

        if (result == ResultTypes.Successful)
        {
            // edit product images
            await EditImg(product.ProductId, model.Images, model.SelectedImages as List<string>,
                model.ChangedImages as List<string>, model.DeletedImages as List<string>);

            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("",
            $"ادمین عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

        return View(model);
    }

    #endregion

    #region laptop

    [HttpGet]
    public async Task<IActionResult> AddOrEditLaptop(int productId = 0)
    {
        if (productId == 0)
        {
            var newModel = new AddOrEditLaptopProductViewModel()
            {
                StoreTitles = _shopperService.GetStoreTitles()
            };

            return View(newModel);
        }
        else
        {
            var product = await _productService.GetTypeLaptopProductDataAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddOrEditLaptop(AddOrEditLaptopProductViewModel model)
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
            ModelState.AddModelError("SelectedImage6", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
            return View(model);
        }

        #endregion




        if (model.ProductId == 0)
        {
            // images could not be null
            if (model.SelectedImage1 == null || model.SelectedImage2 == null ||
                model.SelectedImage3 == null || model.SelectedImage4 == null ||
                model.SelectedImage5 == null || model.SelectedImage6 == null)
            {
                ModelState.AddModelError("", "ادمین عزیز لطفا تمام عکس ها را وارد کنید.");
                return View(model);
            }



            var product = new Product()
            {
                ProductTitle = model.ProductTitle,
                Description = model.Description,
                ProductType = ProductTypes.Mobile.ToString(),
                OfficialBrandProductId = model.OfficialBrandProductId,
                IsActive = model.IsActive
            };


            var laptopDetail = new LaptopDetail()
            {
                Length = model.Length,
                Width = model.Width,
                Height = model.Height,
                Weight = model.Weight,
                CpuCompany = model.CpuCompany,
                CpuSeries = model.CpuSeries,
                CpuModel = model.CpuModel,
                CpuFerequancy = model.CpuFerequancy,
                CpuCache = model.CpuCache,
                RamStorage = model.RamStorage,
                RamStorageTeachnology = model.RamStorageTeachnology,
                Storage = model.Storage,
                StorageTeachnology = model.StorageTeachnology,
                StorageInformation = model.StorageInformation,
                GpuCompany = model.GpuCompany,
                GpuModel = model.GpuModel,
                GpuRam = model.GpuRam,
                DisplaySize = model.DisplaySize,
                DisplayTeachnology = model.DisplayTeachnology,
                DisplayResolutation = model.DisplayResolutation,
                RefreshDisplay = model.RefreshDisplay,
                BlurDisplay = model.BlurDisplay,
                TouchDisplay = model.TouchDisplay,
                DiskDrive = model.DiskDrive,
                FingerTouch = model.FingerTouch,
                Webcam = model.Webcam,
                BacklightKey = model.BacklightKey,
                TouchPadInformation = model.TouchPadInformation,
                ModemInformation = model.ModemInformation,
                Wifi = model.Wifi,
                Bluetooth = model.Bluetooth,
                VgaPort = model.VgaPort,
                HtmiPort = model.HtmiPort,
                DisplayPort = model.DisplayPort,
                LanPort = model.LanPort,
                UsbCPort = model.UsbCPort,
                Usb3Port = model.Usb3Port,
                UsbCQuantity = model.UsbCQuantity,
                UsbQuantity = model.UsbQuantity,
                Usb3Quantity = model.Usb3Quantity,
                BatteryMaterial = model.BatteryMaterial,
                BatteryCharging = model.BatteryCharging,
                BatteryInformation = model.BatteryInformation,
                Os = model.Os,
                Classification = model.Classification,
            };

            var result = await _productService.AddLaptopAsync(product, laptopDetail);

            if (result == ResultTypes.Successful)
            {
                // add product images
                await AddImg(new List<IFormFile>()
                {
                    model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                    model.SelectedImage5, model.SelectedImage6
                }, product.ProductId);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
        else
        {
            var product = await _productService.GetProductByIdAsync(model.ProductId);

            if (product?.LaptopDetailId == null)
                return NotFound();


            var laptopDetail = await _productService.GetLaptopDetailByIdAsync(product.LaptopDetailId.Value);

            if (laptopDetail == null)
                return NotFound();



            //  update product
            product.ProductTitle = model.ProductTitle;
            product.Description = model.Description;
            product.OfficialBrandProductId = model.OfficialBrandProductId;
            product.IsActive = model.IsActive;



            // update laptop detail
            laptopDetail.Length = model.Length;
            laptopDetail.Width = model.Width;
            laptopDetail.Height = model.Height;
            laptopDetail.Weight = model.Weight;
            laptopDetail.CpuCompany = model.CpuCompany;
            laptopDetail.CpuSeries = model.CpuSeries;
            laptopDetail.CpuModel = model.CpuModel;
            laptopDetail.CpuFerequancy = model.CpuFerequancy;
            laptopDetail.CpuCache = model.CpuCache;
            laptopDetail.RamStorage = model.RamStorage;
            laptopDetail.RamStorageTeachnology = model.RamStorageTeachnology;
            laptopDetail.Storage = model.Storage;
            laptopDetail.StorageTeachnology = model.StorageTeachnology;
            laptopDetail.StorageInformation = model.StorageInformation;
            laptopDetail.GpuCompany = model.GpuCompany;
            laptopDetail.GpuModel = model.GpuModel;
            laptopDetail.GpuRam = model.GpuRam;
            laptopDetail.DisplaySize = model.DisplaySize;
            laptopDetail.DisplayTeachnology = model.DisplayTeachnology;
            laptopDetail.DisplayResolutation = model.DisplayResolutation;
            laptopDetail.RefreshDisplay = model.RefreshDisplay;
            laptopDetail.BlurDisplay = model.BlurDisplay;
            laptopDetail.TouchDisplay = model.TouchDisplay;
            laptopDetail.DiskDrive = model.DiskDrive;
            laptopDetail.FingerTouch = model.FingerTouch;
            laptopDetail.Webcam = model.Webcam;
            laptopDetail.BacklightKey = model.BacklightKey;
            laptopDetail.TouchPadInformation = model.TouchPadInformation;
            laptopDetail.ModemInformation = model.ModemInformation;
            laptopDetail.Wifi = model.Wifi;
            laptopDetail.Bluetooth = model.Bluetooth;
            laptopDetail.VgaPort = model.VgaPort;
            laptopDetail.HtmiPort = model.HtmiPort;
            laptopDetail.DisplayPort = model.DisplayPort;
            laptopDetail.LanPort = model.LanPort;
            laptopDetail.UsbCPort = model.UsbCPort;
            laptopDetail.Usb3Port = model.Usb3Port;
            laptopDetail.UsbCQuantity = model.UsbCQuantity;
            laptopDetail.UsbQuantity = model.UsbQuantity;
            laptopDetail.Usb3Quantity = model.Usb3Quantity;
            laptopDetail.BatteryMaterial = model.BatteryMaterial;
            laptopDetail.BatteryCharging = model.BatteryCharging;
            laptopDetail.BatteryInformation = model.BatteryInformation;
            laptopDetail.Os = model.Os;
            laptopDetail.Classification = model.Classification;


            var result = await _productService.EditLaptopAsync(product, laptopDetail);

            if (result == ResultTypes.Successful)
            {
                // edit product images
                //await EditImg(product.ProductId, new List<IFormFile>()
                //    {
                //        model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                //        model.SelectedImage5, model.SelectedImage6
                //    }, new List<string>()
                //    {
                //        model.SelectedImage1IMG,model.SelectedImage2IMG, model.SelectedImage3IMG, model.SelectedImage4IMG,
                //        model.SelectedImage5IMG, model.SelectedImage6IMG
                //    });

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
    }

    #endregion

    #region tablet

    [HttpGet]
    public async Task<IActionResult> AddOrEditTablet(int productId)
    {
        if (productId == 0)
        {
            var newModel = new AddOrEditTabletViewModel()
            {
                StoreTitles = _shopperService.GetStoreTitles()
            };

            return View(newModel);
        }
        else
        {
            var product = await _productService.GetTypeTabletProductDataAsync(productId);
            if (product == null)
                return NotFound();


            return View(product);
        }
    }

    [HttpPost]

    public async Task<IActionResult> AddOrEditTablet(AddOrEditTabletViewModel model)
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
            ModelState.AddModelError("SelectedImage6", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
            return View(model);
        }

        #endregion




        if (model.ProductId == 0)
        {
            // images could not be null
            if (model.SelectedImage1 == null || model.SelectedImage2 == null ||
                model.SelectedImage3 == null || model.SelectedImage4 == null ||
                model.SelectedImage5 == null || model.SelectedImage6 == null)
            {
                ModelState.AddModelError("", "ادمین عزیز لطفا تمام عکس ها را وارد کنید.");
                return View(model);
            }

            var product = new Product()
            {
                ProductTitle = model.ProductTitle,
                Description = model.Description,

                ProductType = ProductTypes.Mobile.ToString(),
                OfficialBrandProductId = model.OfficialBrandProductId,
                IsActive = model.IsActive
            };

            var tabletDetail = new TabletDetail()
            {
                Lenght = model.Lenght,
                Width = model.Width,
                Height = model.Height,
                Weight = model.Weight,
                SimCardIsTrue = model.SimCardIsTrue,
                Call = model.Call,
                SimCardQuantity = model.SimCardQuantity,
                SimCardInpute = model.SimCardInpute,
                SeparateSlotMemoryCard = model.SeparateSlotMemoryCard,
                Announced = model.Announced,
                ChipsetName = model.ChipsetName,
                Cpu = model.Cpu,
                CpuAndFrequency = model.CpuAndFrequency,
                CpuArch = model.CpuArch,
                Gpu = model.Gpu,
                InternalStorage = model.InternalStorage,
                Ram = model.Ram,
                SdCard = model.SdCard,
                SdCardStandard = model.SdCardStandard,
                ColorDisplay = model.ColorDisplay,
                TouchDisplay = model.TouchDisplay,
                DisplayTechnology = model.DisplayTechnology,
                DisplaySize = model.DisplaySize,
                Resolution = model.Resolution,
                PixelDensity = model.PixelDensity,
                ScreenToBodyRatio = model.ScreenToBodyRatio,
                ImageRatio = model.ImageRatio,
                DisplayProtection = model.DisplayProtection,
                MoreInformation = model.MoreInformation,
                ConnectionsNetwork = model.ConnectionsNetwork,
                GsmNetwork = model.GsmNetwork,
                HspaNetwork = model.HspaNetwork,
                LteNetwork = model.LteNetwork,
                FiveGNetwork = model.FiveGNetwork,
                CommunicationTechnology = model.CommunicationTechnology,
                WiFi = model.WiFi,
                Radio = model.Radio,
                Bluetooth = model.Bluetooth,
                GpsInformation = model.GpsInformation,
                ConnectionPort = model.ConnectionPort,
                CameraQuantity = model.CameraQuantity,
                PhotoResolutation = model.PhotoResolutation,
                SelfiCameraPhoto = model.SelfiCameraPhoto,
                CameraCapabilities = model.CameraCapabilities,
                SelfiCameraCapabilities = model.SelfiCameraCapabilities,
                Filming = model.Filming,
                Speakers = model.Speakers,
                OutputAudio = model.OutputAudio,
                AudioInformation = model.AudioInformation,
                OS = model.OS,
                OsVersion = model.OsVersion,
                UiVersion = model.UiVersion,
                MoreInformationSoftWare = model.MoreInformationSoftWare,
                BatteryMaterial = model.BatteryMaterial,
                BatteryCapacity = model.BatteryCapacity,
                Removable‌Battery = model.Removable‌Battery,
                Sensors = model.Sensors,
                ItemsInBox = model.ItemsInBox,
            };


            var result = await _productService.AddTabletAsync(product, tabletDetail);

            if (result == ResultTypes.Successful)
            {
                // add product images
                await AddImg(new List<IFormFile>()
                {
                    model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                    model.SelectedImage5, model.SelectedImage6
                }, product.ProductId);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
        else
        {
            var product = await _productService.GetProductByIdAsync(model.ProductId);

            if (product?.MobileCoverDetailId == null)
                return NotFound();


            var tabletDetail = await _productService.GetTabletByIdAsync(product.MobileCoverDetailId.Value);

            if (tabletDetail == null)
                return NotFound();



            //  update product
            product.ProductTitle = model.ProductTitle;
            product.Description = model.Description;
            product.OfficialBrandProductId = model.OfficialBrandProductId;
            product.IsActive = model.IsActive;

            // update mobile cover detail
            tabletDetail.Lenght = model.Lenght;
            tabletDetail.Width = model.Width;
            tabletDetail.Height = model.Height;
            tabletDetail.Weight = model.Weight;
            tabletDetail.SimCardIsTrue = model.SimCardIsTrue;
            tabletDetail.Call = model.Call;
            tabletDetail.SimCardQuantity = model.SimCardQuantity;
            tabletDetail.SimCardInpute = model.SimCardInpute;
            tabletDetail.SeparateSlotMemoryCard = model.SeparateSlotMemoryCard;
            tabletDetail.Announced = model.Announced;
            tabletDetail.ChipsetName = model.ChipsetName;
            tabletDetail.Cpu = model.Cpu;
            tabletDetail.CpuAndFrequency = model.CpuAndFrequency;
            tabletDetail.CpuArch = model.CpuArch;
            tabletDetail.Gpu = model.Gpu;
            tabletDetail.InternalStorage = model.InternalStorage;
            tabletDetail.Ram = model.Ram;
            tabletDetail.SdCard = model.SdCard;
            tabletDetail.SdCardStandard = model.SdCardStandard;
            tabletDetail.ColorDisplay = model.ColorDisplay;
            tabletDetail.TouchDisplay = model.TouchDisplay;
            tabletDetail.DisplayTechnology = model.DisplayTechnology;
            tabletDetail.DisplaySize = model.DisplaySize;
            tabletDetail.Resolution = model.Resolution;
            tabletDetail.PixelDensity = model.PixelDensity;
            tabletDetail.ScreenToBodyRatio = model.ScreenToBodyRatio;
            tabletDetail.ImageRatio = model.ImageRatio;
            tabletDetail.DisplayProtection = model.DisplayProtection;
            tabletDetail.MoreInformation = model.MoreInformation;
            tabletDetail.ConnectionsNetwork = model.ConnectionsNetwork;
            tabletDetail.GsmNetwork = model.GsmNetwork;
            tabletDetail.HspaNetwork = model.HspaNetwork;
            tabletDetail.LteNetwork = model.LteNetwork;
            tabletDetail.FiveGNetwork = model.FiveGNetwork;
            tabletDetail.CommunicationTechnology = model.CommunicationTechnology;
            tabletDetail.WiFi = model.WiFi;
            tabletDetail.Radio = model.Radio;
            tabletDetail.Bluetooth = model.Bluetooth;
            tabletDetail.GpsInformation = model.GpsInformation;
            tabletDetail.ConnectionPort = model.ConnectionPort;
            tabletDetail.CameraQuantity = model.CameraQuantity;
            tabletDetail.PhotoResolutation = model.PhotoResolutation;
            tabletDetail.SelfiCameraPhoto = model.SelfiCameraPhoto;
            tabletDetail.CameraCapabilities = model.CameraCapabilities;
            tabletDetail.SelfiCameraCapabilities = model.SelfiCameraCapabilities;
            tabletDetail.Filming = model.Filming;
            tabletDetail.Speakers = model.Speakers;
            tabletDetail.OutputAudio = model.OutputAudio;
            tabletDetail.AudioInformation = model.AudioInformation;
            tabletDetail.OS = model.OS;
            tabletDetail.OsVersion = model.OsVersion;
            tabletDetail.UiVersion = model.UiVersion;
            tabletDetail.MoreInformationSoftWare = model.MoreInformationSoftWare;
            tabletDetail.BatteryMaterial = model.BatteryMaterial;
            tabletDetail.BatteryCapacity = model.BatteryCapacity;
            tabletDetail.Removable‌Battery = model.Removable‌Battery;
            tabletDetail.Sensors = model.Sensors;
            tabletDetail.ItemsInBox = model.ItemsInBox;



            var result = await _productService.EditTabletAsync(product, tabletDetail);

            if (result == ResultTypes.Successful)
            {
                // edit product images
                //await EditImg(product.ProductId, new List<IFormFile>()
                //    {
                //        model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                //        model.SelectedImage5, model.SelectedImage6
                //    }, new List<string>()
                //    {
                //        model.SelectedImage1IMG,model.SelectedImage2IMG, model.SelectedImage3IMG, model.SelectedImage4IMG,
                //        model.SelectedImage5IMG, model.SelectedImage6IMG
                //    });

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
    }

    #endregion

    #region mobile cover

    [HttpGet]
    public async Task<IActionResult> AddOrEditMobileCover(int productId = 0)
    {
        if (productId == 0)
        {
            var newModel = new AddOrEditMobileCoverViewModel()
            {
                StoreTitles = _shopperService.GetStoreTitles()
            };

            return View(newModel);
        }
        else
        {
            var product = await _productService.GetTypeMobileCoverProductDataAsync(productId);
            if (product is null)
                return NotFound();


            return View(product);
        }
    }

    [HttpPost]

    public async Task<IActionResult> AddOrEditMobileCover(AddOrEditMobileCoverViewModel model)
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
            ModelState.AddModelError("SelectedImage6", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
            return View(model);
        }

        #endregion

        if (model.ProductId == 0)
        {
            // images could not be null
            if (model.SelectedImage1 == null || model.SelectedImage2 == null ||
                model.SelectedImage3 == null || model.SelectedImage4 == null ||
                model.SelectedImage5 == null || model.SelectedImage6 == null)
            {
                ModelState.AddModelError("", "ادمین عزیز لطفا تمام عکس ها را وارد کنید.");
                return View(model);
            }


            var product = new Product()
            {
                ProductTitle = model.ProductTitle,
                Description = model.Description,

                ProductType = ProductTypes.MobileCover.ToString(),

            };

            var mobileCover = new MobileCoverDetail()
            {
                SuitablePhones = model.SuitablePhones,
                Gender = model.Gender,
                Structure = model.Structure,
                CoverLevel = model.CoverLevel,
                Features = model.Features
            };


            var result = await _productService.AddMobileCoverAsync(product, mobileCover);

            if (result == ResultTypes.Successful)
            {
                // add product images
                await AddImg(new List<IFormFile>()
                {
                    model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                    model.SelectedImage5, model.SelectedImage6
                }, product.ProductId);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
        else
        {
            var product = await _productService.GetProductByIdAsync(model.ProductId);

            if (product?.MobileCoverDetailId == null)
                return NotFound();


            var mobileCoverDetail = await _productService.GetMobileCoverByIdAsync(product.MobileCoverDetailId.Value);

            if (mobileCoverDetail == null)
                return NotFound();



            //  update product
            product.ProductTitle = model.ProductTitle;
            product.Description = model.Description;
            product.OfficialBrandProductId = model.OfficialBrandProductId;
            product.IsActive = model.IsActive;

            // update mobile cover detail
            mobileCoverDetail.SuitablePhones = model.SuitablePhones;
            mobileCoverDetail.Gender = model.Gender;
            mobileCoverDetail.Structure = model.Structure;
            mobileCoverDetail.CoverLevel = model.CoverLevel;
            mobileCoverDetail.Features = model.Features;


            var result = await _productService.EditMobileCoverAsync(product, mobileCoverDetail);

            if (result == ResultTypes.Successful)
            {
                // edit product images
                //await EditImg(product.ProductId, new List<IFormFile>()
                //    {
                //        model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                //        model.SelectedImage5, model.SelectedImage6
                //    }, new List<string>()
                //    {
                //        model.SelectedImage1IMG,model.SelectedImage2IMG, model.SelectedImage3IMG, model.SelectedImage4IMG,
                //        model.SelectedImage5IMG, model.SelectedImage6IMG
                //    });

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }


        }
    }

    #endregion

    #region laptop cover

    [HttpGet]
    public async Task<IActionResult> AddOrEditLaptopCover(int productId = 0)
    {
        if (productId == 0)
        {
            var newModel = new AddOrEditLaptopProductViewModel()
            {
                StoreTitles = _shopperService.GetStoreTitles()
            };

            return View(newModel);
        }
        else
        {
            var product = await _productService.GetTypeLaptopProductDataAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }

    #endregion

    #region speaker

    [HttpGet]
    public async Task<IActionResult> AddOrEditSpeaker(int productId = 0)
    {
        if (productId == 0)
        {
            var newModel = new AddOrEditSpeakerViewModel()
            {
                StoreTitles = _shopperService.GetStoreTitles()
            };

            return View(newModel);
        }
        else
        {
            var product = await _productService.GetTypeSpeakerProductDataAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }

    [HttpPost]

    public async Task<IActionResult> AddOrEditSpeaker(AddOrEditSpeakerViewModel model)
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
            ModelState.AddModelError("SelectedImage6", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
            return View(model);
        }

        #endregion

        if (model.ProductId == 0)
        {
            // images could not be null
            if (model.SelectedImage1 == null || model.SelectedImage2 == null ||
                model.SelectedImage3 == null || model.SelectedImage4 == null ||
                model.SelectedImage5 == null || model.SelectedImage6 == null)
            {
                ModelState.AddModelError("", "ادمین عزیز لطفا تمام عکس ها را وارد کنید.");
                return View(model);
            }

            var product = new Product()
            {
                ProductTitle = model.ProductTitle,
                Description = model.Description,
                ProductType = ProductTypes.Mobile.ToString(),
                OfficialBrandProductId = model.OfficialBrandProductId,
                IsActive = model.IsActive
            };

            var speakerDetail = new SpeakerDetail()
            {
                Lenght = model.Lenght,
                Width = model.Width,
                Height = model.Height,
                ConnectionType = model.ConnectionType,
                Connector = model.Connector,
                IsMemoryCardInput = model.IsMemoryCardInput,
                IsSupportUSBPort = model.IsSupportUSBPort,
                HeadphoneOutput = model.HeadphoneOutput,
                InputSound = model.InputSound,
                MicrophoneInpute = model.MicrophoneInpute,
                IsSupportMicrophone = model.IsSupportMicrophone,
                Display = model.Display,
                ControlRemote = model.ControlRemote,
                IsSupportRadio = model.IsSupportRadio,
                Bluetooth = model.Bluetooth,
                ConnectTwoDevice = model.ConnectTwoDevice,
                SpeakerItemQuantity = model.SpeakerItemQuantity,
                IsBattery = model.IsBattery,
                PlayingTime = model.PlayingTime,
                ChargingTime = model.ChargingTime,
                OsSoppurt = model.OsSoppurt,
            };


            var result = await _productService.AddSpeakerAsync(product, speakerDetail);

            if (result == ResultTypes.Successful)
            {
                // add product images
                await AddImg(new List<IFormFile>()
                {
                    model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                    model.SelectedImage5, model.SelectedImage6
                }, product.ProductId);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
        else
        {
            var product = await _productService.GetProductByIdAsync(model.ProductId);

            if (product?.SpeakerDetailId == null)
                return NotFound();


            var speakerDetail = await _productService.GetSpeakerByIdAsync(product.SpeakerDetailId.Value);

            if (speakerDetail == null)
                return NotFound();



            //  update product
            product.ProductTitle = model.ProductTitle;
            product.Description = model.Description;
            product.OfficialBrandProductId = model.OfficialBrandProductId;
            product.IsActive = model.IsActive;

            // update mobile cover detail
            speakerDetail.Lenght = model.Lenght;
            speakerDetail.Width = model.Width;
            speakerDetail.Height = model.Height;
            speakerDetail.ConnectionType = model.ConnectionType;
            speakerDetail.Connector = model.Connector;
            speakerDetail.IsMemoryCardInput = model.IsMemoryCardInput;
            speakerDetail.IsSupportUSBPort = model.IsSupportUSBPort;
            speakerDetail.HeadphoneOutput = model.HeadphoneOutput;
            speakerDetail.InputSound = model.InputSound;
            speakerDetail.MicrophoneInpute = model.MicrophoneInpute;
            speakerDetail.IsSupportMicrophone = model.IsSupportMicrophone;
            speakerDetail.Display = model.Display;
            speakerDetail.ControlRemote = model.ControlRemote;
            speakerDetail.IsSupportRadio = model.IsSupportRadio;
            speakerDetail.Bluetooth = model.Bluetooth;
            speakerDetail.ConnectTwoDevice = model.ConnectTwoDevice;
            speakerDetail.SpeakerItemQuantity = model.SpeakerItemQuantity;
            speakerDetail.IsBattery = model.IsBattery;
            speakerDetail.PlayingTime = model.PlayingTime;
            speakerDetail.ChargingTime = model.ChargingTime;
            speakerDetail.OsSoppurt = model.OsSoppurt;



            var result = await _productService.EditSpeakerAsync(product, speakerDetail);

            if (result == ResultTypes.Successful)
            {
                // edit product images
                //await EditImg(product.ProductId, new List<IFormFile>()
                //    {
                //        model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                //        model.SelectedImage5, model.SelectedImage6
                //    }, new List<string>()
                //    {
                //        model.SelectedImage1IMG,model.SelectedImage2IMG, model.SelectedImage3IMG, model.SelectedImage4IMG,
                //        model.SelectedImage5IMG, model.SelectedImage6IMG
                //    });

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }

        }
    }

    #endregion

    #region power bank

    [HttpGet]
    public async Task<IActionResult> AddOrEditPowerBank(int productId = 0)
    {
        if (productId == 0)
        {
            var newModel = new AddOrEditPowerBankViewModel()
            {
                StoreTitles = _shopperService.GetStoreTitles()
            };

            return View(newModel);
        }
        else
        {
            var product = await _productService.GetTypeLaptopProductDataAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }

    [HttpPost]

    public async Task<IActionResult> AddOrEditPowerBank(AddOrEditPowerBankViewModel model)
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
            ModelState.AddModelError("SelectedImage6", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
            return View(model);
        }

        #endregion




        if (model.ProductId == 0)
        {
            // images could not be null
            if (model.SelectedImage1 == null || model.SelectedImage2 == null ||
                model.SelectedImage3 == null || model.SelectedImage4 == null ||
                model.SelectedImage5 == null || model.SelectedImage6 == null)
            {
                ModelState.AddModelError("", "ادمین عزیز لطفا تمام عکس ها را وارد کنید.");
                return View(model);
            }



            var product = new Product()
            {
                ProductTitle = model.ProductTitle,
                Description = model.Description,
                ProductType = ProductTypes.Mobile.ToString(),
                OfficialBrandProductId = model.OfficialBrandProductId,
                IsActive = model.IsActive
            };

            var powerBank = new PowerBankDetail()
            {
                Length = model.Length,
                Width = model.Width,
                Height = model.Height,
                Weight = model.Weight,
                CapacityRange = model.CapacityRange,
                InputVoltage = model.InputVoltage,
                OutputVoltage = model.OutputVoltage,
                InputCurrentIntensity = model.InputCurrentIntensity,
                OutputCurrentIntensity = model.OutputCurrentIntensity,
                OutputPortsCount = model.OutputPortsCount,
                IsSupportOfQCTechnology = model.IsSupportOfQCTechnology,
                IsSupportOfPDTechnology = model.IsSupportOfPDTechnology,
                BodyMaterial = model.BodyMaterial,
                DisplayCharge = model.DisplayCharge,
                Features = model.Features,
            };

            var result = await _productService.AddPowerBankAsync(product, powerBank);

            if (result == ResultTypes.Successful)
            {
                // add product images
                await AddImg(new List<IFormFile>()
                {
                    model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                    model.SelectedImage5, model.SelectedImage6
                }, product.ProductId);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
        else
        {
            var product = await _productService.GetProductByIdAsync(model.ProductId);

            if (product?.PowerBankDetailId == null)
                return NotFound();


            var powerBankDetail = await _productService.GetPowerBankDetailByIdAsync(product.PowerBankDetailId.Value);

            if (powerBankDetail == null)
                return NotFound();




            product.ProductTitle = model.ProductTitle;
            product.Description = model.Description;

            product.OfficialBrandProductId = model.OfficialBrandProductId;
            product.IsActive = model.IsActive;

            // update mobile detail
            powerBankDetail.Length = model.Length;
            powerBankDetail.Width = model.Width;
            powerBankDetail.Height = model.Height;
            powerBankDetail.Weight = model.Weight;
            powerBankDetail.CapacityRange = model.CapacityRange;
            powerBankDetail.InputVoltage = model.InputVoltage;
            powerBankDetail.OutputVoltage = model.OutputVoltage;
            powerBankDetail.InputCurrentIntensity = model.InputCurrentIntensity;
            powerBankDetail.OutputCurrentIntensity = model.OutputCurrentIntensity;
            powerBankDetail.OutputPortsCount = model.OutputPortsCount;
            powerBankDetail.IsSupportOfQCTechnology = model.IsSupportOfQCTechnology;
            powerBankDetail.IsSupportOfPDTechnology = model.IsSupportOfPDTechnology;
            powerBankDetail.BodyMaterial = model.BodyMaterial;
            powerBankDetail.DisplayCharge = model.DisplayCharge;
            powerBankDetail.Features = model.Features;



            var result = await _productService.EditPowerBankAsync(product, powerBankDetail);

            if (result == ResultTypes.Successful)
            {
                // edit product images
                //await EditImg(product.ProductId, new List<IFormFile>()
                //    {
                //        model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                //        model.SelectedImage5, model.SelectedImage6
                //    }, new List<string>()
                //    {
                //        model.SelectedImage1IMG,model.SelectedImage2IMG, model.SelectedImage3IMG, model.SelectedImage4IMG,
                //        model.SelectedImage5IMG, model.SelectedImage6IMG
                //    });

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
    }

    #endregion

    #region wrist watch

    [HttpGet]
    public async Task<IActionResult> AddOrEditWristWatch(int productId = 0)
    {
        if (productId == 0)
        {
            var newModel = new AddOrEdirWristWatchViewModel()
            {
                StoreTitles = _shopperService.GetStoreTitles()
            };

            return View(newModel);
        }
        else
        {
            var product = await _productService.GetTypeWristWatchProductDataAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }

    [HttpPost]

    public async Task<IActionResult> AddOrEditWristWatch(AddOrEdirWristWatchViewModel model)
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
            ModelState.AddModelError("SelectedImage6", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
            return View(model);
        }

        #endregion

        if (model.ProductId == 0)
        {
            // images could not be null
            if (model.SelectedImage1 == null || model.SelectedImage2 == null ||
                model.SelectedImage3 == null || model.SelectedImage4 == null ||
                model.SelectedImage5 == null || model.SelectedImage6 == null)
            {
                ModelState.AddModelError("", "ادمین عزیز لطفا تمام عکس ها را وارد کنید.");
                return View(model);
            }

            var product = new Product()
            {
                ProductTitle = model.ProductTitle,
                Description = model.Description,
                ProductType = ProductTypes.Mobile.ToString(),

                OfficialBrandProductId = model.OfficialBrandProductId,
                IsActive = model.IsActive
            };

            var wristWatchDetail = new WristWatchDetail()
            {

            };


            var result = await _productService.AddWristWatchAsync(product, wristWatchDetail);

            if (result == ResultTypes.Successful)
            {
                // add product images
                await AddImg(new List<IFormFile>()
                {
                    model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                    model.SelectedImage5, model.SelectedImage6
                }, product.ProductId);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
        else
        {
            var product = await _productService.GetProductByIdAsync(model.ProductId);

            if (product?.WristWatchDetailId == null)
                return NotFound();


            var wristWatchDetail = await _productService.GetWristWatchByIdAsync(product.WristWatchDetailId.Value);

            if (wristWatchDetail == null)
                return NotFound();



            //  update product
            product.ProductTitle = model.ProductTitle;
            product.Description = model.Description;

            product.OfficialBrandProductId = model.OfficialBrandProductId;
            product.IsActive = model.IsActive;

            // update mobile cover detail




            var result = await _productService.EditWristWatchAsync(product, wristWatchDetail);

            if (result == ResultTypes.Successful)
            {
                // edit product images
                //await EditImg(product.ProductId, new List<IFormFile>()
                //    {
                //        model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                //        model.SelectedImage5, model.SelectedImage6
                //    }, new List<string>()
                //    {
                //        model.SelectedImage1IMG,model.SelectedImage2IMG, model.SelectedImage3IMG, model.SelectedImage4IMG,
                //        model.SelectedImage5IMG, model.SelectedImage6IMG
                //    });

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
    }

    #endregion

    #region smart watch

    [HttpGet]
    public async Task<IActionResult> AddOrEditSmartWatch(int productId = 0)
    {
        if (productId == 0)
        {
            var newModel = new AddOrEditSmartWatchViewModel()
            {
                StoreTitles = _shopperService.GetStoreTitles()
            };

            return View(newModel);
        }
        else
        {
            var product = await _productService.GetTypeSmartWatchProductDataAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }

    [HttpPost]

    public async Task<IActionResult> AddOrEditSmartWatch(AddOrEditSmartWatchViewModel model)
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
            ModelState.AddModelError("SelectedImage6", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
            return View(model);
        }

        #endregion

        if (model.ProductId == 0)
        {
            // images could not be null
            if (model.SelectedImage1 == null || model.SelectedImage2 == null ||
                model.SelectedImage3 == null || model.SelectedImage4 == null ||
                model.SelectedImage5 == null || model.SelectedImage6 == null)
            {
                ModelState.AddModelError("", "ادمین عزیز لطفا تمام عکس ها را وارد کنید.");
                return View(model);
            }

            var product = new Product()
            {
                ProductTitle = model.ProductTitle,
                Description = model.Description,
                ProductType = ProductTypes.Mobile.ToString(),
                OfficialBrandProductId = model.OfficialBrandProductId,
                IsActive = model.IsActive
            };

            var smartWatchDetail = new SmartWatchDetail()
            {
                Lenght = model.Lenght,
                Width = model.Width,
                Height = model.Height,
                Weight = model.Weight,
                SuitableFor = model.SuitableFor,
                Application = model.Application,
                DisplayForm = model.DisplayForm,
                GlassMaterial = model.GlassMaterial,
                CaseMaterial = model.CaseMaterial,
                MaterialStrap = model.MaterialStrap,
                TypeOfLock = model.TypeOfLock,
                ColorDisplay = model.ColorDisplay,
                TouchDisplay = model.TouchDisplay,
                DisplaySize = model.DisplaySize,
                Resolution = model.Resolution,
                PixelDensity = model.PixelDensity,
                DisplayType = model.DisplayType,
                MoreInformationDisplay = model.MoreInformationDisplay,
                SimcardIsSoppurt = model.SimcardIsSoppurt,
                RegisteredSimCardIsSoppurt = model.RegisteredSimCardIsSoppurt,
                GpsIsSoppurt = model.GpsIsSoppurt,
                Os = model.Os,
                Compatibility = model.Compatibility,
                Prossecor = model.Prossecor,
                InternalStorage = model.InternalStorage,
                ExternalStorageSoppurt = model.ExternalStorageSoppurt,
                Camera = model.Camera,
                MusicControl = model.MusicControl,
                Connections = model.Connections,
                Sensors = model.Sensors,
                BatteryMaterial = model.BatteryMaterial,
                CallIsSoppurt = model.CallIsSoppurt,
                MoreInformationHardware = model.MoreInformationHardware,
            };


            var result = await _productService.AddSmartWatchAsync(product, smartWatchDetail);

            if (result == ResultTypes.Successful)
            {
                // add product images
                await AddImg(new List<IFormFile>()
                {
                    model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                    model.SelectedImage5, model.SelectedImage6
                }, product.ProductId);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
        else
        {
            var product = await _productService.GetProductByIdAsync(model.ProductId);

            if (product?.SmartWatchDetailId == null)
                return NotFound();


            var smartWatchDetail = await _productService.GetSmartWatchByIdAsync(product.SmartWatchDetailId.Value);

            if (smartWatchDetail == null)
                return NotFound();



            //  update product
            product.ProductTitle = model.ProductTitle;
            product.Description = model.Description;
            product.OfficialBrandProductId = model.OfficialBrandProductId;
            product.IsActive = model.IsActive;

            // update mobile cover detail
            smartWatchDetail.Lenght = model.Lenght;
            smartWatchDetail.Width = model.Width;
            smartWatchDetail.Height = model.Height;
            smartWatchDetail.Weight = model.Weight;
            smartWatchDetail.SuitableFor = model.SuitableFor;
            smartWatchDetail.Application = model.Application;
            smartWatchDetail.DisplayForm = model.DisplayForm;
            smartWatchDetail.GlassMaterial = model.GlassMaterial;
            smartWatchDetail.CaseMaterial = model.CaseMaterial;
            smartWatchDetail.MaterialStrap = model.MaterialStrap;
            smartWatchDetail.TypeOfLock = model.TypeOfLock;
            smartWatchDetail.ColorDisplay = model.ColorDisplay;
            smartWatchDetail.TouchDisplay = model.TouchDisplay;
            smartWatchDetail.DisplaySize = model.DisplaySize;
            smartWatchDetail.Resolution = model.Resolution;
            smartWatchDetail.PixelDensity = model.PixelDensity;
            smartWatchDetail.DisplayType = model.DisplayType;
            smartWatchDetail.MoreInformationDisplay = model.MoreInformationDisplay;
            smartWatchDetail.SimcardIsSoppurt = model.SimcardIsSoppurt;
            smartWatchDetail.RegisteredSimCardIsSoppurt = model.RegisteredSimCardIsSoppurt;
            smartWatchDetail.GpsIsSoppurt = model.GpsIsSoppurt;
            smartWatchDetail.Os = model.Os;
            smartWatchDetail.Compatibility = model.Compatibility;
            smartWatchDetail.Prossecor = model.Prossecor;
            smartWatchDetail.InternalStorage = model.InternalStorage;
            smartWatchDetail.ExternalStorageSoppurt = model.ExternalStorageSoppurt;
            smartWatchDetail.Camera = model.Camera;
            smartWatchDetail.MusicControl = model.MusicControl;
            smartWatchDetail.Connections = model.Connections;
            smartWatchDetail.Sensors = model.Sensors;
            smartWatchDetail.BatteryMaterial = model.BatteryMaterial;
            smartWatchDetail.CallIsSoppurt = model.CallIsSoppurt;
            smartWatchDetail.MoreInformationHardware = model.MoreInformationHardware;



            var result = await _productService.EditSmartWatchAsync(product, smartWatchDetail);

            if (result == ResultTypes.Successful)
            {
                // edit product images
                //await EditImg(product.ProductId, new List<IFormFile>()
                //    {
                //        model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                //        model.SelectedImage5, model.SelectedImage6
                //    }, new List<string>()
                //    {
                //        model.SelectedImage1IMG,model.SelectedImage2IMG, model.SelectedImage3IMG, model.SelectedImage4IMG,
                //        model.SelectedImage5IMG, model.SelectedImage6IMG
                //    });

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
    }

    #endregion

    #region flash memory

    [HttpGet]
    public async Task<IActionResult> AddOrEditFlashMemory(int productId = 0)
    {
        if (productId == 0)
        {
            var newModel = new AddOrEditFlashMemoryViewModel()
            {
                StoreTitles = _shopperService.GetStoreTitles()
            };

            return View(newModel);
        }
        else
        {
            var product = await _productService.GetTypeLaptopProductDataAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddOrEditFlashMemory(AddOrEditFlashMemoryViewModel model)
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
            ModelState.AddModelError("SelectedImage6", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
            return View(model);
        }

        #endregion

        if (model.ProductId == 0)
        {
            // images could not be null
            if (model.SelectedImage1 == null || model.SelectedImage2 == null ||
                model.SelectedImage3 == null || model.SelectedImage4 == null ||
                model.SelectedImage5 == null || model.SelectedImage6 == null)
            {
                ModelState.AddModelError("", "ادمین عزیز لطفا تمام عکس ها را وارد کنید.");
                return View(model);
            }


            var product = new Product()
            {
                ProductTitle = model.ProductTitle,
                Description = model.Description,
                ProductType = ProductTypes.Mobile.ToString(),
                OfficialBrandProductId = model.OfficialBrandProductId,
                IsActive = model.IsActive
            };

            var flashMemoryDetail = new FlashMemoryDetail()
            {
                Length = model.Length,
                Width = model.Width,
                Height = model.Height,
                BodyMaterial = model.BodyMaterial,
                Connector = model.Connector,
                Capacity = model.Capacity,
                Led = model.Led,
                IsImpactResistance = model.IsImpactResistance,
                WaterResistance = model.WaterResistance,
                ShockResistance = model.ShockResistance,
                DustResistance = model.DustResistance,
                AntiScratch = model.AntiScratch,
                AntiStain = model.AntiStain,
                SpeedDataTransfer = model.SpeedDataTransfer,
                SpeedDataReading = model.SpeedDataReading,
                OsCompatibility = model.OsCompatibility,
                MoreInformation = model.MoreInformation,
            };


            var result = await _productService.AddFlashMemoryAsync(product, flashMemoryDetail);

            if (result == ResultTypes.Successful)
            {
                // add product images
                await AddImg(new List<IFormFile>()
                {
                    model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                    model.SelectedImage5, model.SelectedImage6
                }, product.ProductId);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }

        }
        else
        {
            var product = await _productService.GetProductByIdAsync(model.ProductId);

            if (product?.FlashMemoryDetailId == null)
                return NotFound();


            var flashMemoryDetail = await _productService.GetFlashMemoryByIdAsync(product.FlashMemoryDetailId.Value);

            if (flashMemoryDetail == null)
                return NotFound();



            //  update product
            product.ProductTitle = model.ProductTitle;
            product.Description = model.Description;
            product.OfficialBrandProductId = model.OfficialBrandProductId;
            product.IsActive = model.IsActive;

            // update mobile cover detail
            flashMemoryDetail.Length = model.Length;
            flashMemoryDetail.Width = model.Width;
            flashMemoryDetail.Height = model.Height;
            flashMemoryDetail.BodyMaterial = model.BodyMaterial;
            flashMemoryDetail.Connector = model.Connector;
            flashMemoryDetail.Capacity = model.Capacity;
            flashMemoryDetail.Led = model.Led;
            flashMemoryDetail.IsImpactResistance = model.IsImpactResistance;
            flashMemoryDetail.WaterResistance = model.WaterResistance;
            flashMemoryDetail.ShockResistance = model.ShockResistance;
            flashMemoryDetail.DustResistance = model.DustResistance;
            flashMemoryDetail.AntiScratch = model.AntiScratch;
            flashMemoryDetail.AntiStain = model.AntiStain;
            flashMemoryDetail.SpeedDataTransfer = model.SpeedDataTransfer;
            flashMemoryDetail.SpeedDataReading = model.SpeedDataReading;
            flashMemoryDetail.OsCompatibility = model.OsCompatibility;
            flashMemoryDetail.MoreInformation = model.MoreInformation;



            var result = await _productService.EditFlashMemoryAsync(product, flashMemoryDetail);

            if (result == ResultTypes.Successful)
            {
                // edit product images
                //await EditImg(product.ProductId, new List<IFormFile>()
                //    {
                //        model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                //        model.SelectedImage5, model.SelectedImage6
                //    }, new List<string>()
                //    {
                //        model.SelectedImage1IMG,model.SelectedImage2IMG, model.SelectedImage3IMG, model.SelectedImage4IMG,
                //        model.SelectedImage5IMG, model.SelectedImage6IMG
                //    });

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
    }


    #endregion

    #region memory card

    [HttpGet]
    public async Task<IActionResult> AddOrEditMemoryCard(int productId = 0)
    {
        if (productId == 0)
        {
            var newModel = new AddOrEditMemoryCardViewModel()
            {
                StoreTitles = _shopperService.GetStoreTitles()
            };

            return View(newModel);
        }
        else
        {
            var product = await _productService.GetTypeMemoryCardProductDataAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }

    [HttpPost]

    public async Task<IActionResult> AddOrEditMemoryCard(AddOrEditMemoryCardViewModel model)
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
            ModelState.AddModelError("SelectedImage6", "ادمین عزیز لطفا تعصیر خود را به درستی انتخاب کنید.");
            return View(model);
        }

        #endregion

        if (model.ProductId == 0)
        {
            // images could not be null
            if (model.SelectedImage1 == null || model.SelectedImage2 == null ||
                model.SelectedImage3 == null || model.SelectedImage4 == null ||
                model.SelectedImage5 == null || model.SelectedImage6 == null)
            {
                ModelState.AddModelError("", "ادمین عزیز لطفا تمام عکس ها را وارد کنید.");
                return View(model);
            }

            var product = new Product()
            {
                ProductTitle = model.ProductTitle,
                Description = model.Description,
                ProductType = ProductTypes.Mobile.ToString(),

                OfficialBrandProductId = model.OfficialBrandProductId,
                IsActive = model.IsActive
            };

            var memoryCardDetail = new MemoryCardDetail()
            {
                Length = model.Length,
                Width = model.Width,
                Height = model.Height,
                Capacity = model.Capacity,
                SpeedStandard = model.SpeedStandard,
                ReadingSpeed = model.ReadingSpeed,
                ResistsAgainst = model.ResistsAgainst,
                MoreInformation = model.MoreInformation,
            };


            var result = await _productService.AddMemoryCardAsync(product, memoryCardDetail);

            if (result == ResultTypes.Successful)
            {
                // add product images
                await AddImg(new List<IFormFile>()
                {
                    model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                    model.SelectedImage5, model.SelectedImage6
                }, product.ProductId);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
        else
        {
            var product = await _productService.GetProductByIdAsync(model.ProductId);

            if (product?.MemoryCardDetailId == null)
                return NotFound();


            var memoryCardDetail = await _productService.GetMemoryCardByIdAsync(product.MemoryCardDetailId.Value);

            if (memoryCardDetail == null)
                return NotFound();



            //  update product
            product.ProductTitle = model.ProductTitle;
            product.Description = model.Description;

            product.OfficialBrandProductId = model.OfficialBrandProductId;
            product.IsActive = model.IsActive;

            // update mobile cover detail
            memoryCardDetail.Length = model.Length;
            memoryCardDetail.Width = model.Width;
            memoryCardDetail.Height = model.Height;
            memoryCardDetail.Capacity = model.Capacity;
            memoryCardDetail.SpeedStandard = model.SpeedStandard;
            memoryCardDetail.ReadingSpeed = model.ReadingSpeed;
            memoryCardDetail.ResistsAgainst = model.ResistsAgainst;
            memoryCardDetail.MoreInformation = model.MoreInformation;



            var result = await _productService.EditMemoryCardAsync(product, memoryCardDetail);

            if (result == ResultTypes.Successful)
            {
                // edit product images
                //await EditImg(product.ProductId, new List<IFormFile>()
                //    {
                //        model.SelectedImage1, model.SelectedImage2, model.SelectedImage3, model.SelectedImage4,
                //        model.SelectedImage5, model.SelectedImage6
                //    }, new List<string>()
                //    {
                //        model.SelectedImage1IMG,model.SelectedImage2IMG, model.SelectedImage3IMG, model.SelectedImage4IMG,
                //        model.SelectedImage5IMG, model.SelectedImage6IMG
                //    });

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("",
                    $"ادمین عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                return View(model);
            }
        }
    }

    #endregion

    #region AUX

    [HttpGet]
    [Permission(PermissionsConstants.AddAux)]
    public IActionResult AddAux()
    {
        var newModel = new AddAuxViewModel()
        {
            StoreTitles = _shopperService.GetStoreTitles()
        };

        return View(newModel);
    }

    [HttpPost]
    [Permission(PermissionsConstants.EditAux)]
    public async Task<IActionResult> AddAux(AddAuxViewModel model)
    {
        // data for select Product
        model.StoreTitles = _shopperService.GetStoreTitles();
        model.Brands = _brandService.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
        model.OfficialProducts = _brandService.GetBrandOfficialProducts(model.SelectedBrand);
        model.ChildCategories = _brandService.GetChildCategoriesOfBrand(model.SelectedBrand);

        if (!ModelState.IsValid) return View(model);

        // in this section we check that all images are ok

        #region images security

        if (model.Images != null)
        {
            foreach (var image in model.Images)
            {
                if (!image.IsImage())
                {
                    ModelState.AddModelError("", "لطفا تصاویر خود را به درستی انتخاب کنید");
                    return View(model);
                }
            }
        }

        #endregion



        if (model.Images == null)
        {
            ModelState.AddModelError("", "لطفا تصاویر کالا را انتخاب کنید.");
            return View(model);
        }
        else if (model.Images.Count <= 2)
        {
            ModelState.AddModelError("", "تصاویر کالا باید حداقل 3 تصویر باشد.");
            return View(model);
        }


        var product = new Product()
        {
            ProductTitle = model.ProductTitle,
            Description = model.Description,
            ProductType = ProductTypes.AUX.ToString(),
            OfficialBrandProductId = model.OfficialBrandProductId,
            IsActive = model.IsActive,
            ChildCategoryId = model.SelectedChildCategory
        };

        var auxDetail = new AUXDetail()
        {
            CableMaterial = model.CableMaterial,
            CableLenght = model.CableLenght
        };


        var result = await _productService.AddAUXAsync(product, auxDetail);

        if (result == ResultTypes.Successful)
        {
            // add product images
            await AddImg(model.Images, product.ProductId);


            return RedirectToAction(nameof(Index));
        }
        else
        {
            ModelState.AddModelError("",
                $"ادمین عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

            return View(model);
        }

    }


    [HttpGet]
    [Permission(PermissionsConstants.EditAux)]
    public async Task<IActionResult> EditAux(int productId = 0)
    {
        if (productId == 0)
        {
            var newModel = new EditAuxViewModel()
            {
                StoreTitles = _shopperService.GetStoreTitles()
            };

            return View(newModel);
        }
        else
        {
            var product = await _productService.GetTypeAUXProductDataAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }

    [HttpPost]
    [Permission(PermissionsConstants.EditAux)]
    public async Task<IActionResult> EditAux(EditAuxViewModel model)
    {
        // data for select Product
        model.StoreTitles = _shopperService.GetStoreTitles();
        model.Brands = _brandService.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
        model.OfficialProducts = _brandService.GetBrandOfficialProducts(model.SelectedBrand);
        model.ChildCategories = _brandService.GetChildCategoriesOfBrand(model.SelectedBrand);

        if (!ModelState.IsValid) return View(model);

        // in this section we check that all images are ok

        #region images security

        if (model.Images != null)
        {
            foreach (var image in model.Images)
            {
                if (!image.IsImage())
                {
                    ModelState.AddModelError("", "لطفا تصاویر خود را به درستی انتخاب کنید");
                    return View(model);
                }
            }
        }

        #endregion


        var product = await _productService.GetProductByIdAsync(model.ProductId);

        if (product?.AuxDetailId == null)
            return NotFound();


        var auxDetail = await _productService.GetAUXByIdAsync(product.AuxDetailId.Value);

        if (auxDetail == null)
            return NotFound();



        //  update product
        product.ProductTitle = model.ProductTitle;
        product.Description = model.Description;
        product.OfficialBrandProductId = model.OfficialBrandProductId;
        product.IsActive = model.IsActive;
        product.ChildCategoryId = model.SelectedChildCategory;

        // update mobile cover detail
        auxDetail.CableLenght = model.CableLenght;
        auxDetail.CableMaterial = model.CableMaterial;



        var result = await _productService.EditAUXAsync(product, auxDetail);

        if (result == ResultTypes.Successful)
        {
            // edit product images
            await EditImg(product.ProductId, model.Images, model.SelectedImages as List<string>,
                model.ChangedImages as List<string>, model.DeletedImages as List<string>);

            return RedirectToAction(nameof(Index));
        }
        else
        {
            ModelState.AddModelError("",
                $"ادمین عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

            return View(model);
        }
    }

    #endregion

    // ---------------------------------------------------------------------

    #region Remove

    [HttpPost]

    public async Task<IActionResult> DeleteProduct(int productId)
    {
        await _productService.RemoveProductAccessAsync(productId);

        return RedirectToAction("Index");
    }

    #endregion

    #region private methods

    private async Task AddImg(List<IFormFile> images, int productId)
    {

        for (int i = 0; i < images.Count; i++)
        {
            if (images[i].Length > 0)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "ProductImages");

                string imgName = await ImageConvertor.CreateNewImage(images[i], path, 1600);

                var productGallery = new ProductGallery()
                {
                    ProductId = productId,
                    ImageName = imgName,
                    OrderBy = i + 1,
                };

                await _productService.AddProductGalleryAsync(productGallery);
            }
        }
    }

    private async Task EditImg(int productId, List<IFormFile> newImages, List<string> imagesName,
        List<string> editedImages, List<string> deletedImages)
    {
        if (newImages == null && deletedImages == null) return;

        imagesName ??= new List<string>();


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

        // delete selected images 
        if (deletedImages != null && deletedImages.Count > 0)
        {
            foreach (string deletedImage in deletedImages)
            {
                int num = int.Parse(deletedImage) - 1;

                var imageName = imagesName[num];

                var imageInDatabase = await _productService.GetProductGalleryAsync(productId, imageName);

                if (imageInDatabase != null)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images",
                        "ProductImages");

                    // delete last image
                    ImageConvertor.DeleteImage(filePath + "/" + imageName);
                    await _productService.DeleteProductGalleryAsync(imageInDatabase);
                }
            }
        }

        if (newImages == null) return;

        // edit selected Images
        if (editedImages != null && editedImages.Count > 0)
        {
            int editImageCounter = 0;
            editedImages = editedImages.Distinct().ToList();


            foreach (string editedImage in editedImages)
            {
                int num = int.Parse(editedImage) - 1;

                var imageName = imagesName[num];
                IFormFile imageFile = newImages[editImageCounter];

                editImageCounter++;

                if (imageFile != null && imageFile.Length > 0)
                {
                    var imageInDatabase = await _productService.GetProductGalleryAsync(productId, imageName);

                    if (imageInDatabase != null)
                    {
                        int imageOrderBy = imageInDatabase.OrderBy;


                        string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot",
                            "images",
                            "ProductImages");

                        // delete last image
                        ImageConvertor.DeleteImage(filePath + "/" + imageName);

                        await _productService.DeleteProductGalleryAsync(imageInDatabase);
                        // create new image

                        var newImageName = await ImageConvertor.CreateNewImage(imageFile, filePath, 1600);

                        var newProductGallery = new ProductGallery()
                        {
                            ProductId = productId,
                            ImageName = newImageName,
                            OrderBy = imageOrderBy,
                        };

                        await _productService.AddProductGalleryAsync(newProductGallery);
                    }

                }
            }
        }

        // skip edited images and add new images
        if (editedImages != null)
        {
            newImages = newImages.Skip(editedImages.Count).ToList();
        }

        int newImagesOrderByCounter = imagesName.Count + 1;

        foreach (var image in newImages)
        {
            if (image.Length > 0)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "ProductImages");

                string imgName = await ImageConvertor.CreateNewImage(image, path, 1600);

                var productGallery = new ProductGallery()
                {
                    ProductId = productId,
                    ImageName = imgName,
                    OrderBy = newImagesOrderByCounter,
                };

                await _productService.AddProductGalleryAsync(productGallery);

                newImagesOrderByCounter++;
            }
        }
    }

    #endregion
}
