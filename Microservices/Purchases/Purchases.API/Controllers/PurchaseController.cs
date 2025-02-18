using Infrastructure.Masstransit;
using Infrastructure.Masstransit.Purchases.Requests;
using Infrastructure.Masstransit.Purchases.Responses;
using Infrastructure.Models;
using Infrastructure.Models.Identity;
using Infrastructure.Models.Purchases;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Purchases.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseController(IBusControl busController) : ControllerBase
{
    private Uri _rabbitMqPurchaseUri = new Uri($"queue:{RabbitQueueNames.PURCHASES}");

    [HttpPost("get-transactions")]
    [Authorize]
    public async Task<IActionResult> GetTransactions([FromBody] GetTransactionsRequest request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<GetTransactionsRequest, TransactionsResponse>(request, busController, _rabbitMqPurchaseUri);
        return Ok(ApiResult<TransactionsResponse>.Success200(res));
    }

    [HttpPost("add-transaction")]
    [Authorize]
    public async Task<IActionResult> AddTransaction([FromBody] AddTransactionRequest request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<AddTransactionRequest, BaseMasstransitResponse>(request, busController, _rabbitMqPurchaseUri);
        return Ok(ApiResult<BaseMasstransitResponse>.Success200(res));
    }

    [HttpPost("get-transaction-by-id")]
    [Authorize]
    public async Task<IActionResult> GetTransactionById([FromBody] GetTransactionByIdRequest request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<GetTransactionByIdRequest, Transaction>(request, busController, _rabbitMqPurchaseUri);
        
        return Ok(ApiResult<Transaction>.Success200(res));
    }

    [HttpPut("update-transaction")]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> UpdateTransaction([FromBody] UpdateTransactionRequest request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<UpdateTransactionRequest, BaseMasstransitResponse>(request, busController, _rabbitMqPurchaseUri);
        
        return Ok(ApiResult<BaseMasstransitResponse>.Success200(res));
    } 
}