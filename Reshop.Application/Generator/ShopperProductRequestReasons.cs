using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Application.Generator
{
    public static class ShopperProductRequestReasons
    {
        public static string AdminAddedProduct()
        {
            return "این محصول توسط ادمین به فروشنده افزوده شده است.";
        }
        public static string AdminEditedProduct()
        {
            return "این محصول توسط ادمین ویرایش شده است.";
        }
        public static string AdminAddedColor()
        {
            return "این رنگ از محصول توسط ادمین افزوده شده است.";
        }
        public static string AdminEditedColor()
        {
            return "این رنگ از محصول توسط ادمین ویرایش شده است.";
        }
    }
}
