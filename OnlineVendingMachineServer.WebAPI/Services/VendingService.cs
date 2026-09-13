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
            vending.VendingImage = fileService.SaveFile(request.VendingImage, "VendingImages");
        vendingRepository.Add(vending);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Vending created successfully.");
    }

    public async Task<Result<string>> Delete(Guid vendingId, CancellationToken cancellationToken)
    {
        Vending vending = await vendingRepository.GetByExpressionAsync(v => v.Id == vendingId, cancellationToken);
        if (vending is null)
            return Result<string>.Failure("Vending not found.");
        vendingRepository.Delete(vending);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Vending deleted successfully.");
    }

    public async Task<Result<VendingGetDto>> Get(Guid vendingId, CancellationToken cancellationToken)
    {
        Vending vending = await vendingRepository.GetByExpressionAsync(v => v.Id == vendingId, cancellationToken);
        var vendingDto = vending.Adapt<VendingGetDto>();
        return Result<VendingGetDto>.Succeed(vendingDto);
    }

    public async Task<Result<IEnumerable<VendingGetAllDto>>> GetAll(CancellationToken cancellationToken)
    {
        IEnumerable<Vending> vendings = vendingRepository.GetAll();
        var vendingsDto = vendings.Select(v => v.Adapt<VendingGetAllDto>());
        return Result<IEnumerable<VendingGetAllDto>>.Succeed(vendingsDto);
    }

    public async Task<Result<string>> Update(VendingUpdateDto request, CancellationToken cancellationToken)
    {
        Vending vending = await vendingRepository.GetByExpressionAsync(v => v.Id == request.Id, cancellationToken);
        if (vending is null)
            return Result<string>.Failure("Vending not found.");
        request.Adapt(vending);
        if(request.VendingImage is not null)
            vending.VendingImage = fileService.UpdateFile(request.VendingImage, vending.VendingImage ?? "", "VendingImages");
        if(vendingRepository.Any(v => v.VendingSerialNumber == request.VendingSerialNumber))
            return Result<string>.Failure("Vending serial number already exists.");
        if(request.VendingSerialNumber is null || request.VendingSerialNumber == "")
            vending.VendingSerialNumber = generalService.GenerateSerialNumber("VENDING");
        vendingRepository.Update(vending);
        return Result<string>.Succeed("Vending updated successfully.");
    }
}
