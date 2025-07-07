using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using ExcelToWeb.Models;
using ExcelToWeb.Controllers.helper;
using ExcelToWeb.Models.ViewModels;
using System.Runtime.CompilerServices;
using System.Web.Security;


namespace ExcelToWeb.Controllers
{
    public class usersAccessInfoesController : Controller
    {
        private Supply_ChainEntities1 db = new Supply_ChainEntities1();


        public ActionResult Index()
        {

            //This method only redirects you to Login

            return View();
        }

        // GET: usersAccessInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: usersAccessInfoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(userRegisterViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);

            }


            //Variable for new Id, we get year first
            int actualYear = DateTime.Now.Year;

            //This will be where new id created on db is saved 
            int newId;

            //This will help concatenate 
            string newIdUser;

            //variables for the out values of hashing
            byte[] hash, salt;

            //We use the password to can add some hash on it - this method is static so there is no need to create instances
            Passwordhelper.createPsswordHash(model.Password, out hash, out salt);

            //We convert hash and salt to base64
            string hashBase64 = Convert.ToBase64String(hash);
            string saltBase64 = Convert.ToBase64String(salt);



            //Now creating the users object
            var newUser = new usersAccessInfo
            {

                name = model.name,
                lastName = model.lastName,
                passwordSalt = saltBase64,
                passwordHashing = hashBase64,
                userEmail = model.userEmail,
                userEmailConfirm = false,
                userType = 1
            };

            //Validate modelstate(values to add) and save on database
            if (ModelState.IsValid)
            {
                db.usersAccessInfo.Add(newUser);
                db.SaveChanges();

                //Now we save th return value of the usercreated
                newId = newUser.Id;
                //We now concatenate and do something like Year + newId --> eg: 2025 +1 =20251
                newIdUser = (actualYear.ToString() + newId.ToString());

                //We asign new data
                newUser.idUser = newIdUser;


                //We use Entry because this is an existing entity and we dont want to erase other columns data
                db.Entry(newUser).State = EntityState.Modified;

                //Now modified we also save changes
                db.SaveChanges();


                return RedirectToAction("Index");
            }



            //If model is not valid we go back view with visible errors
            return View(model);
        }


        [HttpGet]
        public ActionResult Details()
        {

            //Obtenemos el usuario de sesion, si es null con el ? se acepta valor como nulo
            string Id = Session["idUser"]?.ToString();

            //Obtenemos los datos del usuario en sesion
            var userDetails = db.usersAccessInfo.FirstOrDefault(u=>u.idUser==Id);
                            
            return View(userDetails);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(userLoginViewModel model)
        {
            try
            {
                //We first check model state
                if (!ModelState.IsValid) 
                {
                    return View(model);
                }


                //Variables for hashStored and passwordStored
                byte[] hashStored, saltStored;
                //This brings the true or false from verifyPassword method
                bool verifiedPassword;

                //Now we check if user exists on database and if data given was email or User
                var foundUser = !string.IsNullOrWhiteSpace(model.idUser) ? 
                    db.usersAccessInfo.FirstOrDefault(u => u.idUser == model.idUser || u.userEmail==model.idUser) 
                    : null;


                //We check this is not emptyu
                if (foundUser == null)
                {
                    TempData["Error"] = "User or Password incorrect!";
                    return RedirectToAction("Index", "usersAccessInfoes");
                }


                //Now we then get passwordSalt and passwordHash
                hashStored = Convert.FromBase64String(foundUser.passwordHashing);
                saltStored = Convert.FromBase64String(foundUser.passwordSalt);

                //Now we use then helper to check if password given is correct
                verifiedPassword = Passwordhelper.verifyPassword(model.Password, hashStored, saltStored);

                //We  check if values is true
                if (!verifiedPassword)
                {

                    TempData["Error"] = "User or Password incorrect!";
                    return RedirectToAction("Index", "usersAccessInfoes");

                }
                else
                {
                    //We use the authentication cookie to stay log in
                    FormsAuthentication.SetAuthCookie(foundUser.idUser.ToString(), true);
                    //We save on session user
                    Session["id"] = foundUser.Id;
                    Session["idUser"] =foundUser.idUser;
                    Session["name"] =foundUser.name;
                    Session["lastName"] = foundUser.lastName;



                    //Now if everything is okay then we redirect to the Home view
                    return RedirectToAction("Items", "inventoryItems");

                }



            }
            catch (Exception e) 
            {

                TempData["Error"] = "Ocurrió un error inesperado.";
                //If by any chance a cacth error comes in, then return to index view of Login
                return RedirectToAction("Index","usersAccessInfoes");
            }

            

            
        }

        public ActionResult logOut()
        {
            //SE BORRAN DATOS DE SESION
            Session.Abandon();

            //Se borra los datos de autenticacion en forms
            FormsAuthentication.SignOut();

            //Ahora se regresa al loggin

            return RedirectToAction("Index","usersAccessInfoes");
            
        }

        

        
        /*This method closes the DB connections and cleans up memory*/
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
