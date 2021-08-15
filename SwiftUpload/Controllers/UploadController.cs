using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SwiftUpload.Models;

namespace SwiftUpload.Controllers
{
    public class UploadController : Controller
    {
        //
        // GET: /Upload/

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
