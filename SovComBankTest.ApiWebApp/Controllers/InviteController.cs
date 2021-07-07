using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SovComBankTest.ApiWebApp.Models;

namespace SovComBankTest.ApiWebApp.Controllers
{
    /// <summary>
    ///     API СМС-приглашений.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/invite")]
    public class InviteController: ControllerBase
    {
        /// <summary>
        ///     Отправка СМС-приглашения на список телефонных номеров для подключения к Системе.
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST /api/invite/send/
        ///     {
        ///         "phones": ["79998887766", "75554443322"],
        ///         "message": "Hello",
        ///         "apiId": 4
        ///     }
        /// 
        /// </remarks>
        /// <param name="message">Запрос с номерами телефонов и сообщением указанным адресатам</param>
        /// <returns>Результат запроса.</returns>
        /// <response code="200">Успешный результат.</response>
        /// <response code="400">Неверный запрос.</response>
        /// <response code="429">Слишком много запросов.</response>
        [Produces(typeof(ApiResult))]
        [ProducesResponseType(400, Type = typeof(ApiResult<IEnumerable<ValidationError>>))]
        [ProducesResponseType(429, Type = typeof(ApiResult))]
        [HttpPost("send")]
        public ActionResult Send([Required(ErrorMessage = "Unknown Format")][InviteMessageValidation] InviteMessage message)
        {
            if (message.ApiId != 4)
                return StatusCode(401);

            //TODO: Check 429

            //TODO: Send and Save

            return Ok();
        }
    }
}