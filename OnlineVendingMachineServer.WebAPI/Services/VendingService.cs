using ED.GenericRepository;
using ED.Result;
using Mapster;
using OnlineVendingMachineServer.WebAPI.DTOs;
using OnlineVendingMachineServer.WebAPI.Models;
using OnlineVendingMachineServer.WebAPI.Repositories;

namespace OnlineVendingMachineServer.WebAPI.Services;

public sealed class VendingService(
    IVendingRepository vendingRepository,
    IUnitOfWork unitOfWork,
    IGeneralService generalService,
    IFileService fileService) : IVendingService
{
    public async Task<Result<string>> Create(VendingCreateDto request, CancellationToken cancellationToken)
    {
        Vending vending = request.Adapt<Vending>();
        vending.VendingSerialNumber = generalService.GenerateSerialNumber("VENDING");
        if (request.VendingImage is not null)
        {
            vending.VendingImage = fileService.SaveFile(request.VendingImage, "VendingImages");
        }
        vendingRepository.Add(vending);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Vending created successfully.");
    }

    public Task<Result<string>> Delete(Guid vendingId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<VendingGetDto>> Get(Guid vendingId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<VendingGetAllDto>>> GetAll(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> Update(VendingUpdateDto request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
