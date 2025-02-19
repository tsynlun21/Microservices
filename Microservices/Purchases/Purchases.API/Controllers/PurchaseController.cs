using Infrastructure.Masstransit;
using Infrastructure.Masstransit.Purchases.Requests;
using Infrastructure.Models;
using Infrastructure.Models.Identity;
using Infrastructure.Models.Purchases;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Purchases.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseController(IBusControl busController) : ControllerBase
{
    private Uri _rabbitMqPurchaseUri = new Uri($"queue:{RabbitQueueNames.PURCHASES}");

    [HttpGet("get-transactions")]
    public async Task<IActionResult> GetTransactions()
    {
        var userFromContext = HttpContext.Items["User"] as UserModel;

        var res = await RabbitWorker.GetRabbitMessageResponse<GetTransactionsRequest, Transaction[]>(
            new GetTransactionsRequest()
            {
                user = userFromContext!
            }, busController, _rabbitMqPurchaseUri);

        return Ok(ApiResult<Transaction[]>.Success200(res));
    }

    [HttpPost("add-transaction")]
    public async Task<IActionResult> AddTransaction([FromBody] Transaction request)
    {
        var userFromContext = HttpContext.Items["User"] as UserModel;
        var res = await RabbitWorker.GetRabbitMessageResponse<AddTransactionRequest, Transaction>(new()
        {
            Transaction = request,
            User        = userFromContext!
        }, busController, _rabbitMqPurchaseUri);
        
        return Ok(ApiResult<Transaction>.Success200(res));
    }

    [HttpGet("get-transaction-by-id")]
    public async Task<IActionResult> GetTransactionById([FromRoute] long id)
    {
        var userFromContext = HttpContext.Items["User"] as UserModel;
        var res = await RabbitWorker.GetRabbitMessageResponse<GetTransactionByIdRequest, Transaction>(new()
        {
            User          = userFromContext!,
            TransactionId = id
        }, busController, _rabbitMqPurchaseUri);

        return Ok(ApiResult<Transaction>.Success200(res));
    }

    [HttpPut("update-transaction")]
    public async Task<IActionResult> UpdateTransaction([FromBody] UpdateTransaction request)
    {
        var userFromContext = HttpContext.Items["User"] as UserModel;
        var res = await RabbitWorker.GetRabbitMessageResponse<UpdateTransactionRequest, Transaction>(new()
        {
            User        = userFromContext!,
            Transaction = request,
        }, busController, _rabbitMqPurchaseUri);

        return Ok(ApiResult<Transaction>.Success200(res));
    }
}