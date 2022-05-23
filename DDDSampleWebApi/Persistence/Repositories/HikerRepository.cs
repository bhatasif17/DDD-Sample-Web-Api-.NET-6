using DDDSampleWebApi.DTOs;
using DDDSampleWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DDDSampleWebApi.Persistence.Repositories;

public sealed class HikerRepository : IHikerRepository, IDisposable
{
    private readonly ApiContext _ctx;
    private bool _disposed = false;

    public HikerRepository(ApiContext ctx)
    {
        _ctx = ctx;
    }
    public async Task RegisterHiker(Hiker hiker)
    {
        try
        {
            await _ctx.Hikers.AddAsync(hiker);
        }
        catch (Exception e)
        {
            throw;
        }
    }
    public Task<List<Hiker>> GetAllHikers()
    {
        return _ctx.Hikers
           .Include(o=>o.Location)
           .Include(o=>o.Items)
           .ToListAsync();
    }

    public async Task<Hiker?> GetById(uint id)
    {
       return await _ctx.Hikers
           .Include(o=>o.Location)
           .Include(o=>o.Items)
           .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<bool> HikerExists(string username)
    {
       return await _ctx.Hikers.AnyAsync(o => o.Username == username);
    }

    public async void Save()
    {
        try
        {
            await _ctx.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async void UpdateLocation(float longitude, float latitude, string secret)
    {
        var hiker = await _ctx.Hikers
            .Include(o=>o.Location)
            .FirstOrDefaultAsync(o => o.Secret == secret);
        
        hiker.Location.Latitude = latitude;
        hiker.Location.Longitude = longitude;
        
        _ctx.Update(hiker);
    }

    public async void ReportHiker(string username, string reportedBy, string illness)
    {
        var rhiker = new ReportedHiker();
        var reporter = _ctx.Hikers.FirstOrDefault(o => o.Secret == reportedBy).Id;
        var hiker = await _ctx.Hikers.FirstOrDefaultAsync(o => o.Username == username);

        hiker.ReportCount++;
        if (hiker.ReportCount >= 3)
            hiker.IsIll = true;
        
        rhiker.Illness = illness;
        rhiker.ReportedBy = reporter;
        rhiker.Hiker = hiker;
        rhiker.Date = DateTime.Now;

        await _ctx.AddAsync(rhiker);
        _ctx.Update(hiker);
    }

    public bool AlreadyReported(string username, string reportedBy)
    {
        var r = _ctx.Hikers.FirstOrDefault(o => o.Secret == reportedBy).Id;
        return _ctx.ReportedHikers.Any(o => o.ReportedBy == r && o.Hiker.Username == username);
    }

    public bool SelfReport(string username, string reportedBy)
    {
        var r = _ctx.Hikers.FirstOrDefault(o => o.Secret == reportedBy);
        return r.Username == username;
    }

    public async Task TradeItems(TradeItemsDto dto)
    {
        var fromHiker = await _ctx.Hikers
            .Include(o=>o.Items)
            .FirstOrDefaultAsync(o => o.Username == dto.From.Username);
        var ToHiker = await _ctx.Hikers
            .Include(o=>o.Items)
            .FirstOrDefaultAsync(o => o.Username == dto.To.Username);
        
        foreach (var item in dto.From.TradeItems)
        {
            var fhi = ToHiker.Items.FirstOrDefault(o => o.ItemId == item.Id);
            if(fhi is not null)
                fhi.Quantity += item.Quantity;
            else
            {
                var iss = new Item
                {
                    ItemId = item.Id,
                    Quantity = item.Quantity
                };

                ToHiker.Items.Add(iss);
                _ctx.Update(ToHiker);
            }
            
            var thi = fromHiker.Items.FirstOrDefault(o => o.ItemId == item.Id);
            thi.Quantity -= item.Quantity;
        }
        foreach (var item in dto.To.TradeItems)
        {
            var fhi = fromHiker.Items.FirstOrDefault(o => o.ItemId == item.Id);
            if(fhi is not  null)
               fhi.Quantity += item.Quantity;
            else
            {
                var iss = new Item
                {
                    ItemId = item.Id,
                    Quantity = item.Quantity
                };

                fromHiker.Items.Add(iss);
                _ctx.Update(fromHiker);
            }
            
            var thi = ToHiker.Items.FirstOrDefault(o => o.ItemId == item.Id);
            thi.Quantity -= item.Quantity;
        }
    }

    private void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                _ctx.Dispose();
            }
        }
        this._disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}