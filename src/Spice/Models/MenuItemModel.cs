using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class MenuItemModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public string Spicyness { get; set; }
        public enum ESpicy { NA=0, NotSpicy=1, Spicy=2, VerySpicy=3 }

        public string Image { get; set; }

        [Display(Name ="Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryModel Category { get; set; }

        [Display(Name="SubCategory")]
        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual SubCategoryModel SubCategory { get; set; }

        [Range(1, int.MaxValue, ErrorMessage ="The Price should not be less than ${1}")]
        public int Price { get; set; }


    }
}
