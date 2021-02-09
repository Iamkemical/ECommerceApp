using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<MenuItemModel> MenuItem { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        public IEnumerable<CouponModel> Coupon { get; set; }
    }
}
