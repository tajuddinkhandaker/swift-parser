using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SwiftUpload.Models;
using System.Text.RegularExpressions;
using System.IO;

namespace SwiftUpload.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files == null || Request.Files.Count == 0)
            {
                string msg = String.Format("No files for upload! Count={0}", Request.Files.Count);
                //return Json(new { result = false, message = msg });
                ViewBag.Message = msg;
                return View();
            }
            string fileNames = "";
            foreach (string requestFile in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[requestFile];
                if (file == null || file.ContentLength == 0)
                {
                    string msg = String.Format("Invalid file upload! Length={0}", file.ContentLength);
                    //return Json(new { result = false, message = msg });
                    ViewBag.Message = msg;
                    return View();
                }
                string directory = Server.MapPath("~/App_Data/uploads/");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string fileName = Path.GetFileName(GetFileNameCrossBrowser(file));
                file.SaveAs(Path.Combine(directory, "swift_demo.txt"));
                fileNames += "\n" + fileName;
            }
            ViewBag.Message = fileNames;
            string data = System.IO.File.ReadAllText(SwiftParser.GetDocRoot());
            data = data.Trim();
            //string num_data = "Operator:(?<operator_name>.*?)\n+Number\\sof\\sentries:\\s+(?<num_tt>[0-9]+).*?\n+User selection:\\s+Selected Items";
            //new Regex(@"" + num_data, RegexOptions.Singleline | RegexOptions.IgnoreCase).Match(data).Groups["num_tt"].Value.Trim(' ', '\r', '.', '\n');
            //MatchCollection allTT = Regex.Matches(data, @"F59: Beneficiary Customer - Account - Name and Address", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //ViewBag.Doc = allTT.Count;
            ViewBag.Records = SwiftParser.ParseCSV();
            return View(SwiftParser.ParseCSV());
            //return Json(new { result = true, message = fileNames });
        }

        private string GetFileNameCrossBrowser(HttpPostedFileBase file)
        {
            // Checking for Internet Explorer  
            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
            {
                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                return testfiles[testfiles.Length - 1];
            }
            return file.FileName;
        }

        public ActionResult About()
        {
            return View();
        }

        private string ReadTestDataFile()
        {
            return System.IO.File.ReadAllText(SwiftParser.GetDocRoot()); 
        }
    }
}
