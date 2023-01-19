using Microsoft.AspNetCore.Mvc;
using Reenbit.HireMe.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Reenbit.HireMe.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IConfigurationManager configurationManager;

        public BaseController(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        protected string UserEmail => this.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

        protected bool IsAdmin() 
        {
            return this.configurationManager.Admins.Contains(this.UserEmail);
        }


        public static string EncryptString(string key, object plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
    }
}