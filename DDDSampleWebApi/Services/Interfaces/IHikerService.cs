using DDDSampleWebApi.DTOs;
using DDDSampleWebApi.Models;

namespace DDDSampleWebApi.Services.Interfaces;

public interface IHikerService
{
    /// <summary>
    /// Register a hiker
    /// </summary>
    Task RegisterHiker(Hiker hiker);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tradeItemsDto"></param>
    Task TradeItems(TradeItemsDto tradeItemsDto);
    /// <summary>
    /// 
    /// </summary>
    Task<ReportDto> GenerateReport();
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<Hiker>> GetAllHikers();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Hiker?> GetById(uint id);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hiker"></param>
    /// <param name="reportedBy"></param>
    /// <param name="illness"></param>
    void Report(string username, string reportedBy, string illness);
}