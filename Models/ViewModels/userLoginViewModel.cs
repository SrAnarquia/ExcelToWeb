using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

//This view model is to create and check errors on logins and avoid being redirect to ErrorView on shared folder

namespace ExcelToWeb.Models.ViewModels
{
	public class userLoginViewModel
	{
		[Required(ErrorMessage = "Usuario no puede estar vaci")]
		public string idUser { get; set; }

		[Required(ErrorMessage = "Contraseña no puede esta vacia")]
		public string Password { get;set; }



	}
}