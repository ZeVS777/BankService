using Microsoft.AspNetCore.Mvc;

namespace SovComBankTest.ApiWebApp.Controllers
{
    [Route("")]
    public class HomeController: Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return Redirect("/help");
        }
    }
}