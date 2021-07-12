using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Application.Generator
{
    public static class ShopperProductRequestReasons
    {
        public static string AdminAdded()
        {
            return "این محصول توسط ادمین به فروشنده اضافه شده است.";
        }
        public static string AdminEdited()
        {
            return "این محصول توسط ادمین ویرایش شده است.";
        }
    }
}
