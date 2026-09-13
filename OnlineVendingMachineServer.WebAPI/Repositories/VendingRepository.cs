using ED.GenericRepository;
using OnlineVendingMachineServer.WebAPI.Context;
using OnlineVendingMachineServer.WebAPI.Models;

namespace OnlineVendingMachineServer.WebAPI.Repositories;

public class VendingRepository : Repository<Vending, ApplicationDbContext>, IVendingRepository
{
    public VendingRepository(ApplicationDbContext context) : base(context)
    {
    }
}
