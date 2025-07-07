using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Collections;




namespace ExcelToWeb.Controllers.helper
{
	public static class Passwordhelper
	{

		//This method creates the hash
		public static void createPsswordHash(string Pass,out byte[] hash,out byte[] salt) 
		{


			//WE CREATE THE OUTPUT SALT
			using (var rng = RandomNumberGenerator.Create()) 
			{
				salt = new byte[16];

				rng.GetBytes(salt);
				
			}

			//WE THEN CREATE THE OUTPUT HASH WITH THE SALT

			using (var pkdf2 =new Rfc2898DeriveBytes(Pass,salt,100_000) ) 
			{
				hash = pkdf2.GetBytes(32);
				
			}




		}

		//This method is isued to login and also verify hash

		public static bool verifyPassword(string pass, byte[] storedHash, byte[] storedSalt) 
		{
			//With stored values we then recreate again hash
			using (var pkdf2 = new Rfc2898DeriveBytes(pass, storedSalt, 100_000))
			{
				//With the stored values we then create a new hash
				byte[] computeHash = pkdf2.GetBytes(32);


				//We compare recreated hash and storedHash and return value

				return StructuralComparisons.StructuralEqualityComparer.Equals(computeHash,storedHash);
			}


		}




	}
}