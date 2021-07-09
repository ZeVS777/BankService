using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
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
        /// <response code="403">Предоставленному ApiId не разрешено выполнить данный запрос.</response>
        /// <response code="429">Слишком много запросов.</response>
        [Produces(typeof(ApiResult<SendResultModel>))]
        [ProducesResponseType(400, Type = typeof(ApiResult<IEnumerable<ValidationError>>))]
        [ProducesResponseType(403, Type = typeof(ApiResult))]
        [ProducesResponseType(429, Type = typeof(ApiResult))]
        [HttpPost("send")]
        public async Task<ActionResult> Send(
            [FromServices] ISmsService smsService,
            [Required(ErrorMessage = "Unknown format.")][InviteMessageValidation] InviteMessage message)
        {
            Debug.Assert(message.ApiId.HasValue);

            var (status, remains, sentMessage) = await smsService.SendAsync(new InviteMessageModel(message.Phones, message.Message, message.ApiId.Value));

            return status switch
            {
                SendStatus.Ok => Ok(new ApiResult<SendResultModel>(new SendResultModel(remains, sentMessage!))),
                SendStatus.TooMany when remains == 0 => StatusCode(StatusCodes.Status429TooManyRequests),
                SendStatus.TooMany => NotEnoughRemains(remains),
                SendStatus.Forbidden => StatusCode(StatusCodes.Status403Forbidden),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private ActionResult NotEnoughRemains(int remains)
        {
            ModelState.AddModelError("message", PhoneValidationErrorMessages.TooMuchPhoneNumbersPerDay);
            ModelState.AddModelError("message", $"You have {remains} messages remains");
            return BadRequest(new ApiResult<IEnumerable<ValidationError>>(
                ValidationError.GetValidationErrors(ControllerContext), "Bad Request"));
        }
    }
}