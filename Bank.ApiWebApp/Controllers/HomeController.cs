using Microsoft.AspNetCore.Mvc;

namespace Bank.ApiWebApp.Controllers;

/// <summary>
/// Контроллер по умолчанию
/// </summary>
[Route("")]
public class HomeController : Controller
{
    /// <summary>
    /// Метод по умолчанию
    /// </summary>
    /// <returns>Результат обработки запроса</returns>
    [Route("")]
    public ActionResult Index() => Redirect("/help");
}
