using Microsoft.AspNetCore.Mvc;
using Mini_Payments_Backend.Dtos.Request;
using Mini_Payments_Backend.Dtos.Response;
using Mini_Payments_Backend.Services;

namespace Mini_Payments_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController: ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountsController> _logger;
    public AccountsController(IAccountService accountService, ILogger<AccountsController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<ActionResult<AccountResponseDto>> CreateAccount([FromBody] CreateAccountDto dto)
    {
        try
        {
            var res = await _accountService.CreateAccountAsync(dto);
            return CreatedAtAction(nameof(GetAccountById), new { id = res.Id }, res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating account");
            return StatusCode(500, "Unhandled error occured");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AccountResponseDto>> GetAccountById(Guid id)
    {
        try
        {
            var res = await _accountService.GetAccountByIdAsync(id);

            if (res == null)
            {
                return NotFound();
            }

            return Ok(res);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Unhandled error occured");
        }
    }
}