using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models.ViewModels
{
    public class MenuItemViewModel
    {
        public MenuItemModel MenuItem { get; set; }
        public IEnumerable<CategoryModel> Category { get; set; }
        public IEnumerable<SubCategoryModel> SubCategory { get; set; }
    }
}
