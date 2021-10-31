using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuxDetails",
                columns: table => new
                {
                    AUXDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CableMaterial = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CableLenght = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuxDetails", x => x.AUXDetailId);
                });

            migrationBuilder.CreateTable(
                name: "BatteryChargerDetails",
                columns: table => new
                {
                    BatteryChargerDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lenght = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Width = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Height = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Weight = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    InputVoltage = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OutputVoltage = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OutputCurrentIntensity = table.Column<double>(type: "float", nullable: false),
                    OutputPortsCount = table.Column<byte>(type: "tinyint", nullable: false),
                    OutputTypeCharger = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MobileCable = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MoreInformation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatteryChargerDetails", x => x.BatteryChargerDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ColorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.ColorId);
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
                    Length = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Width = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Height = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BodyMaterial = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Connector = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Led = table.Column<bool>(type: "bit", nullable: false),
                    IsImpactResistance = table.Column<bool>(type: "bit", nullable: false),
                    WaterResistance = table.Column<bool>(type: "bit", nullable: false),
                    ShockResistance = table.Column<bool>(type: "bit", nullable: false),
                    DustResistance = table.Column<bool>(type: "bit", nullable: false),
                    AntiScratch = table.Column<bool>(type: "bit", nullable: false),
                    AntiStain = table.Column<bool>(type: "bit", nullable: false),
                    SpeedDataTransfer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SpeedDataReading = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OsCompatibility = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    MoreInformation = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
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
                    Length = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Width = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Height = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Weight = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CpuCompany = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CpuSeries = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CpuModel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CpuFerequancy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CpuCache = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RamStorage = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    RamStorageTeachnology = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Storage = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    StorageTeachnology = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StorageInformation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GpuCompany = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GpuModel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GpuRam = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    DisplaySize = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DisplayTeachnology = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayResolutation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RefreshDisplay = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    BlurDisplay = table.Column<bool>(type: "bit", nullable: false),
                    TouchDisplay = table.Column<bool>(type: "bit", nullable: false),
                    DiskDrive = table.Column<bool>(type: "bit", nullable: false),
                    FingerTouch = table.Column<bool>(type: "bit", nullable: false),
                    Webcam = table.Column<bool>(type: "bit", nullable: false),
                    BacklightKey = table.Column<bool>(type: "bit", nullable: false),
                    TouchPadInformation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModemInformation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Wifi = table.Column<bool>(type: "bit", nullable: false),
                    Bluetooth = table.Column<bool>(type: "bit", nullable: false),
                    VgaPort = table.Column<bool>(type: "bit", nullable: false),
                    HtmiPort = table.Column<bool>(type: "bit", nullable: false),
                    DisplayPort = table.Column<bool>(type: "bit", nullable: false),
                    LanPort = table.Column<bool>(type: "bit", nullable: false),
                    UsbCPort = table.Column<bool>(type: "bit", nullable: false),
                    Usb3Port = table.Column<bool>(type: "bit", nullable: false),
                    UsbCQuantity = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    UsbQuantity = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    Usb3Quantity = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    BatteryMaterial = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BatteryCharging = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BatteryInformation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Os = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Classification = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false)
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
                    Length = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Width = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Height = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Capacity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SpeedStandard = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReadingSpeed = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    ResistsAgainst = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MoreInformation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
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
                    Lenght = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Width = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Height = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Weight = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SimCardQuantity = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    SimCardInpute = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SeparateSlotMemoryCard = table.Column<bool>(type: "bit", nullable: false),
                    Announced = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ChipsetName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Cpu = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CpuAndFrequency = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CpuArch = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Gpu = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    InternalStorage = table.Column<int>(type: "int", maxLength: 40, nullable: false),
                    Ram = table.Column<int>(type: "int", maxLength: 40, nullable: false),
                    SdCard = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SdCardStandard = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ColorDisplay = table.Column<bool>(type: "bit", nullable: false),
                    TouchDisplay = table.Column<bool>(type: "bit", nullable: false),
                    DisplayTechnology = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DisplaySize = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Resolution = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PixelDensity = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    ScreenToBodyRatio = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    ImageRatio = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    DisplayProtection = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    MoreInformation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ConnectionsNetwork = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    GsmNetwork = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HspaNetwork = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LteNetwork = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FiveGNetwork = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CommunicationTechnology = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WiFi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Radio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Bluetooth = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    GpsInformation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConnectionPort = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    CameraQuantity = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    PhotoResolutation = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    SelfiCameraPhoto = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    CameraCapabilities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelfiCameraCapabilities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Filming = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Speakers = table.Column<bool>(type: "bit", nullable: false),
                    OutputAudio = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AudioInformation = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OsVersion = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    UiVersion = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MoreInformationSoftWare = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatteryMaterial = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BatteryCapacity = table.Column<int>(type: "int", maxLength: 6, nullable: false),
                    RemovableBattery = table.Column<bool>(type: "bit", maxLength: 6, nullable: false),
                    Sensors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemsInBox = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Length = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Width = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Height = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Weight = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CapacityRange = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    InputVoltage = table.Column<double>(type: "float", nullable: false),
                    OutputVoltage = table.Column<double>(type: "float", nullable: false),
                    InputCurrentIntensity = table.Column<double>(type: "float", nullable: false),
                    OutputCurrentIntensity = table.Column<double>(type: "float", nullable: false),
                    OutputPortsCount = table.Column<int>(type: "int", nullable: false),
                    IsSupportOfQCTechnology = table.Column<bool>(type: "bit", maxLength: 200, nullable: false),
                    IsSupportOfPDTechnology = table.Column<bool>(type: "bit", maxLength: 200, nullable: false),
                    BodyMaterial = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayCharge = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Features = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerBankDetails", x => x.PowerBankId);
                });

            migrationBuilder.CreateTable(
                name: "ReportCommentTypes",
                columns: table => new
                {
                    ReportCommentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportCommentTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCommentTypes", x => x.ReportCommentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ReportQuestionAnswerTypes",
                columns: table => new
                {
                    ReportQuestionAnswerTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportQuestionAnswerTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportQuestionAnswerTypes", x => x.ReportQuestionAnswerTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ReportQuestionTypes",
                columns: table => new
                {
                    ReportQuestionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportQuestionTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportQuestionTypes", x => x.ReportQuestionTypeId);
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
                name: "SmartWatchDetails",
                columns: table => new
                {
                    SmartWatchDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lenght = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Width = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Height = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Weight = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    SuitableFor = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Application = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DisplayForm = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GlassMaterial = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CaseMaterial = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaterialStrap = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TypeOfLock = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ColorDisplay = table.Column<bool>(type: "bit", nullable: false),
                    TouchDisplay = table.Column<bool>(type: "bit", nullable: false),
                    DisplaySize = table.Column<double>(type: "float", maxLength: 8, nullable: false),
                    Resolution = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PixelDensity = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    DisplayType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MoreInformationDisplay = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SimcardIsSoppurt = table.Column<bool>(type: "bit", nullable: false),
                    RegisteredSimCardIsSoppurt = table.Column<bool>(type: "bit", nullable: false),
                    GpsIsSoppurt = table.Column<bool>(type: "bit", nullable: false),
                    Os = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Compatibility = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Prossecor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    InternalStorage = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    ExternalStorageSoppurt = table.Column<bool>(type: "bit", nullable: false),
                    Camera = table.Column<bool>(type: "bit", nullable: false),
                    MusicControl = table.Column<bool>(type: "bit", nullable: false),
                    Connections = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sensors = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    BatteryMaterial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CallIsSoppurt = table.Column<bool>(type: "bit", maxLength: 50, nullable: false),
                    MoreInformationHardware = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Lenght = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Width = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Height = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ConnectionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Connector = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsMemoryCardInput = table.Column<bool>(type: "bit", nullable: false),
                    IsSupportUSBPort = table.Column<bool>(type: "bit", nullable: false),
                    HeadphoneOutput = table.Column<bool>(type: "bit", nullable: false),
                    InputSound = table.Column<bool>(type: "bit", nullable: false),
                    MicrophoneInpute = table.Column<bool>(type: "bit", nullable: false),
                    IsSupportMicrophone = table.Column<bool>(type: "bit", nullable: false),
                    Display = table.Column<bool>(type: "bit", nullable: false),
                    ControlRemote = table.Column<bool>(type: "bit", nullable: false),
                    IsSupportRadio = table.Column<bool>(type: "bit", nullable: false),
                    Bluetooth = table.Column<bool>(type: "bit", nullable: false),
                    ConnectTwoDevice = table.Column<bool>(type: "bit", nullable: false),
                    SpeakerItemQuantity = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    IsBattery = table.Column<bool>(type: "bit", nullable: false),
                    PlayingTime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChargingTime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OsSoppurt = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
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
                    Lenght = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Width = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Height = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Weight = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SimCardIsTrue = table.Column<bool>(type: "bit", nullable: false),
                    Call = table.Column<bool>(type: "bit", nullable: false),
                    SimCardQuantity = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    SimCardInpute = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SeparateSlotMemoryCard = table.Column<bool>(type: "bit", nullable: false),
                    Announced = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ChipsetName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Cpu = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CpuAndFrequency = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CpuArch = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Gpu = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    InternalStorage = table.Column<int>(type: "int", maxLength: 40, nullable: false),
                    Ram = table.Column<int>(type: "int", maxLength: 40, nullable: false),
                    SdCard = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SdCardStandard = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ColorDisplay = table.Column<bool>(type: "bit", nullable: false),
                    TouchDisplay = table.Column<bool>(type: "bit", nullable: false),
                    DisplayTechnology = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DisplaySize = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Resolution = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PixelDensity = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    ScreenToBodyRatio = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    ImageRatio = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    DisplayProtection = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    MoreInformation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ConnectionsNetwork = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    GsmNetwork = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HspaNetwork = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LteNetwork = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FiveGNetwork = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CommunicationTechnology = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WiFi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Radio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Bluetooth = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    GpsInformation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConnectionPort = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    CameraQuantity = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    PhotoResolutation = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    SelfiCameraPhoto = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    CameraCapabilities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelfiCameraCapabilities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Filming = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Speakers = table.Column<bool>(type: "bit", nullable: false),
                    OutputAudio = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AudioInformation = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OsVersion = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    UiVersion = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MoreInformationSoftWare = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatteryMaterial = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BatteryCapacity = table.Column<int>(type: "int", maxLength: 6, nullable: false),
                    RemovableBattery = table.Column<bool>(type: "bit", maxLength: 6, nullable: false),
                    Sensors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemsInBox = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabletDetails", x => x.TabletDetailId);
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
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AccountBalance = table.Column<decimal>(type: "Money", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
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
                    Lenght = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Width = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Height = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Weight = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    SuitableFor = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Application = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DisplayForm = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MaterialStrap = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TypeOfLock = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TouchDisplay = table.Column<bool>(type: "bit", nullable: false),
                    DisplaySize = table.Column<double>(type: "float", maxLength: 8, nullable: false),
                    Resolution = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PixelDensity = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    DisplayType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GPS = table.Column<bool>(type: "bit", nullable: false),
                    Compatibility = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Prossesor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Resists = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sensors = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Connection = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Features = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatteryMaterial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BatteryCapacity = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    BattryChargingS = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    MoreInformation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WristWatchDetails", x => x.WristWatchDetailId);
                });

            migrationBuilder.CreateTable(
                name: "CategoryGalleries",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    OrderBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryGalleries", x => new { x.CategoryId, x.ImageName });
                    table.ForeignKey(
                        name: "FK_CategoryGalleries_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChildCategories",
                columns: table => new
                {
                    ChildCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildCategoryTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildCategories", x => x.ChildCategoryId);
                    table.ForeignKey(
                        name: "FK_ChildCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
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
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreTitleId = table.Column<int>(type: "int", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LatinBrandName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandId);
                    table.ForeignKey(
                        name: "FK_Brands_StoreTitles_StoreTitleId",
                        column: x => x.StoreTitleId,
                        principalTable: "StoreTitles",
                        principalColumn: "StoreTitleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shoppers",
                columns: table => new
                {
                    ShopperId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisterShopper = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OnNationalCardImageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BackNationalCardImageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BusinessLicenseImageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shoppers", x => x.ShopperId);
                    table.ForeignKey(
                        name: "FK_Shoppers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Plaque = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AddressText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Addresses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderAddresses",
                columns: table => new
                {
                    OrderAddressId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Plaque = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AddressText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAddresses", x => x.OrderAddressId);
                    table.ForeignKey(
                        name: "FK_OrderAddresses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandToChildCategories",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    ChildCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandToChildCategories", x => new { x.BrandId, x.ChildCategoryId });
                    table.ForeignKey(
                        name: "FK_BrandToChildCategories_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandToChildCategories_ChildCategories_ChildCategoryId",
                        column: x => x.ChildCategoryId,
                        principalTable: "ChildCategories",
                        principalColumn: "ChildCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfficialBrandProducts",
                columns: table => new
                {
                    OfficialBrandProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficialBrandProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LatinOfficialBrandProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficialBrandProducts", x => x.OfficialBrandProductId);
                    table.ForeignKey(
                        name: "FK_OfficialBrandProducts_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
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
                name: "StoresAddress",
                columns: table => new
                {
                    StoreAddressId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopperId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Plaque = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AddressText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    LandlinePhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoresAddress", x => x.StoreAddressId);
                    table.ForeignKey(
                        name: "FK_StoresAddress_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoresAddress_Shoppers_ShopperId",
                        column: x => x.ShopperId,
                        principalTable: "Shoppers",
                        principalColumn: "ShopperId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoresAddress_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OrderAddressId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TrackingCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderDiscount = table.Column<decimal>(type: "Money", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "Money", nullable: false),
                    Sum = table.Column<decimal>(type: "Money", nullable: false),
                    IsPayed = table.Column<bool>(type: "bit", nullable: false),
                    PayDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsReceived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_OrderAddresses_OrderAddressId",
                        column: x => x.OrderAddressId,
                        principalTable: "OrderAddresses",
                        principalColumn: "OrderAddressId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OfficialBrandProductId = table.Column<int>(type: "int", nullable: false),
                    ChildCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    MemoryCardDetailId = table.Column<int>(type: "int", nullable: true),
                    AuxDetailId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_AuxDetails_AuxDetailId",
                        column: x => x.AuxDetailId,
                        principalTable: "AuxDetails",
                        principalColumn: "AUXDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_BatteryChargerDetails_BatteryChargerDetailId",
                        column: x => x.BatteryChargerDetailId,
                        principalTable: "BatteryChargerDetails",
                        principalColumn: "BatteryChargerDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ChildCategories_ChildCategoryId",
                        column: x => x.ChildCategoryId,
                        principalTable: "ChildCategories",
                        principalColumn: "ChildCategoryId",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_Products_OfficialBrandProducts_OfficialBrandProductId",
                        column: x => x.OfficialBrandProductId,
                        principalTable: "OfficialBrandProducts",
                        principalColumn: "OfficialBrandProductId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "ProductGalleries",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    OrderBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGalleries", x => new { x.ProductId, x.ImageName });
                    table.ForeignKey(
                        name: "FK_ProductGalleries_Products_ProductId",
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
                    QuestionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
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
                name: "ShopperProductRequests",
                columns: table => new
                {
                    ShopperProductRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ShopperId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestType = table.Column<bool>(type: "bit", nullable: false),
                    Warranty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopperProductRequests", x => x.ShopperProductRequestId);
                    table.ForeignKey(
                        name: "FK_ShopperProductRequests_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopperProductRequests_Shoppers_ShopperId",
                        column: x => x.ShopperId,
                        principalTable: "Shoppers",
                        principalColumn: "ShopperId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopperProductRequests_Users_RequestUserId",
                        column: x => x.RequestUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopperProducts",
                columns: table => new
                {
                    ShopperProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ShopperId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Warranty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsFinally = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopperProducts", x => x.ShopperProductId);
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
                    QuestionAnswerDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "QuestionLikes",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionLikes", x => new { x.QuestionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_QuestionLikes_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionReports",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportQuestionTypeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionReports", x => new { x.QuestionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_QuestionReports_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionReports_ReportQuestionTypes_ReportQuestionTypeId",
                        column: x => x.ReportQuestionTypeId,
                        principalTable: "ReportQuestionTypes",
                        principalColumn: "ReportQuestionTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionReports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopperProductColorRequests",
                columns: table => new
                {
                    ShopperProductColorRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopperProductId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    RequestType = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopperProductColorRequests", x => x.ShopperProductColorRequestId);
                    table.ForeignKey(
                        name: "FK_ShopperProductColorRequests_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "ColorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopperProductColorRequests_ShopperProducts_ShopperProductId",
                        column: x => x.ShopperProductId,
                        principalTable: "ShopperProducts",
                        principalColumn: "ShopperProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopperProductColorRequests_Users_RequestUserId",
                        column: x => x.RequestUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopperProductColors",
                columns: table => new
                {
                    ShopperProductColorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopperProductId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    ShortKey = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    SaleCount = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFinally = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopperProductColors", x => x.ShopperProductColorId);
                    table.ForeignKey(
                        name: "FK_ShopperProductColors_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "ColorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopperProductColors_ShopperProducts_ShopperProductId",
                        column: x => x.ShopperProductId,
                        principalTable: "ShopperProducts",
                        principalColumn: "ShopperProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswerLikes",
                columns: table => new
                {
                    QuestionAnswerId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswerLikes", x => new { x.QuestionAnswerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_QuestionAnswerLikes_QuestionAnswers_QuestionAnswerId",
                        column: x => x.QuestionAnswerId,
                        principalTable: "QuestionAnswers",
                        principalColumn: "QuestionAnswerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionAnswerLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswerReports",
                columns: table => new
                {
                    QuestionAnswerId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportQuestionAnswerTypeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswerReports", x => new { x.QuestionAnswerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_QuestionAnswerReports_QuestionAnswers_QuestionAnswerId",
                        column: x => x.QuestionAnswerId,
                        principalTable: "QuestionAnswers",
                        principalColumn: "QuestionAnswerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionAnswerReports_ReportQuestionAnswerTypes_ReportQuestionAnswerTypeId",
                        column: x => x.ReportQuestionAnswerTypeId,
                        principalTable: "ReportQuestionAnswerTypes",
                        principalColumn: "ReportQuestionAnswerTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionAnswerReports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
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
                    ShopperProductColorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CommentTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductSatisfaction = table.Column<int>(type: "int", nullable: false),
                    ConstructionQuality = table.Column<int>(type: "int", nullable: false),
                    FeaturesAndCapabilities = table.Column<int>(type: "int", nullable: false),
                    DesignAndAppearance = table.Column<int>(type: "int", nullable: false),
                    OverallScore = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
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
                        name: "FK_Comments_ShopperProductColors_ShopperProductColorId",
                        column: x => x.ShopperProductColorId,
                        principalTable: "ShopperProductColors",
                        principalColumn: "ShopperProductColorId",
                        onDelete: ReferentialAction.Restrict);
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
                    ShopperProductColorId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProducts", x => x.FavoriteProductId);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_ShopperProductColors_ShopperProductColorId",
                        column: x => x.ShopperProductColorId,
                        principalTable: "ShopperProductColors",
                        principalColumn: "ShopperProductColorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ShopperProductColorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    ProductDiscountPrice = table.Column<decimal>(type: "Money", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrackingCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_OrderDetails_ShopperProductColors_ShopperProductColorId",
                        column: x => x.ShopperProductColorId,
                        principalTable: "ShopperProductColors",
                        principalColumn: "ShopperProductColorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopperProductDiscounts",
                columns: table => new
                {
                    ShopperProductDiscountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopperProductColorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DiscountPercent = table.Column<byte>(type: "tinyint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopperProductDiscounts", x => x.ShopperProductDiscountId);
                    table.ForeignKey(
                        name: "FK_ShopperProductDiscounts_ShopperProductColors_ShopperProductColorId",
                        column: x => x.ShopperProductColorId,
                        principalTable: "ShopperProductColors",
                        principalColumn: "ShopperProductColorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommentFeedBacks",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentFeedBacks", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CommentFeedBacks_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentFeedBacks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportComments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportCommentTypeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportComments", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ReportComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportComments_ReportCommentTypes_ReportCommentTypeId",
                        column: x => x.ReportCommentTypeId,
                        principalTable: "ReportCommentTypes",
                        principalColumn: "ReportCommentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityId",
                table: "Addresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_StoreTitleId",
                table: "Brands",
                column: "StoreTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandToChildCategories_ChildCategoryId",
                table: "BrandToChildCategories",
                column: "ChildCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildCategories_CategoryId",
                table: "ChildCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentFeedBacks_UserId",
                table: "CommentFeedBacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProductId",
                table: "Comments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ShopperProductColorId",
                table: "Comments",
                column: "ShopperProductColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_ShopperProductColorId",
                table: "FavoriteProducts",
                column: "ShopperProductColorId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_UserId",
                table: "FavoriteProducts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialBrandProducts_BrandId",
                table: "OfficialBrandProducts",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAddresses_CityId",
                table: "OrderAddresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ShopperProductColorId",
                table: "OrderDetails",
                column: "ShopperProductColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderAddressId",
                table: "Orders",
                column: "OrderAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ParentId",
                table: "Permissions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AuxDetailId",
                table: "Products",
                column: "AuxDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BatteryChargerDetailId",
                table: "Products",
                column: "BatteryChargerDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ChildCategoryId",
                table: "Products",
                column: "ChildCategoryId");

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
                name: "IX_Products_OfficialBrandProductId",
                table: "Products",
                column: "OfficialBrandProductId");

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
                name: "IX_QuestionAnswerLikes_UserId",
                table: "QuestionAnswerLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswerReports_ReportQuestionAnswerTypeId",
                table: "QuestionAnswerReports",
                column: "ReportQuestionAnswerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswerReports_UserId",
                table: "QuestionAnswerReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_QuestionId",
                table: "QuestionAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_UserId",
                table: "QuestionAnswers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionLikes_UserId",
                table: "QuestionLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionReports_ReportQuestionTypeId",
                table: "QuestionReports",
                column: "ReportQuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionReports_UserId",
                table: "QuestionReports",
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
                name: "IX_ReportComments_ReportCommentTypeId",
                table: "ReportComments",
                column: "ReportCommentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportComments_UserId",
                table: "ReportComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductColorRequests_ColorId",
                table: "ShopperProductColorRequests",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductColorRequests_RequestUserId",
                table: "ShopperProductColorRequests",
                column: "RequestUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductColorRequests_ShopperProductId",
                table: "ShopperProductColorRequests",
                column: "ShopperProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductColors_ColorId",
                table: "ShopperProductColors",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductColors_ShopperProductId",
                table: "ShopperProductColors",
                column: "ShopperProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductDiscounts_ShopperProductColorId",
                table: "ShopperProductDiscounts",
                column: "ShopperProductColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductRequests_ProductId",
                table: "ShopperProductRequests",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductRequests_RequestUserId",
                table: "ShopperProductRequests",
                column: "RequestUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductRequests_ShopperId",
                table: "ShopperProductRequests",
                column: "ShopperId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProducts_ProductId",
                table: "ShopperProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProducts_ShopperId",
                table: "ShopperProducts",
                column: "ShopperId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoppers_UserId",
                table: "Shoppers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperStoreTitles_StoreTitleId",
                table: "ShopperStoreTitles",
                column: "StoreTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_StoresAddress_CityId",
                table: "StoresAddress",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_StoresAddress_ShopperId",
                table: "StoresAddress",
                column: "ShopperId");

            migrationBuilder.CreateIndex(
                name: "IX_StoresAddress_StateId",
                table: "StoresAddress",
                column: "StateId");

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
                name: "BrandToChildCategories");

            migrationBuilder.DropTable(
                name: "CategoryGalleries");

            migrationBuilder.DropTable(
                name: "CommentFeedBacks");

            migrationBuilder.DropTable(
                name: "FavoriteProducts");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductGalleries");

            migrationBuilder.DropTable(
                name: "QuestionAnswerLikes");

            migrationBuilder.DropTable(
                name: "QuestionAnswerReports");

            migrationBuilder.DropTable(
                name: "QuestionLikes");

            migrationBuilder.DropTable(
                name: "QuestionReports");

            migrationBuilder.DropTable(
                name: "ReportComments");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "ShopperProductColorRequests");

            migrationBuilder.DropTable(
                name: "ShopperProductDiscounts");

            migrationBuilder.DropTable(
                name: "ShopperProductRequests");

            migrationBuilder.DropTable(
                name: "ShopperStoreTitles");

            migrationBuilder.DropTable(
                name: "StoresAddress");

            migrationBuilder.DropTable(
                name: "UserDiscountCodes");

            migrationBuilder.DropTable(
                name: "UserInvites");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "QuestionAnswers");

            migrationBuilder.DropTable(
                name: "ReportQuestionAnswerTypes");

            migrationBuilder.DropTable(
                name: "ReportQuestionTypes");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "ReportCommentTypes");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "WalletTypes");

            migrationBuilder.DropTable(
                name: "OrderAddresses");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "ShopperProductColors");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "ShopperProducts");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Shoppers");

            migrationBuilder.DropTable(
                name: "AuxDetails");

            migrationBuilder.DropTable(
                name: "BatteryChargerDetails");

            migrationBuilder.DropTable(
                name: "ChildCategories");

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
                name: "OfficialBrandProducts");

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
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "StoreTitles");
        }
    }
}
