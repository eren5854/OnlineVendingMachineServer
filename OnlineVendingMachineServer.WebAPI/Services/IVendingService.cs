using ED.Result;
using OnlineVendingMachineServer.WebAPI.DTOs;

namespace OnlineVendingMachineServer.WebAPI.Services;

public interface IVendingService
{
    Task<Result<string>> Create(VendingCreateDto request, CancellationToken cancellationToken);
    Task<Result<string>> Update(VendingUpdateDto request, CancellationToken cancellationToken);
    Task<Result<string>> Delete(Guid vendingId, CancellationToken cancellationToken);
    Task<Result<VendingGetDto>> Get(Guid vendingId, CancellationToken cancellationToken);
    Task<Result<IEnumerable<VendingGetAllDto>>> GetAll(CancellationToken cancellationToken);
}
