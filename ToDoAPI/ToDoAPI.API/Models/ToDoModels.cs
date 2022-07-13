using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.API.Models
{
    public class ToDoViewModel
    {
        [Key]
        public int ToDoID { get; set; }
        [Required]
        public string Action { get; set; }
        [Required]
        public bool Done { get; set; }
        [Required]
        public int CategoryID { get; set; }
        public virtual CategoryViewModel Category { get; set; }
    }

    public class CategoryViewModel
    {
        [Key]
        public int CategoryID { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Name must be 50 characters or less.")]
        public string Name { get; set; }
        [StringLength(100, ErrorMessage = "Description must be 100 characters or less.")]
        public string Description { get; set; }

    }
}