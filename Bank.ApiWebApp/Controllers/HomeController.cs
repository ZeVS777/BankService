using Microsoft.AspNetCore.Mvc;

namespace Bank.ApiWebApp.Controllers
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