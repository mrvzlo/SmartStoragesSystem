﻿using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.CreationModels
{
	public class SignInModel
	{
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Description = "Email")]
        [StringLength(128)]
		public string Email{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Description = "Password")]
        [StringLength(128)]
        public string Password{ get; set; }
        public string ReturnUrl { get; set; }
    }

}