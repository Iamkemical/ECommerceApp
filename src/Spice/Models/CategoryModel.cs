using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }
    }
}
