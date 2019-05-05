using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PgpCore;
/// <summary>
/// https://blog.bitscry.com/2018/07/05/pgp-encryption-and-decryption-in-c/
//// Encrypt file
//pgp.EncryptFile(@"C:\TEMP\keys\content.txt", @"C:\TEMP\keys\content__encrypted.pgp", @"C:\TEMP\keys\public.asc", true, true);
//// Encrypt and sign file
//pgp.EncryptFileAndSign(@"C:\TEMP\keys\content.txt", @"C:\TEMP\keys\content__encrypted_signed.pgp", @"C:\TEMP\keys\public.asc", @"C:\TEMP\keys\private.asc", "password", true, true);
//// Decrypt file
//pgp.DecryptFile(@"C:\TEMP\keys\content__encrypted.pgp", @"C:\TEMP\keys\content__decrypted.txt", @"C:\TEMP\keys\private.asc", "password");
//// Decrypt signed file
//pgp.DecryptFile(@"C:\TEMP\keys\content__encrypted_signed.pgp", @"C:\TEMP\keys\content__decrypted_signed.txt", @"C:\TEMP\keys\private.asc", "password");

//// Encrypt stream
//using (FileStream inputFileStream = new FileStream(@"C:\TEMP\keys\content.txt", FileMode.Open))
//using (Stream outputFileStream = File.Create(@"C:\TEMP\keys\content__encrypted2.pgp"))
//using (Stream publicKeyStream = new FileStream(@"C:\TEMP\keys\public.asc", FileMode.Open))
//    pgp.EncryptStream(inputFileStream, outputFileStream, publicKeyStream, true, true);

//// Decrypt stream
//using (FileStream inputFileStream = new FileStream(@"C:\TEMP\keys\content__encrypted2.pgp", FileMode.Open))
//using (Stream outputFileStream = File.Create(@"C:\TEMP\keys\content__decrypted2.txt"))
//using (Stream privateKeyStream = new FileStream(@"C:\TEMP\keys\private.asc", FileMode.Open))
//    pgp.DecryptStream(inputFileStream, outputFileStream, privateKeyStream, "password");
/// </summary>
namespace PGPEncryptAndDecrypt.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PGPController : ControllerBase
    {
        private IHostingEnvironment _env;
        public PGPController(IHostingEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        [ActionName("GenerateKey")]
        public IActionResult GenerateKey()
        {
            bool IsGenrate = false;
            string webRoot = _env.WebRootPath;
            String FileCert = webRoot + "/Cert/";

            using (PGP pgp = new PGP())
            {
                try
                {
                    // Generate keys
                    //pgp.GenerateKey(FileCert + "public.asc", FileCert + "private.asc", null, null);
                    pgp.GenerateKey(FileCert + "public.asc", FileCert + "private.asc", "youremail@email.com", "yourpassword");
                    IsGenrate = true;
                }
                catch(Exception e)
                {
                    IsGenrate = false;
                }
            }

            return Ok(IsGenrate);
        }

        [HttpGet]
        [ActionName("EncryptFile")]
        public IActionResult EncryptFile()
        {
            bool IsEncrypt = false;
            string webRoot = _env.WebRootPath;
            String FileCert = webRoot + "/Cert/";
            try
            {
                using (PGP pgp = new PGP())
                {
                    //pgp.EncryptFile((webRoot+ "/OriginalFile/MyFile.txt"), (webRoot + "/EncryptFile/MyFile.pgp"), (FileCert + "public.asc"), true, true);
                    pgp.EncryptFileAndSign((webRoot + "/OriginalFile/MyFile.txt"), (webRoot + "/EncryptFile/MyFile.pgp"), (FileCert + "public.asc"), (FileCert + "private.asc"), "yourpassword", true, true);

                    IsEncrypt = true;
                }
            }
            catch(Exception e)
            {
                IsEncrypt = false;
            }
            return Ok(IsEncrypt);
        }

        [HttpGet]
        [ActionName("DecryptFile")]
        public IActionResult DecryptFile()
        {
            bool IsDecrypt = false;
            string webRoot = _env.WebRootPath;
            String FileCert = webRoot + "/Cert/";
            try
            {
                using (PGP pgp = new PGP())
                {
                    //pgp.DecryptFile((webRoot + "/EncryptFile/MyFile.pgp"), (webRoot + "/DecryptFile/MyFile_Decrypt.txt"), (FileCert + "private.asc"), null);
                    pgp.DecryptFile((webRoot + "/EncryptFile/MyFile.pgp"), (webRoot + "/DecryptFile/MyFile_Decrypt.txt"), (FileCert + "private.asc"), "yourpassword");

                    IsDecrypt = true;
                }
            }
            catch (Exception e)
            {
                IsDecrypt = false;
            }
            return Ok(IsDecrypt);
        }
    }
}