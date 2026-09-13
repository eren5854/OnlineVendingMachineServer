using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineVendingMachineServer.WebAPI.DTOs;
using OnlineVendingMachineServer.WebAPI.Services;

namespace OnlineVendingMachineServer.WebAPI.Controllers;

public sealed class VendingsController(
    IVendingService vendingService) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] VendingCreateDto request, CancellationToken cancellationToken)
    {
        var response = await vendingService.Create(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromForm] VendingUpdateDto request, CancellationToken cancellationToken)
    {
        var response = await vendingService.Update(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid vendingId, CancellationToken cancellationToken)
    {
        var response = await vendingService.Delete(vendingId, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get(Guid vendingId, CancellationToken cancellationToken)
    {
        var response = await vendingService.Get(vendingId, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await vendingService.GetAll(cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
