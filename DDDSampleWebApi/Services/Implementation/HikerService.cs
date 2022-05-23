using DDDSampleWebApi.DTOs;
using DDDSampleWebApi.Models;
using DDDSampleWebApi.Persistence.Repositories;
using DDDSampleWebApi.Services.Interfaces;
using System.Linq;

namespace DDDSampleWebApi.Services.Implementation;

public class HikerService : IHikerService
{
    private readonly IHikerRepository _hikerRepository;
    private readonly IItemsRepository _itemsRepository;

    public HikerService(IHikerRepository hikerRepository, IItemsRepository itemsRepository)
    {
        _hikerRepository = hikerRepository;
        _itemsRepository = itemsRepository;
    }

    public async Task RegisterHiker(Hiker hiker)
    {
        await _hikerRepository.RegisterHiker(hiker);
        _hikerRepository.Save();
    }

    public async Task TradeItems(TradeItemsDto dto)
    {
        var fromitemList = dto.From.TradeItems;
        var toItemList = dto.To.TradeItems;

        int fromPoints = fromitemList.Sum(item => _itemsRepository.GetPointsById(item.Id) * item.Quantity);
        int toPoints = toItemList.Sum(item => _itemsRepository.GetPointsById(item.Id) * item.Quantity);

        if(fromPoints == toPoints)
           await _hikerRepository.TradeItems(dto);
    }

    public async Task<ReportDto> GenerateReport()
    { 
        var dto = new ReportDto();
       var hikers = await _hikerRepository.GetAllHikers();
       var totalCount = hikers.Count;
       var injuredHikers = hikers.Count(o => o.IsIll == true);
       dto.InjuredHikers = (double) (injuredHikers / (double) totalCount) * 100;
       dto.NonInjuredHikers = (double) ((totalCount - injuredHikers) /(double) totalCount) * 100;

       foreach (var x in hikers.Where(x => x.IsIll == true))
       {
           dto.LostPoints = x.Items.Sum(o => o.Quantity * _itemsRepository.GetPointsById(o.ItemId));
       }

       var items = await _itemsRepository.GetItemsAsync();

       var dict = new Dictionary<int, decimal>();
       
           foreach (var i in items)
           {
               var d = hikers.Aggregate(0M, (current, x) => current + x.Items.Where(o => o.ItemId == i.Id).Sum(o => o.Quantity));
               dict.Add(i.Id, d);
           }
           dto.AverageItem = dict;

           return dto;
    }

    public async Task<List<Hiker>> GetAllHikers()
    {
       return await _hikerRepository.GetAllHikers();
    }

    public async Task<Hiker?> GetById(uint id)
    {
        return await _hikerRepository.GetById(id);
    }

    public void Report(string username, string reportedBy, string illness)
    {
        _hikerRepository.ReportHiker(username, reportedBy, illness);
    }
}