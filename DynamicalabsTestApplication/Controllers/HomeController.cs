using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DynamicalabsTestApplication.Controllers
{
    public class HomeController : Controller
    {
        public class BaseModel
        {
            [Required]
            [Display(Name = "File")]
            [DataType(DataType.Upload)]
            public IFormFile File { get; set; }
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile(BaseModel model)
        {
            if (model != null && model.File != null && model.File.Length > 0)
            {
                switch (model.File.ContentType.ToLower())
                {
                    case "application/zip":
                    case "application/x-zip":
                    case "application/x-zip-compressed":
                        break;
                    default:
                        throw new Exception("Invalid file type");
                }

                var filePath = Path.GetTempFileName();
                try
                {
                    //Upload zip file to temporary folder
                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await model.File.CopyToAsync(stream);


                    var proc = new Models.Processing();
                    await proc.MethodA(filePath);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw ex;
                }
                finally
                {
                    var fi = new FileInfo(filePath);
                    if (fi.Exists) fi.Delete();
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
