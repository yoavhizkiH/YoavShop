using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using YoavShop.DAL;


namespace YoavShop.Controllers
{
    public class LogInController : Controller
    {
        // GET: /Login/

        YoavShopContext Db = new YoavShopContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login LoginVar)
        {



            var result1 = (from emp in Db.TblEmployees where emp.EmailId == LoginVar.UserName select emp.Password).FirstOrDefault();

            string DPassword = Decrypt(result1);

            // Db.TblEmployees.Where(m=>m.EmailId==LoginVar.UserName && m.Password==LoginVar.Password).FirstOrDefault()

            if (DPassword == LoginVar.Password)

            {

                return RedirectToAction("EmployeeDetails", "Employee", new { area = "Admin" });

            }

            else

            {

                ViewBag.Message = string.Format("UserName and Password is incorrect");

                return View();

            }



        }



        [NonAction]

        public string Decrypt(string cipherText)

        {

            if (cipherText != null)

            {

                string EncryptionKey = "MAKV2SPBNI99212";

                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (Aes encryptor = Aes.Create())

                {

                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                    encryptor.Key = pdb.GetBytes(32);

                    encryptor.IV = pdb.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())

                    {

                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))

                        {

                            cs.Write(cipherBytes, 0, cipherBytes.Length);

                            cs.Close();

                        }

                        cipherText = Encoding.Unicode.GetString(ms.ToArray());

                    }

                }

                return cipherText;

            }

            return null;

        }

    }

}