using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BatteryChargerDetails",
                columns: table => new
                {
                    BatteryChargerDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutputCurrentIntensity = table.Column<double>(type: "float", nullable: false),
                    OutputPortsCount = table.Column<byte>(type: "tinyint", nullable: false),
                    MobileCable = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatteryChargerDetails", x => x.BatteryChargerDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ChildCategories",
                columns: table => new
                {
                    ChildCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildCategoryTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildCategories", x => x.ChildCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    DiscountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DiscountPercent = table.Column<byte>(type: "tinyint", nullable: false),
                    UsableCount = table.Column<short>(type: "smallint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.DiscountId);
                });

            migrationBuilder.CreateTable(
                name: "FlashMemoryDetails",
                columns: table => new
                {
                    FlashDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Connector = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<double>(type: "float", nullable: false),
                    IsImpactResistance = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashMemoryDetails", x => x.FlashDetailId);
                });

            migrationBuilder.CreateTable(
                name: "HandsfreeAndHeadPhoneDetails",
                columns: table => new
                {
                    HeadPhoneDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WorkSuggestion = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Connector = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsSupportBattery = table.Column<bool>(type: "bit", nullable: false),
                    Features = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandsfreeAndHeadPhoneDetails", x => x.HeadPhoneDetailId);
                });

            migrationBuilder.CreateTable(
                name: "LaptopDetails",
                columns: table => new
                {
                    LaptopDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RAMCapacity = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    InternalMemory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GPUManufacturer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Size = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ProcessorSeries = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RAMType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ScreenAccuracy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsMatteScreen = table.Column<bool>(type: "bit", nullable: false),
                    IsTouchScreen = table.Column<bool>(type: "bit", nullable: false),
                    OperatingSystem = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsHDMIPort = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaptopDetails", x => x.LaptopDetailId);
                });

            migrationBuilder.CreateTable(
                name: "MemoryCardDetails",
                columns: table => new
                {
                    MemoryCardDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Capacity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Size = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SpeedStandard = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResistsAgainst = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryCardDetails", x => x.MemoryCardDetailId);
                });

            migrationBuilder.CreateTable(
                name: "MobileCoverDetails",
                columns: table => new
                {
                    MobileCoverDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuitablePhones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Structure = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CoverLevel = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Features = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileCoverDetails", x => x.MobileCoverDetailId);
                });

            migrationBuilder.CreateTable(
                name: "MobileDetails",
                columns: table => new
                {
                    MobileDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InternalMemory = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CommunicationNetworks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BackCameras = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OperatingSystem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SIMCardDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RAMValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhotoResolution = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OperatingSystemVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayTechnology = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Features = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Size = table.Column<double>(type: "float", nullable: false),
                    QuantitySIMCard = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileDetails", x => x.MobileDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionTitle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                    table.ForeignKey(
                        name: "FK_Permissions_Permissions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PowerBankDetails",
                columns: table => new
                {
                    PowerBankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CapacityRange = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OutputCurrentIntensity = table.Column<double>(type: "float", nullable: false),
                    OutputPortsCount = table.Column<byte>(type: "tinyint", nullable: false),
                    IsSupportOfQCTechnology = table.Column<bool>(type: "bit", maxLength: 200, nullable: false),
                    IsSupportOfPDTechnology = table.Column<bool>(type: "bit", maxLength: 200, nullable: false),
                    Features = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BodyMaterial = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerBankDetails", x => x.PowerBankId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Shoppers",
                columns: table => new
                {
                    ShopperId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisterShopper = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LandlinePhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    OnNationalCardImageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BackNationalCardImageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BusinessLicenseImageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Condition = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shoppers", x => x.ShopperId);
                });

            migrationBuilder.CreateTable(
                name: "SmartWatchDetails",
                columns: table => new
                {
                    SmartWatchDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsSuitableForMen = table.Column<bool>(type: "bit", nullable: false),
                    IsSuitableForWomen = table.Column<bool>(type: "bit", nullable: false),
                    IsScreenColorful = table.Column<bool>(type: "bit", nullable: false),
                    IsSIMCardSupporter = table.Column<bool>(type: "bit", nullable: false),
                    IsTouchScreen = table.Column<bool>(type: "bit", nullable: false),
                    IsSupportSIMCardRegister = table.Column<bool>(type: "bit", nullable: false),
                    WorkSuggestion = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    IsSupportGPS = table.Column<bool>(type: "bit", nullable: false),
                    WatchForm = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BodyMaterial = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Connections = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sensors = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDirectTalkable = table.Column<bool>(type: "bit", nullable: false),
                    IsTalkableWithBluetooth = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartWatchDetails", x => x.SmartWatchDetailId);
                });

            migrationBuilder.CreateTable(
                name: "SpeakerDetails",
                columns: table => new
                {
                    SpeakerDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Connector = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BluetoothVersion = table.Column<double>(type: "float", nullable: false),
                    IsMemoryCardInput = table.Column<bool>(type: "bit", nullable: false),
                    IsSupportBattery = table.Column<bool>(type: "bit", nullable: false),
                    IsSupportUSBPort = table.Column<bool>(type: "bit", nullable: false),
                    IsSupportMicrophone = table.Column<bool>(type: "bit", nullable: false),
                    IsSupportRadio = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeakerDetails", x => x.SpeakerDetailId);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    StateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.StateId);
                });

            migrationBuilder.CreateTable(
                name: "StoreTitles",
                columns: table => new
                {
                    StoreTitleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreTitleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTitles", x => x.StoreTitleId);
                });

            migrationBuilder.CreateTable(
                name: "TabletDetails",
                columns: table => new
                {
                    TabletDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InternalMemory = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RAMValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsTalkAbility = table.Column<bool>(type: "bit", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CommunicationNetworks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Features = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsSIMCardSupporter = table.Column<bool>(type: "bit", nullable: false),
                    QuantitySIMCard = table.Column<byte>(type: "tinyint", maxLength: 10, nullable: false),
                    OperatingSystemVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CommunicationTechnologies = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CommunicationPorts = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabletDetails", x => x.TabletDetailId);
                });

            migrationBuilder.CreateTable(
                name: "WalletTypes",
                columns: table => new
                {
                    WalletTypeId = table.Column<int>(type: "int", nullable: false),
                    TypeTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTypes", x => x.WalletTypeId);
                });

            migrationBuilder.CreateTable(
                name: "WristWatchDetails",
                columns: table => new
                {
                    WristWatchDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsSupportGPS = table.Column<bool>(type: "bit", nullable: false),
                    IsTouchScreen = table.Column<bool>(type: "bit", nullable: false),
                    WatchForm = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WristWatchDetails", x => x.WristWatchDetailId);
                });

            migrationBuilder.CreateTable(
                name: "BrandProducts",
                columns: table => new
                {
                    BrandProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandProducts", x => x.BrandProductId);
                    table.ForeignKey(
                        name: "FK_BrandProducts_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChildCategoryToCategories",
                columns: table => new
                {
                    ChildCategoryId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildCategoryToCategories", x => new { x.CategoryId, x.ChildCategoryId });
                    table.ForeignKey(
                        name: "FK_ChildCategoryToCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildCategoryToCategories_ChildCategories_ChildCategoryId",
                        column: x => x.ChildCategoryId,
                        principalTable: "ChildCategories",
                        principalColumn: "ChildCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserAvatar = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    InviteCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InviteCount = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    ActiveCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AccountBalance = table.Column<decimal>(type: "Money", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RegisteredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPhoneNumberActive = table.Column<bool>(type: "bit", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    IsUserShopper = table.Column<bool>(type: "bit", nullable: false),
                    ShopperId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Shoppers_ShopperId",
                        column: x => x.ShopperId,
                        principalTable: "Shoppers",
                        principalColumn: "ShopperId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StateCities",
                columns: table => new
                {
                    StateId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateCities", x => new { x.StateId, x.CityId });
                    table.ForeignKey(
                        name: "FK_StateCities_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StateCities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopperStoreTitles",
                columns: table => new
                {
                    ShopperId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreTitleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopperStoreTitles", x => new { x.ShopperId, x.StoreTitleId });
                    table.ForeignKey(
                        name: "FK_ShopperStoreTitles_Shoppers_ShopperId",
                        column: x => x.ShopperId,
                        principalTable: "Shoppers",
                        principalColumn: "ShopperId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopperStoreTitles_StoreTitles_StoreTitleId",
                        column: x => x.StoreTitleId,
                        principalTable: "StoreTitles",
                        principalColumn: "StoreTitleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ShortKey = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    ProductType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BrandProduct = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AllViewsCount = table.Column<int>(type: "int", nullable: false),
                    AllSalesCount = table.Column<int>(type: "int", nullable: false),
                    MobileDetailId = table.Column<int>(type: "int", nullable: true),
                    LaptopDetailId = table.Column<int>(type: "int", nullable: true),
                    MobileCoverDetailId = table.Column<int>(type: "int", nullable: true),
                    TabletDetailId = table.Column<int>(type: "int", nullable: true),
                    SpeakerDetailId = table.Column<int>(type: "int", nullable: true),
                    PowerBankDetailId = table.Column<int>(type: "int", nullable: true),
                    WristWatchDetailId = table.Column<int>(type: "int", nullable: true),
                    SmartWatchDetailId = table.Column<int>(type: "int", nullable: true),
                    HandsfreeAndHeadPhoneDetailId = table.Column<int>(type: "int", nullable: true),
                    FlashMemoryDetailId = table.Column<int>(type: "int", nullable: true),
                    BatteryChargerDetailId = table.Column<int>(type: "int", nullable: true),
                    MemoryCardDetailId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_BatteryChargerDetails_BatteryChargerDetailId",
                        column: x => x.BatteryChargerDetailId,
                        principalTable: "BatteryChargerDetails",
                        principalColumn: "BatteryChargerDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_FlashMemoryDetails_FlashMemoryDetailId",
                        column: x => x.FlashMemoryDetailId,
                        principalTable: "FlashMemoryDetails",
                        principalColumn: "FlashDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_HandsfreeAndHeadPhoneDetails_HandsfreeAndHeadPhoneDetailId",
                        column: x => x.HandsfreeAndHeadPhoneDetailId,
                        principalTable: "HandsfreeAndHeadPhoneDetails",
                        principalColumn: "HeadPhoneDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_LaptopDetails_LaptopDetailId",
                        column: x => x.LaptopDetailId,
                        principalTable: "LaptopDetails",
                        principalColumn: "LaptopDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_MemoryCardDetails_MemoryCardDetailId",
                        column: x => x.MemoryCardDetailId,
                        principalTable: "MemoryCardDetails",
                        principalColumn: "MemoryCardDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_MobileCoverDetails_MobileCoverDetailId",
                        column: x => x.MobileCoverDetailId,
                        principalTable: "MobileCoverDetails",
                        principalColumn: "MobileCoverDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_MobileDetails_MobileDetailId",
                        column: x => x.MobileDetailId,
                        principalTable: "MobileDetails",
                        principalColumn: "MobileDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_PowerBankDetails_PowerBankDetailId",
                        column: x => x.PowerBankDetailId,
                        principalTable: "PowerBankDetails",
                        principalColumn: "PowerBankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_SmartWatchDetails_SmartWatchDetailId",
                        column: x => x.SmartWatchDetailId,
                        principalTable: "SmartWatchDetails",
                        principalColumn: "SmartWatchDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_SpeakerDetails_SpeakerDetailId",
                        column: x => x.SpeakerDetailId,
                        principalTable: "SpeakerDetails",
                        principalColumn: "SpeakerDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_TabletDetails_TabletDetailId",
                        column: x => x.TabletDetailId,
                        principalTable: "TabletDetails",
                        principalColumn: "TabletDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_WristWatchDetails_WristWatchDetailId",
                        column: x => x.WristWatchDetailId,
                        principalTable: "WristWatchDetails",
                        principalColumn: "WristWatchDetailId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    State = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Plaque = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    AddressText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrackingCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sum = table.Column<decimal>(type: "Money", nullable: false),
                    IsPayed = table.Column<bool>(type: "bit", nullable: false),
                    PayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsReceived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDiscountCodes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDiscountCodes", x => new { x.UserId, x.DiscountId });
                    table.ForeignKey(
                        name: "FK_UserDiscountCodes_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "DiscountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDiscountCodes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInvites",
                columns: table => new
                {
                    UserInviteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InviterUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InvitedUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInvites", x => x.UserInviteId);
                    table.ForeignKey(
                        name: "FK_UserInvites_Users_InviterUserId",
                        column: x => x.InviterUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    WalletId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WalletTypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<decimal>(type: "Money", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPayed = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.WalletId);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wallets_WalletTypes_WalletTypeId",
                        column: x => x.WalletTypeId,
                        principalTable: "WalletTypes",
                        principalColumn: "WalletTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CommentTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteProducts",
                columns: table => new
                {
                    FavoriteProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProducts", x => x.FavoriteProductId);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductGalleries",
                columns: table => new
                {
                    ProductGalleryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGalleries", x => x.ProductGalleryId);
                    table.ForeignKey(
                        name: "FK_ProductGalleries_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductToChildCategories",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ChildCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductToChildCategories", x => new { x.ProductId, x.ChildCategoryId });
                    table.ForeignKey(
                        name: "FK_ProductToChildCategories_ChildCategories_ChildCategoryId",
                        column: x => x.ChildCategoryId,
                        principalTable: "ChildCategories",
                        principalColumn: "ChildCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductToChildCategories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    QuestionTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    QuestionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Questions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopperProducts",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ShopperUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    SaleCount = table.Column<int>(type: "int", nullable: false),
                    ShopperId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopperProducts", x => new { x.ShopperUserId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ShopperProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopperProducts_Shoppers_ShopperId",
                        column: x => x.ShopperId,
                        principalTable: "Shoppers",
                        principalColumn: "ShopperId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopperProducts_Users_ShopperUserId",
                        column: x => x.ShopperUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ShopperUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrackingCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Count = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    Sum = table.Column<decimal>(type: "Money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Users_ShopperUserId",
                        column: x => x.ShopperUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommentAnswers",
                columns: table => new
                {
                    CommentAnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentAnswers", x => x.CommentAnswerId);
                    table.ForeignKey(
                        name: "FK_CommentAnswers_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswers",
                columns: table => new
                {
                    QuestionAnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswers", x => x.QuestionAnswerId);
                    table.ForeignKey(
                        name: "FK_QuestionAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandProducts_BrandId",
                table: "BrandProducts",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildCategoryToCategories_ChildCategoryId",
                table: "ChildCategoryToCategories",
                column: "ChildCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentAnswers_CommentId",
                table: "CommentAnswers",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentAnswers_UserId",
                table: "CommentAnswers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProductId",
                table: "Comments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_ProductId",
                table: "FavoriteProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_UserId",
                table: "FavoriteProducts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ShopperUserId",
                table: "OrderDetails",
                column: "ShopperUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ParentId",
                table: "Permissions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGalleries_ProductId",
                table: "ProductGalleries",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BatteryChargerDetailId",
                table: "Products",
                column: "BatteryChargerDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_FlashMemoryDetailId",
                table: "Products",
                column: "FlashMemoryDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_HandsfreeAndHeadPhoneDetailId",
                table: "Products",
                column: "HandsfreeAndHeadPhoneDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_LaptopDetailId",
                table: "Products",
                column: "LaptopDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MemoryCardDetailId",
                table: "Products",
                column: "MemoryCardDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MobileCoverDetailId",
                table: "Products",
                column: "MobileCoverDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MobileDetailId",
                table: "Products",
                column: "MobileDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PowerBankDetailId",
                table: "Products",
                column: "PowerBankDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SmartWatchDetailId",
                table: "Products",
                column: "SmartWatchDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SpeakerDetailId",
                table: "Products",
                column: "SpeakerDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TabletDetailId",
                table: "Products",
                column: "TabletDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_WristWatchDetailId",
                table: "Products",
                column: "WristWatchDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductToChildCategories_ChildCategoryId",
                table: "ProductToChildCategories",
                column: "ChildCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_QuestionId",
                table: "QuestionAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_UserId",
                table: "QuestionAnswers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ProductId",
                table: "Questions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_UserId",
                table: "Questions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProducts_ProductId",
                table: "ShopperProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProducts_ShopperId",
                table: "ShopperProducts",
                column: "ShopperId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperStoreTitles_StoreTitleId",
                table: "ShopperStoreTitles",
                column: "StoreTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_StateCities_CityId",
                table: "StateCities",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscountCodes_DiscountId",
                table: "UserDiscountCodes",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInvites_InviterUserId",
                table: "UserInvites",
                column: "InviterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ShopperId",
                table: "Users",
                column: "ShopperId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_WalletTypeId",
                table: "Wallets",
                column: "WalletTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "BrandProducts");

            migrationBuilder.DropTable(
                name: "ChildCategoryToCategories");

            migrationBuilder.DropTable(
                name: "CommentAnswers");

            migrationBuilder.DropTable(
                name: "FavoriteProducts");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductGalleries");

            migrationBuilder.DropTable(
                name: "ProductToChildCategories");

            migrationBuilder.DropTable(
                name: "QuestionAnswers");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "ShopperProducts");

            migrationBuilder.DropTable(
                name: "ShopperStoreTitles");

            migrationBuilder.DropTable(
                name: "StateCities");

            migrationBuilder.DropTable(
                name: "UserDiscountCodes");

            migrationBuilder.DropTable(
                name: "UserInvites");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ChildCategories");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "StoreTitles");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "WalletTypes");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BatteryChargerDetails");

            migrationBuilder.DropTable(
                name: "FlashMemoryDetails");

            migrationBuilder.DropTable(
                name: "HandsfreeAndHeadPhoneDetails");

            migrationBuilder.DropTable(
                name: "LaptopDetails");

            migrationBuilder.DropTable(
                name: "MemoryCardDetails");

            migrationBuilder.DropTable(
                name: "MobileCoverDetails");

            migrationBuilder.DropTable(
                name: "MobileDetails");

            migrationBuilder.DropTable(
                name: "PowerBankDetails");

            migrationBuilder.DropTable(
                name: "SmartWatchDetails");

            migrationBuilder.DropTable(
                name: "SpeakerDetails");

            migrationBuilder.DropTable(
                name: "TabletDetails");

            migrationBuilder.DropTable(
                name: "WristWatchDetails");

            migrationBuilder.DropTable(
                name: "Shoppers");
        }
    }
}
