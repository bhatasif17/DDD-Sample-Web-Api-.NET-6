using DDDSampleWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DDDSampleWebApi.Persistence.Repositories;

public class ItemsRepository :IItemsRepository
{
    private readonly ApiContext _ctx;

    public ItemsRepository(ApiContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<Items>> GetItemsAsync()
    {
       return  await _ctx.Items.ToListAsync();
    }

    public async Task AddItems(List<Items> items)
    {
        try
        {
            await _ctx.Items.AddRangeAsync(items);
            await _ctx.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public int GetPointsById(int itemId)
    {
       var item =  _ctx.Items.FirstOrDefault(o => o.Id == itemId);
       return item.Points;
    }
}