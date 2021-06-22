using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Generator;
using Reshop.Application.Interfaces.Product;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Product;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Application.Enums.Product;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ProductManagerController : Controller
    {
        #region constructor

        private readonly IProductService _productService;
        private readonly IShopperService _shopperService;

        public ProductManagerController(IProductService productService, IShopperService shopperService)
        {
            _productService = productService;
            _shopperService = shopperService;
        }

        #endregion


        [HttpGet]
        public async Task<IActionResult> Index(ProductTypes type = ProductTypes.All, SortTypes soryBy = SortTypes.News, int pageId = 1)
        {
            return View(await _productService.GetProductsWithPaginationAsync(type, soryBy, pageId, 24));
        }


        #region ProductType

        [HttpGet]
        public async Task<IActionResult> EditProduct(int productId)
        {
            var productType = await _productService.GetProductTypeByIdAsync(productId);




            return productType switch
            {
                ProductTypes.Mobile => RedirectToAction("AddOrEditMobile", "ProductManager",
                    new { productId = productId }),

                ProductTypes.Laptop => RedirectToAction("AddOrEditLaptop", "ProductManager",
                    new { productId = productId }),

                ProductTypes.Tablet => RedirectToAction("AddOrEditTablet", "ProductManager",
                    new { productId = productId }),

                ProductTypes.MobileCover => RedirectToAction("AddOrEditMobileCover", "ProductManager",
                    new { productId = productId }),

                ProductTypes.LaptopCover => RedirectToAction("AddOrEditLaptopCover", "ProductManager",
                    new { productId = productId }),

                ProductTypes.Handsfree or ProductTypes.Handsfree => RedirectToAction("AddOrEditHandsfreeAndHeadPhone", "ProductManager",
                    new { productId = productId }),

                ProductTypes.Speaker => RedirectToAction("AddOrEditSpeaker", "ProductManager",
                    new { productId = productId }),

                ProductTypes.PowerBank => RedirectToAction("AddOrEditPowerBank", "ProductManager",
                    new { productId = productId }),

                ProductTypes.WristWatch => RedirectToAction("AddOrEditWristWatch", "ProductManager",
                    new { productId = productId }),

                ProductTypes.SmartWatch => RedirectToAction("AddOrEditSmartWatch", "ProductManager",
                    new { productId = productId }),

                ProductTypes.FlashMemory => RedirectToAction("AddOrEditFlashMemory", "ProductManager",
                    new { productId = productId }),

                ProductTypes.NotFound => NotFound(),

                _ => RedirectToAction("Index")
            };
        }

        #endregion

        #region mobile

        [HttpGet]
        public async Task<IActionResult> AddOrEditMobile(int productId = 0)
        {
            return productId == 0 ? View(new AddOrEditMobileProductViewModel() { ProductId = 0 }) : View(await _productService.GetTypeMobileProductDataAsync(productId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditMobile(AddOrEditMobileProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userFirstName = User.FindFirstValue(ClaimTypes.Name);

            if (model.SelectedImages != null && model.SelectedImages.Count() > 6)
            {
                ModelState.AddModelError("", $"{userFirstName} عزیز تعداد تصاویر انتخابی برای محصول بیش از حد مجاز است.");

                return View(model);
            }

            if (model.ProductId == 0)
            {
                var product = new Product()
                {
                    ProductTitle = model.ProductTitle,
                    Description = model.Description,
                    ShortKey = NameGenerator.GenerateShortKey(),
                    ProductType = ProductTypes.Mobile.ToString(),
                    Brand = model.ProductBrand,
                    BrandProduct = model.BrandProduct,
                };
                
                var mobileDetail = new MobileDetail()
                {
                    InternalMemory = model.InternalMemory,
                    CommunicationNetworks = model.CommunicationNetworks,
                    BackCameras = model.BackCameras,
                    OperatingSystem = model.OperatingSystem,
                    SIMCardDescription = model.SIMCardDescription,
                    RAMValue = model.RAMValue,
                    PhotoResolution = model.PhotoResolution,
                    OperatingSystemVersion = model.OperatingSystemVersion,
                    DisplayTechnology = model.DisplayTechnology,
                    Features = model.Features,
                    Size = model.Size,
                    QuantitySIMCard = model.QuantitySIMCard
                };

                var result = await _productService.AddMobileAsync(product, mobileDetail);

                if (result == ResultTypes.Successful)
                {
                    if (model.SelectedImages is not null)
                    {
                        foreach (var image in model.SelectedImages)
                        {
                            if (image.Length > 0)
                            {
                                var imgName = NameGenerator.GenerateUniqCodeWithDash();

                                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot",
                                    "images",
                                    "ProductImages",
                                    imgName + Path.GetExtension(image.FileName));


                                await using var stream = new FileStream(filePath, FileMode.Create);
                                await image.CopyToAsync(stream);

                                var productGallery = new ProductGallery()
                                {
                                    ProductId = product.ProductId,
                                    ImageName = imgName + Path.GetExtension(image.FileName)
                                };

                                await _productService.AddProductGalleryAsync(productGallery);
                            }
                        }
                    }

                    var shopperProduct = new ShopperProduct()
                    {
                        ShopperUserId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString(),
                        ProductId = product.ProductId
                    };

                    await _shopperService.AddShopperProductAsync(shopperProduct);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                    return View(model);
                }
            }
            else
            {
                var product = await _productService.GetProductByIdAsync(model.ProductId);

                if (product?.MobileDetailId == null)
                    return NotFound();


                var mobileDetail = await _productService.GetMobileDetailByIdAsync(product.MobileDetailId.Value);

                if (mobileDetail == null)
                    return NotFound();



                //  update product
                product.ProductTitle = model.ProductTitle;
                product.Description = model.Description;
                product.Brand = model.ProductBrand;
                product.BrandProduct = model.BrandProduct;

                // update mobile cover detail
                mobileDetail.InternalMemory = model.InternalMemory;
                mobileDetail.CommunicationNetworks = model.CommunicationNetworks;
                mobileDetail.BackCameras = model.BackCameras;
                mobileDetail.OperatingSystem = model.OperatingSystem;
                mobileDetail.SIMCardDescription = model.SIMCardDescription;
                mobileDetail.RAMValue = model.RAMValue;
                mobileDetail.PhotoResolution = model.PhotoResolution;
                mobileDetail.OperatingSystemVersion = model.OperatingSystemVersion;
                mobileDetail.DisplayTechnology = model.DisplayTechnology;
                mobileDetail.Features = model.Features;
                mobileDetail.Size = model.Size;
                mobileDetail.QuantitySIMCard = model.QuantitySIMCard;



                var result = await _productService.EditMobileAsync(product, mobileDetail);

                if (result == ResultTypes.Successful)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                    return View(model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMobile(int productId)
        {
            if (!await _productService.IsProductExistAsync(productId))
                return NotFound();

            await _productService.RemoveMobileAsync(productId);

            return RedirectToAction("Index");
        }

        // جزعیات
        [HttpGet]
        public async Task<IActionResult> MobileDetail(int productId)
        {
            if (!await _productService.IsProductExistAsync(productId))
                return NotFound();

            return View(await _productService.GetProductDetailAsync(productId));
        }

        #endregion

        #region laptop

        [HttpGet]
        public async Task<IActionResult> AddOrEditLaptop(int productId = 0)
        {
            if (productId == 0)
            {
                return View();
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditLaptop(AddOrEditLaptopProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userFirstName = User.FindFirstValue(ClaimTypes.Actor);

            if (model.SelectedImages != null && model.SelectedImages.Count() > 6)
            {
                ModelState.AddModelError("", $"{userFirstName} عزیز تعداد تصاویر انتخابی برای محصول بیش از حد مجاز است.");

                return View(model);
            }

            if (model.ProductId == 0)
            {
                var product = new Product()
                {
                    ProductTitle = model.ProductTitle,
                    Description = model.Description,
                    ShortKey = NameGenerator.GenerateShortKey(),
                    ProductType = ProductTypes.Laptop.ToString(),
                    Brand = model.ProductBrand,
                    BrandProduct = model.BrandProduct,
                };

                var laptopDetail = new LaptopDetail()
                {
                    RAMCapacity = model.RAMCapacity,
                    InternalMemory = model.InternalMemory,
                    GPUManufacturer = model.GPUManufacturer,
                    Size = model.Size,
                    Category = model.Category,
                    ProcessorSeries = model.ProcessorSeries,
                    RAMType = model.RAMType,
                    ScreenAccuracy = model.ScreenAccuracy,
                    IsMatteScreen = model.IsMatteScreen,
                    IsTouchScreen = model.IsTouchScreen,
                    OperatingSystem = model.OperatingSystem,
                    IsHDMIPort = model.IsHDMIPort,
                };

                var result = await _productService.AddLaptopAsync(product, laptopDetail);

                if (result == ResultTypes.Successful)
                {
                    if (model.SelectedImages is not null)
                    {
                        foreach (var image in model.SelectedImages)
                        {
                            if (image.Length > 0)
                            {
                                var imgName = NameGenerator.GenerateUniqCodeWithDash();

                                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot",
                                    "images",
                                    imgName + Path.GetExtension(image.FileName));


                                await using var stream = new FileStream(filePath, FileMode.Create);
                                await image.CopyToAsync(stream);

                                var productGallery = new ProductGallery()
                                {
                                    ProductId = product.ProductId,
                                    ImageName = imgName + Path.GetExtension(image.FileName)
                                };

                                await _productService.AddProductGalleryAsync(productGallery);
                            }
                        }
                    }



                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

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
                
                product.Brand = model.ProductBrand;
                product.BrandProduct = model.BrandProduct;

                // update mobile cover detail
                laptopDetail.RAMCapacity = model.RAMCapacity;
                laptopDetail.InternalMemory = model.InternalMemory;
                laptopDetail.GPUManufacturer = model.GPUManufacturer;
                laptopDetail.Size = model.Size;
                laptopDetail.Category = model.Category;
                laptopDetail.ProcessorSeries = model.ProcessorSeries;
                laptopDetail.RAMType = model.RAMType;
                laptopDetail.ScreenAccuracy = model.ScreenAccuracy;
                laptopDetail.IsMatteScreen = model.IsMatteScreen;
                laptopDetail.IsTouchScreen = model.IsTouchScreen;
                laptopDetail.OperatingSystem = model.OperatingSystem;
                laptopDetail.IsHDMIPort = model.IsHDMIPort;

                var result = await _productService.EditLaptopAsync(product, laptopDetail);

                if (result == ResultTypes.Successful)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                    return View(model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLaptop(int productId)
        {
            if (!await _productService.IsProductExistAsync(productId))
                return NotFound();

            await _productService.RemoveLaptopAsync(productId);

            return RedirectToAction("Index");
        }

        #endregion

        #region tablet

        [HttpGet]
        public async Task<IActionResult> AddOrEditTablet(int productId)
        {
            if (productId == 0)
            {
                return View(new AddOrEditTabletViewModel());
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditTablet(AddOrEditTabletViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userFirstName = User.FindFirstValue(ClaimTypes.Actor);

            if (model.SelectedImages != null && model.SelectedImages.Count() > 6)
            {
                ModelState.AddModelError("", $"{userFirstName} عزیز تعداد تصاویر انتخابی برای محصول بیش از حد مجاز است.");

                return View(model);
            }

            if (model.ProductId == 0)
            {
                var product = new Product()
                {
                    ProductTitle = model.ProductTitle,
                    Description = model.Description,
                    ShortKey = NameGenerator.GenerateShortKey(),
                    ProductType = ProductTypes.Tablet.ToString(),
                    Brand = model.ProductBrand,
                    BrandProduct = model.BrandProduct,
                };

                var tabletDetail = new TabletDetail()
                {
                    InternalMemory = model.InternalMemory,
                    RAMValue = model.RAMValue,
                    IsTalkAbility = model.IsTalkAbility,
                    Size = model.Size,
                    CommunicationNetworks = model.CommunicationNetworks,
                    Features = model.Features,
                    IsSIMCardSupporter = model.IsSIMCardSupporter,
                    QuantitySIMCard = model.QuantitySIMCard,
                    OperatingSystemVersion = model.OperatingSystemVersion,
                    CommunicationTechnologies = model.CommunicationTechnologies,
                    CommunicationPorts = model.CommunicationPorts
                };


                var result = await _productService.AddTabletAsync(product, tabletDetail);

                if (result == ResultTypes.Successful)
                {
                    if (model.SelectedImages is not null)
                    {
                        foreach (var image in model.SelectedImages)
                        {
                            if (image.Length > 0)
                            {
                                var imgName = NameGenerator.GenerateUniqCodeWithDash();

                                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot",
                                    "images",
                                    imgName + Path.GetExtension(image.FileName));


                                await using var stream = new FileStream(filePath, FileMode.Create);
                                await image.CopyToAsync(stream);

                                var productGallery = new ProductGallery()
                                {
                                    ProductId = product.ProductId,
                                    ImageName = imgName + Path.GetExtension(image.FileName)
                                };

                                await _productService.AddProductGalleryAsync(productGallery);
                            }
                        }
                    }



                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است. لطفا دوباره تلاش کنید!");

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
                product.Brand = model.ProductBrand;
                product.BrandProduct = model.BrandProduct;

                // update mobile cover detail
                tabletDetail.InternalMemory = model.InternalMemory;
                tabletDetail.RAMValue = model.RAMValue;
                tabletDetail.IsTalkAbility = model.IsTalkAbility;
                tabletDetail.Size = model.Size;
                tabletDetail.CommunicationNetworks = model.CommunicationNetworks;
                tabletDetail.Features = model.Features;
                tabletDetail.IsSIMCardSupporter = model.IsSIMCardSupporter;
                tabletDetail.QuantitySIMCard = model.QuantitySIMCard;
                tabletDetail.OperatingSystemVersion = model.OperatingSystemVersion;
                tabletDetail.CommunicationTechnologies = model.CommunicationTechnologies;
                tabletDetail.CommunicationPorts = model.CommunicationPorts;



                var result = await _productService.EditTabletAsync(product, tabletDetail);

                if (result == ResultTypes.Successful)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است. لطفا دوباره تلاش کنید!");

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
                return View(new AddOrEditMobileCoverViewModel());
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditMobileCover(AddOrEditMobileCoverViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userFirstName = User.FindFirstValue(ClaimTypes.Actor);

            if (model.SelectedImages != null && model.SelectedImages.Count() > 6)
            {
                ModelState.AddModelError("", $"{userFirstName} عزیز تعداد تصاویر انتخابی برای محصول بیش از حد مجاز است.");

                return View(model);
            }

            if (model.ProductId == 0)
            {
                var product = new Product()
                {
                    ProductTitle = model.ProductTitle,
                    Description = model.Description,
                    ShortKey = NameGenerator.GenerateShortKey(),
                    ProductType = ProductTypes.MobileCover.ToString(),
                    Brand = model.ProductBrand,
                    BrandProduct = model.BrandProduct,
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
                    if (model.SelectedImages is not null)
                    {
                        foreach (var image in model.SelectedImages)
                        {
                            if (image.Length > 0)
                            {
                                var imgName = NameGenerator.GenerateUniqCodeWithDash();

                                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot",
                                    "images",
                                    imgName + Path.GetExtension(image.FileName));


                                await using var stream = new FileStream(filePath, FileMode.Create);
                                await image.CopyToAsync(stream);

                                var productGallery = new ProductGallery()
                                {
                                    ProductId = product.ProductId,
                                    ImageName = imgName + Path.GetExtension(image.FileName)
                                };

                                await _productService.AddProductGalleryAsync(productGallery);
                            }
                        }
                    }



                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است. لطفا دوباره تلاش کنید!");

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
                product.Brand = model.ProductBrand;
                product.BrandProduct = model.BrandProduct;

                // update mobile cover detail
                mobileCoverDetail.SuitablePhones = model.SuitablePhones;
                mobileCoverDetail.Gender = model.Gender;
                mobileCoverDetail.Structure = model.Structure;
                mobileCoverDetail.CoverLevel = model.CoverLevel;
                mobileCoverDetail.Features = model.Features;


                var result = await _productService.EditMobileCoverAsync(product, mobileCoverDetail);

                if (result == ResultTypes.Successful)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است. لطفا دوباره تلاش کنید!");

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
                return View();
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

        #region headphone and handsfree

        [HttpGet]
        public async Task<IActionResult> AddOrEditHeadPhone(int productId = 0)
        {
            if (productId == 0)
            {
                return View();
            }
            else
            {
                var product = await _productService.GetTypeHandsfreeAndHeadPhoneProductDataAsync(productId);
                if (product == null)
                    return NotFound();

                return View(product);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditHeadPhone(AddOrEditHandsfreeAndHeadPhoneViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userFirstName = User.FindFirstValue(ClaimTypes.Actor);

            if (model.SelectedImages != null && model.SelectedImages.Count() > 6)
            {
                ModelState.AddModelError("", $"{userFirstName} عزیز تعداد تصاویر انتخابی برای محصول بیش از حد مجاز است.");

                return View(model);
            }

            if (model.ProductId == 0)
            {
                var product = new Product()
                {
                    ProductTitle = model.ProductTitle,
                    Description = model.Description,
                    ShortKey = NameGenerator.GenerateShortKey(),
                    ProductType = ProductTypes.HeadPhone.ToString(),
                    Brand = model.ProductBrand,
                    BrandProduct = model.BrandProduct,
                };

                var hansAndHeadPhoneDetail = new HandsfreeAndHeadPhoneDetail()
                {
                    ConnectionType = model.ConnectionType,
                    PhoneType = model.PhoneType,
                    WorkSuggestion = model.WorkSuggestion,
                    Connector = model.Connector,
                    IsSupportBattery = model.IsSupportBattery,
                    Features = model.Features
                };


                var result = await _productService.AddHandsfreeAndHeadPhoneDetailAsync(product, hansAndHeadPhoneDetail);

                if (result == ResultTypes.Successful)
                {
                    if (model.SelectedImages is not null)
                    {
                        foreach (var image in model.SelectedImages)
                        {
                            if (image.Length > 0)
                            {
                                var imgName = NameGenerator.GenerateUniqCodeWithDash();

                                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot",
                                    "images",
                                    imgName + Path.GetExtension(image.FileName));


                                await using var stream = new FileStream(filePath, FileMode.Create);
                                await image.CopyToAsync(stream);

                                var productGallery = new ProductGallery()
                                {
                                    ProductId = product.ProductId,
                                    ImageName = imgName + Path.GetExtension(image.FileName)
                                };

                                await _productService.AddProductGalleryAsync(productGallery);
                            }
                        }
                    }



                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است. لطفا دوباره تلاش کنید!");

                    return View(model);
                }
            }
            else
            {
                var product = await _productService.GetProductByIdAsync(model.ProductId);

                if (product?.MobileCoverDetailId == null)
                    return NotFound();


                var handsfreeAndHeadPhoneDetail = await _productService.GetHandsfreeAndHeadPhoneDetailByIdAsync(product.MobileCoverDetailId.Value);

                if (handsfreeAndHeadPhoneDetail == null)
                    return NotFound();



                //  update product
                product.ProductTitle = model.ProductTitle;
                product.Description = model.Description;
                product.Brand = model.ProductBrand;
                product.BrandProduct = model.BrandProduct;

                // update mobile cover detail
                handsfreeAndHeadPhoneDetail.ConnectionType = model.ConnectionType;
                handsfreeAndHeadPhoneDetail.PhoneType = model.PhoneType;
                handsfreeAndHeadPhoneDetail.WorkSuggestion = model.WorkSuggestion;
                handsfreeAndHeadPhoneDetail.Connector = model.Connector;
                handsfreeAndHeadPhoneDetail.IsSupportBattery = model.IsSupportBattery;
                handsfreeAndHeadPhoneDetail.Features = model.Features;



                var result = await _productService.EditHandsfreeAndHeadPhoneDetailAsync(product, handsfreeAndHeadPhoneDetail);

                if (result == ResultTypes.Successful)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است. لطفا دوباره تلاش کنید!");

                    return View(model);
                }

            }
        }


        [HttpGet]
        public async Task<IActionResult> AddOrEditHandsfree(int productId = 0)
        {
            if (productId == 0)
            {
                return View();
            }
            else
            {
                var product = await _productService.GetTypeHandsfreeAndHeadPhoneProductDataAsync(productId);
                if (product == null)
                    return NotFound();

                return View(product);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditHandsfree(AddOrEditHandsfreeAndHeadPhoneViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userFirstName = User.FindFirstValue(ClaimTypes.Actor);

            if (model.SelectedImages != null && model.SelectedImages.Count() > 6)
            {
                ModelState.AddModelError("", $"{userFirstName} عزیز تعداد تصاویر انتخابی برای محصول بیش از حد مجاز است.");

                return View(model);
            }

            if (model.ProductId == 0)
            {
                var product = new Product()
                {
                    ProductTitle = model.ProductTitle,
                    Description = model.Description,
                    ShortKey = NameGenerator.GenerateShortKey(),
                    ProductType = ProductTypes.Handsfree.ToString(),
                    Brand = model.ProductBrand,
                    BrandProduct = model.BrandProduct,
                };

                var hansAndHeadPhoneDetail = new HandsfreeAndHeadPhoneDetail()
                {
                    ConnectionType = model.ConnectionType,
                    PhoneType = model.PhoneType,
                    WorkSuggestion = model.WorkSuggestion,
                    Connector = model.Connector,
                    IsSupportBattery = model.IsSupportBattery,
                    Features = model.Features
                };


                var result = await _productService.AddHandsfreeAndHeadPhoneDetailAsync(product, hansAndHeadPhoneDetail);

                if (result == ResultTypes.Successful)
                {
                    if (model.SelectedImages is not null)
                    {
                        foreach (var image in model.SelectedImages)
                        {
                            if (image.Length > 0)
                            {
                                var imgName = NameGenerator.GenerateUniqCodeWithDash();

                                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot",
                                    "images",
                                    imgName + Path.GetExtension(image.FileName));


                                await using var stream = new FileStream(filePath, FileMode.Create);
                                await image.CopyToAsync(stream);

                                var productGallery = new ProductGallery()
                                {
                                    ProductId = product.ProductId,
                                    ImageName = imgName + Path.GetExtension(image.FileName)
                                };

                                await _productService.AddProductGalleryAsync(productGallery);
                            }
                        }
                    }



                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است. لطفا دوباره تلاش کنید!");

                    return View(model);
                }
            }
            else
            {
                var product = await _productService.GetProductByIdAsync(model.ProductId);

                if (product?.MobileCoverDetailId == null)
                    return NotFound();


                var handsfreeAndHeadPhoneDetail = await _productService.GetHandsfreeAndHeadPhoneDetailByIdAsync(product.MobileCoverDetailId.Value);

                if (handsfreeAndHeadPhoneDetail == null)
                    return NotFound();



                //  update product
                product.ProductTitle = model.ProductTitle;
                product.Description = model.Description;
                product.Brand = model.ProductBrand;
                product.BrandProduct = model.BrandProduct;

                // update mobile cover detail
                handsfreeAndHeadPhoneDetail.ConnectionType = model.ConnectionType;
                handsfreeAndHeadPhoneDetail.PhoneType = model.PhoneType;
                handsfreeAndHeadPhoneDetail.WorkSuggestion = model.WorkSuggestion;
                handsfreeAndHeadPhoneDetail.Connector = model.Connector;
                handsfreeAndHeadPhoneDetail.IsSupportBattery = model.IsSupportBattery;
                handsfreeAndHeadPhoneDetail.Features = model.Features;



                var result = await _productService.EditHandsfreeAndHeadPhoneDetailAsync(product, handsfreeAndHeadPhoneDetail);

                if (result == ResultTypes.Successful)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است. لطفا دوباره تلاش کنید!");

                    return View(model);
                }

            }
        }


        #endregion

        #region speaker

        [HttpGet]
        public async Task<IActionResult> AddOrEditSpeaker(int productId = 0)
        {
            if (productId == 0)
            {
                return View(new AddOrEditSpeakerViewModel());
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditSpeaker(AddOrEditSpeakerViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userFirstName = User.FindFirstValue(ClaimTypes.Actor);

            if (model.SelectedImages != null && model.SelectedImages.Count() > 6)
            {
                ModelState.AddModelError("", $"{userFirstName} عزیز تعداد تصاویر انتخابی برای محصول بیش از حد مجاز است.");

                return View(model);
            }

            if (model.ProductId == 0)
            {
                var product = new Product()
                {
                    ProductTitle = model.ProductTitle,
                    Description = model.Description,
                    ShortKey = NameGenerator.GenerateShortKey(),
                    ProductType = ProductTypes.Speaker.ToString(),
                    Brand = model.ProductBrand,
                    BrandProduct = model.BrandProduct,
                };

                var speakerDetail = new SpeakerDetail()
                {
                    ConnectionType = model.ConnectionType,
                    Connector = model.Connector,
                    BluetoothVersion = model.BluetoothVersion,
                    IsMemoryCardInput = model.IsMemoryCardInput,
                    IsSupportBattery = model.IsSupportBattery,
                    IsSupportMicrophone = model.IsSupportMicrophone,
                    IsSupportUSBPort = model.IsSupportUSBPort,
                    IsSupportRadio = model.IsSupportRadio,
                };


                var result = await _productService.AddSpeakerAsync(product, speakerDetail);

                if (result == ResultTypes.Successful)
                {
                    if (model.SelectedImages is not null)
                    {
                        foreach (var image in model.SelectedImages)
                        {
                            if (image.Length > 0)
                            {
                                var imgName = NameGenerator.GenerateUniqCodeWithDash();

                                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot",
                                    "images",
                                    imgName + Path.GetExtension(image.FileName));


                                await using var stream = new FileStream(filePath, FileMode.Create);
                                await image.CopyToAsync(stream);

                                var productGallery = new ProductGallery()
                                {
                                    ProductId = product.ProductId,
                                    ImageName = imgName + Path.GetExtension(image.FileName)
                                };

                                await _productService.AddProductGalleryAsync(productGallery);
                            }
                        }
                    }



                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

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
                product.Brand = model.ProductBrand;
                product.BrandProduct = model.BrandProduct;

                // update mobile cover detail
                speakerDetail.ConnectionType = model.ConnectionType;
                speakerDetail.Connector = model.Connector;
                speakerDetail.BluetoothVersion = model.BluetoothVersion;
                speakerDetail.IsMemoryCardInput = model.IsMemoryCardInput;
                speakerDetail.IsSupportBattery = model.IsSupportBattery;
                speakerDetail.IsSupportMicrophone = model.IsSupportMicrophone;
                speakerDetail.IsSupportUSBPort = model.IsSupportUSBPort;
                speakerDetail.IsSupportRadio = model.IsSupportRadio;



                var result = await _productService.EditSpeakerAsync(product, speakerDetail);

                if (result == ResultTypes.Successful)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

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
                return View();
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

        #region wrist watch

        [HttpGet]
        public async Task<IActionResult> AddOrEditWristWatch(int productId = 0)
        {
            if (productId == 0)
            {
                return View();
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditWristWatch(AddOrEdirWristWatchViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userFirstName = User.FindFirstValue(ClaimTypes.Actor);

            if (model.SelectedImages != null && model.SelectedImages.Count() > 6)
            {
                ModelState.AddModelError("", $"{userFirstName} عزیز تعداد تصاویر انتخابی برای محصول بیش از حد مجاز است.");

                return View(model);
            }

            if (model.ProductId == 0)
            {
                var product = new Product()
                {
                    ProductTitle = model.ProductTitle,
                    Description = model.Description,
                    ShortKey = NameGenerator.GenerateShortKey(),
                    ProductType = ProductTypes.WristWatch.ToString(),
                    Brand = model.ProductBrand,
                    BrandProduct = model.BrandProduct,
                };

                var wristWatchDetail = new WristWatchDetail()
                {
                    IsSupportGPS = model.IsSupportGPS,
                    IsTouchScreen = model.IsTouchScreen,
                    WatchForm = model.WatchForm
                };


                var result = await _productService.AddWristWatchAsync(product, wristWatchDetail);

                if (result == ResultTypes.Successful)
                {
                    if (model.SelectedImages is not null)
                    {
                        foreach (var image in model.SelectedImages)
                        {
                            if (image.Length > 0)
                            {
                                var imgName = NameGenerator.GenerateUniqCodeWithDash();

                                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot",
                                    "images",
                                    imgName + Path.GetExtension(image.FileName));


                                await using var stream = new FileStream(filePath, FileMode.Create);
                                await image.CopyToAsync(stream);

                                var productGallery = new ProductGallery()
                                {
                                    ProductId = product.ProductId,
                                    ImageName = imgName + Path.GetExtension(image.FileName)
                                };

                                await _productService.AddProductGalleryAsync(productGallery);
                            }
                        }
                    }



                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

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
                product.Brand = model.ProductBrand;
                product.BrandProduct = model.BrandProduct;

                // update mobile cover detail
                wristWatchDetail.IsSupportGPS = model.IsSupportGPS;
                wristWatchDetail.IsTouchScreen = model.IsTouchScreen;
                wristWatchDetail.WatchForm = model.WatchForm;



                var result = await _productService.EditWristWatchAsync(product, wristWatchDetail);

                if (result == ResultTypes.Successful)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

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
                return View();
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditSmartWatch(AddOrEditSmartWatchViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userFirstName = User.FindFirstValue(ClaimTypes.Actor);

            if (model.SelectedImages != null && model.SelectedImages.Count() > 6)
            {
                ModelState.AddModelError("", $"{userFirstName} عزیز تعداد تصاویر انتخابی برای محصول بیش از حد مجاز است.");

                return View(model);
            }

            if (model.ProductId == 0)
            {
                var product = new Product()
                {
                    ProductTitle = model.ProductTitle,
                    Description = model.Description,
                    ShortKey = NameGenerator.GenerateShortKey(),
                    ProductType = ProductTypes.SmartWatch.ToString(),
                    Brand = model.ProductBrand,
                    BrandProduct = model.BrandProduct,
                };

                var smartWatchDetail = new SmartWatchDetail()
                {
                    IsSuitableForMen = model.IsSuitableForMen,
                    IsSuitableForWomen = model.IsSuitableForWomen,
                    IsScreenColorful = model.IsScreenColorful,
                    IsSIMCardSupporter = model.IsSIMCardSupporter,
                    IsTouchScreen = model.IsTouchScreen,
                    IsSupportSIMCardRegister = model.IsSupportSIMCardRegister,
                    WorkSuggestion = model.WorkSuggestion,
                    IsSupportGPS = model.IsSupportGPS,
                    WatchForm = model.WatchForm,
                    BodyMaterial = model.BodyMaterial,
                    Connections = model.Connections,
                    Sensors = model.Sensors,
                    IsDirectTalkable = model.IsDirectTalkable,
                    IsTalkableWithBluetooth = model.IsTalkableWithBluetooth
                };


                var result = await _productService.AddSmartWatchAsync(product, smartWatchDetail);

                if (result == ResultTypes.Successful)
                {
                    if (model.SelectedImages is not null)
                    {
                        foreach (var image in model.SelectedImages)
                        {
                            if (image.Length > 0)
                            {
                                var imgName = NameGenerator.GenerateUniqCodeWithDash();

                                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot",
                                    "images",
                                    imgName + Path.GetExtension(image.FileName));


                                await using var stream = new FileStream(filePath, FileMode.Create);
                                await image.CopyToAsync(stream);

                                var productGallery = new ProductGallery()
                                {
                                    ProductId = product.ProductId,
                                    ImageName = imgName + Path.GetExtension(image.FileName)
                                };

                                await _productService.AddProductGalleryAsync(productGallery);
                            }
                        }
                    }



                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

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
                product.Brand = model.ProductBrand;
                product.BrandProduct = model.BrandProduct;

                // update mobile cover detail
                smartWatchDetail.IsSuitableForMen = model.IsSuitableForMen;
                smartWatchDetail.IsSuitableForWomen = model.IsSuitableForWomen;
                smartWatchDetail.IsScreenColorful = model.IsScreenColorful;
                smartWatchDetail.IsSIMCardSupporter = model.IsSIMCardSupporter;
                smartWatchDetail.IsTouchScreen = model.IsTouchScreen;
                smartWatchDetail.IsSupportSIMCardRegister = model.IsSupportSIMCardRegister;
                smartWatchDetail.WorkSuggestion = model.WorkSuggestion;
                smartWatchDetail.IsSupportGPS = model.IsSupportGPS;
                smartWatchDetail.WatchForm = model.WatchForm;
                smartWatchDetail.BodyMaterial = model.BodyMaterial;
                smartWatchDetail.Connections = model.Connections;
                smartWatchDetail.Sensors = model.Sensors;
                smartWatchDetail.IsDirectTalkable = model.IsDirectTalkable;
                smartWatchDetail.IsTalkableWithBluetooth = model.IsTalkableWithBluetooth;

                var result = await _productService.EditSmartWatchAsync(product, smartWatchDetail);

                if (result == ResultTypes.Successful)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

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
                return View(new AddOrEditFlashMemoryViewModel());
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditFlashMemory(AddOrEditFlashMemoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userFirstName = User.FindFirstValue(ClaimTypes.Actor);

            if (model.SelectedImages != null && model.SelectedImages.Count() > 6)
            {
                ModelState.AddModelError("", $"{userFirstName} عزیز تعداد تصاویر انتخابی برای محصول بیش از حد مجاز است.");

                return View(model);
            }

            if (model.ProductId == 0)
            {
                var product = new Product()
                {
                    ProductTitle = model.ProductTitle,
                    Description = model.Description,
                    ShortKey = NameGenerator.GenerateShortKey(),
                    ProductType = ProductTypes.FlashMemory.ToString(),
                    Brand = model.ProductBrand,
                    BrandProduct = model.BrandProduct,
                };

                var flashMemoryDetail = new FlashMemoryDetail()
                {
                    Connector = model.Connector,
                    Capacity = model.Capacity,
                    IsImpactResistance = model.IsImpactResistance
                };


                var result = await _productService.AddFlashMemoryAsync(product, flashMemoryDetail);

                if (result == ResultTypes.Successful)
                {
                    if (model.SelectedImages is not null)
                    {
                        foreach (var image in model.SelectedImages)
                        {
                            if (image.Length > 0)
                            {
                                var imgName = NameGenerator.GenerateUniqCodeWithDash();

                                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot",
                                    "images",
                                    imgName + Path.GetExtension(image.FileName));


                                await using var stream = new FileStream(filePath, FileMode.Create);
                                await image.CopyToAsync(stream);

                                var productGallery = new ProductGallery()
                                {
                                    ProductId = product.ProductId,
                                    ImageName = imgName + Path.GetExtension(image.FileName)
                                };

                                await _productService.AddProductGalleryAsync(productGallery);
                            }
                        }
                    }



                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

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
                product.Brand = model.ProductBrand;
                product.BrandProduct = model.BrandProduct;

                // update mobile cover detail
                flashMemoryDetail.Connector = model.Connector;
                flashMemoryDetail.Capacity = model.Capacity;
                flashMemoryDetail.IsImpactResistance = model.IsImpactResistance;



                var result = await _productService.EditFlashMemoryAsync(product, flashMemoryDetail);

                if (result == ResultTypes.Successful)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

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
                return View(new AddOrEditFlashMemoryViewModel());
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditMemoryCard(AddOrEditMemoryCardViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userFirstName = User.FindFirstValue(ClaimTypes.Actor);

            if (model.SelectedImages != null && model.SelectedImages.Count() > 6)
            {
                ModelState.AddModelError("", $"{userFirstName} عزیز تعداد تصاویر انتخابی برای محصول بیش از حد مجاز است.");

                return View(model);
            }

            if (model.ProductId == 0)
            {
                var product = new Product()
                {
                    ProductTitle = model.ProductTitle,
                    Description = model.Description,
                    ShortKey = NameGenerator.GenerateShortKey(),
                    ProductType = ProductTypes.FlashMemory.ToString(),
                    Brand = model.ProductBrand,
                    BrandProduct = model.BrandProduct,
                };

                var memoryCardDetail = new MemoryCardDetail()
                {
                    Capacity = model.Capacity,
                    Size = model.Size,
                    SpeedStandard = model.SpeedStandard,
                    ResistsAgainst = model.ResistsAgainst
                };


                var result = await _productService.AddMemoryCardAsync(product, memoryCardDetail);

                if (result == ResultTypes.Successful)
                {
                    if (model.SelectedImages is not null)
                    {
                        foreach (var image in model.SelectedImages)
                        {
                            if (image.Length > 0)
                            {
                                var imgName = NameGenerator.GenerateUniqCodeWithDash();

                                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot",
                                    "images",
                                    imgName + Path.GetExtension(image.FileName));


                                await using var stream = new FileStream(filePath, FileMode.Create);
                                await image.CopyToAsync(stream);

                                var productGallery = new ProductGallery()
                                {
                                    ProductId = product.ProductId,
                                    ImageName = imgName + Path.GetExtension(image.FileName)
                                };

                                await _productService.AddProductGalleryAsync(productGallery);
                            }
                        }
                    }



                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ثبت محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

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
                product.Brand = model.ProductBrand;
                product.BrandProduct = model.BrandProduct;

                // update mobile cover detail
                memoryCardDetail.Capacity = model.Capacity;
                memoryCardDetail.Size = model.Size;
                memoryCardDetail.SpeedStandard = model.SpeedStandard;
                memoryCardDetail.ResistsAgainst = model.ResistsAgainst;



                var result = await _productService.EditMemoryCardAsync(product, memoryCardDetail);

                if (result == ResultTypes.Successful)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", $"{userFirstName} عزیز متاسفانه خطایی هنگام ویرایش محصول به وجود آمده است! لطفا با پشتیبانی تماس بگیرید.");

                    return View(model);
                }
            }
        }

        #endregion
    }
}
