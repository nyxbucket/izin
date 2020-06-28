using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace izin.Controllers
{
    [Authorize(Roles = "Admin,Yonetici,Personel")]
    public abstract class BaseController : Controller
    {
        
    }
}