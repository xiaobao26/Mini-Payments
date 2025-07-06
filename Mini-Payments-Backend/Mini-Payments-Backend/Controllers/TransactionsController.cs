using Microsoft.AspNetCore.Mvc;
using Mini_Payments_Backend.Dtos.Request;
using Mini_Payments_Backend.Dtos.Response;
using Mini_Payments_Backend.Services;

namespace Mini_Payments_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController: ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
    {
        _transactionService = transactionService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<TransactionResonseDto>> CreateTransaction(Guid accountId, CreateTransactionDto dto)
    {
        try
        {
            var res = await _transactionService.CreateTransactionAsync(accountId, dto);

            return CreatedAtAction(nameof(GetAllTransactionByAccountId), new { id = res.Id }, res);
        }
        catch (Exception ex)
        {
            _logger.LogError( ex, "Error creating transaction");
            return StatusCode(500, "Unhandled error occured");
        }
    }

    [HttpGet]
    public async Task<ActionResult<TransactionResonseDto>> GetAllTransactionByAccountId(Guid accountId)
    {
        try
        {
            var list = _transactionService.GetAllTransactionsByAccountId(accountId);
            return Ok(list);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Unhandled error occured");
        }
    }
}