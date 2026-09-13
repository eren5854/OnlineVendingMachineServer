using ED.GenericRepository;
using OnlineVendingMachineServer.WebAPI.Context;
using OnlineVendingMachineServer.WebAPI.Models;

namespace OnlineVendingMachineServer.WebAPI.Repositories;

public class VendingSlotRepository : Repository<VendingSlot, ApplicationDbContext>, IVendingSlotRepository
{
    public VendingSlotRepository(ApplicationDbContext context) : base(context)
    {
    }
}
