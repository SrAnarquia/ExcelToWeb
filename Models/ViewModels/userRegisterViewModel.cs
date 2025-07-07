using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExcelToWeb.Models.ViewModels
{
	public class userRegisterViewModel
	{

		


		[Required(ErrorMessage ="El nombre es obligatorio")]
		public string name { get; set; }


		[Required(ErrorMessage ="El apellido es obligatorio")]
		public string lastName { get; set; }


		[Required(ErrorMessage ="El Email es obligatorio")]
		[EmailAddress]
		public string userEmail { get; set; }


		[Required(ErrorMessage ="El passsword es obligatorio")]
		[DataType(DataType.Password)]
		[MinLength(6)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password",ErrorMessage="Las contraseñas no coinciden")]
		public string ConfirmPassword { get; set; }











	}
}