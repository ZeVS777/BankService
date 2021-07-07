using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SovComBankTest.ApiWebApp.Models;
using SovComBankTest.Services.Abstractions;
using SovComBankTest.Services.Models;

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
        /// <param name="smsService">SMS сервис</param>
        /// <param name="message">Запрос с номерами телефонов и сообщением указанным адресатам</param>
        /// <returns>Результат запроса.</returns>
        /// <response code="200">Успешный результат.</response>
        /// <response code="400">Неверный запрос.</response>
        /// <response code="401">Не предоставлен ApiId.</response>
        /// <response code="403">Предоставленному ApiId не разрешено выполнить данный запрос.</response>
        /// <response code="429">Слишком много запросов.</response>
        [Produces(typeof(ApiResult))]
        [ProducesResponseType(400, Type = typeof(ApiResult<IEnumerable<ValidationError>>))]
        [ProducesResponseType(401, Type = typeof(ApiResult))]
        [ProducesResponseType(403, Type = typeof(ApiResult))]
        [ProducesResponseType(429, Type = typeof(ApiResult))]
        [HttpPost("send")]
        public ActionResult Send(
            [FromServices] ISmsService smsService,
            [Required(ErrorMessage = "Unknown Format.")][InviteMessageValidation] InviteMessage message)
        {
            if (message.ApiId == null) return StatusCode(StatusCodes.Status401Unauthorized, new ApiResult("Provide ApiId."));
            
            Debug.Assert(message.Phones != null);
            Debug.Assert(message.Message != null);

            var sendStatus = smsService.Send(new InviteMessageModel(message.Phones, message.Message, message.ApiId.Value));

            return sendStatus switch
            {
                SendStatus.Ok => Ok(),
                SendStatus.TooMany => StatusCode(StatusCodes.Status429TooManyRequests),
                SendStatus.Forbidden => StatusCode(StatusCodes.Status403Forbidden),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}