using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class CouponModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name="Coupon Type")]
        public string CouponType { get; set; }

        public enum ECoupounType { Percent=0, Dollar=1 }

        [Required]
        public double Discount { get; set; }

        [Required]
        [Display(Name= "Minimum Amount")]
        public double MinimumAmount { get; set; }

        public byte[] Picture { get; set; }

        public bool IsActive { get; set; }
    }
}
