using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// Home
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}