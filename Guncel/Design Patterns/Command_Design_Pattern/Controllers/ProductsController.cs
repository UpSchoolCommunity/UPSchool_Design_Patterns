using Command_Design_Pattern.Commands;
using Command_Design_Pattern.Models;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Command_Design_Pattern.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppIdentityDbContext _appIdentityDbContext;

        public ProductsController(AppIdentityDbContext appIdentityDbContext)
        {
            _appIdentityDbContext = appIdentityDbContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _appIdentityDbContext.Products.ToListAsync());
        }

        public async Task<IActionResult> CreateFile(int type)
        {
            var products = await _appIdentityDbContext.Products.ToListAsync();

            FileCreateInvoker fileCreateInvoker = new();


            EFileType fileType = (EFileType)type;

            switch (fileType)
            {
                case EFileType.Excell:
                    ExcellFile<Product> excellFile = new(products);
                    fileCreateInvoker.SetCommand(new CreateExcellTableActionCommand<Product>(excellFile));
                    break;
                case EFileType.Pdf:
                    PdfFile<Product> pdfFile = new(products, HttpContext);
                    fileCreateInvoker.SetCommand(new CreatePdfTableActionCommand<Product>(pdfFile));
                    break;
                default:
                    break;
            }

            return fileCreateInvoker.CreateFile();
        }



        public async Task<IActionResult> CreateFiles()
        {
            var products = await _appIdentityDbContext.Products.ToListAsync();

            ExcellFile<Product> excellFile = new(products);
            PdfFile<Product> pdfFile = new(products, HttpContext);

            FileCreateInvoker fileCreateInvoker = new();

            fileCreateInvoker.AddCommand(new CreateExcellTableActionCommand<Product>(excellFile));
            fileCreateInvoker.AddCommand(new CreatePdfTableActionCommand<Product>(pdfFile));

            var fileResult = fileCreateInvoker.CreateFiles();

            using (var zipMemoryStream = new MemoryStream()) 
            {
                using (var archive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create)) 
                {
                    foreach (var item in fileResult)
                    {
                        var fileContent = item as FileContentResult;
                        var zipFile = archive.CreateEntry(fileContent.FileDownloadName);
                        using (var zipEntryStream = zipFile.Open()) 
                        {
                             await new MemoryStream(fileContent.FileContents).CopyToAsync(zipEntryStream);
                        }

                    }



                }


                return File(zipMemoryStream.ToArray(), "application/zip", "all.zip");
            }




        }
    }
}
