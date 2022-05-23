using DDDSampleWebApi.DTOs;
using DDDSampleWebApi.Models;

namespace DDDSampleWebApi.Persistence.Repositories;

public interface IHikerRepository :IDisposable
{
    Task RegisterHiker(Hiker hiker);
    Task<List<Hiker>> GetAllHikers();
    Task<Hiker?> GetById(uint id);
    Task<bool> HikerExists(string username);
    void Save();
    void UpdateLocation(float longitude, float latitude, string secret);
    void ReportHiker(string username, string reportedBy, string illness);
    bool AlreadyReported(string username, string reportedBy);
    bool SelfReport(string username, string reportedBy);
    Task TradeItems(TradeItemsDto dto);
}