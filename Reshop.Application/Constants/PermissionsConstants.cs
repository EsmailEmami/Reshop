using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Application.Constants
{
   public static class PermissionsConstants
   {
       public const string AdminPanel = "AdminPanelMainPage";

       // product panel
       public const string ProductsPage = "ProductsMainPage";
       public const string AddAux = "AddAUX";
        public const string EditAux = "EditAUX";
        public const string ProductDetail = "ProductDetail";
        public const string ColorDetailOfProduct = "ColorDetail-Product";
        public const string DiscountDetailOfProduct = "DiscountDetail-Product";

        // brand panel
        public const string BrandsPage = "BrandsMainPage";
        public const string AddBrand = "AddBrand";
        public const string EditBrand = "EditBrand";
        public const string BrandDetail = "BrandDetail";
        public const string AvailableBrand = "AvailableBrand";
        public const string UnAvailableBrand = "UnAvailableBrand";


        // official brand product panel
        public const string OfficialBrandProductsPage = "OfficialBrandProductsMainPage";
        public const string AddOfficialBrandProduct = "AddOfficialBrandProduct";
        public const string EditOfficialBrandProduct = "EditOfficialBrandProduct";
        public const string OfficialBrandProductDetail = "OfficialBrandProductDetail";
        public const string AvailableOfficialBrandProduct = "AvailableOfficialBrandProduct";
        public const string UnAvailableOfficialBrandProduct = "UnAvailableOfficialBrandProduct";

        // color panel
        public const string ColorsPage = "ColorsMainPage";
        public const string AddColor = "AddColor";
        public const string EditColor = "EditColor";

        // permission panel
        public const string PermissionsPage = "PermissionsMainPage";
        public const string AddPermission = "AddPermission";
        public const string EditPermission = "EditPermission";
        public const string DeletePermission = "DeletePermission";

        // role panel
        public const string RolesPage = "RolesMainPage";
        public const string AddRole = "AddRole";
        public const string EditRole = "EditRole";
        public const string DeleteRole = "DeleteRole";

        // shopper
        public const string Shopper = "Shopper";
   }
}
