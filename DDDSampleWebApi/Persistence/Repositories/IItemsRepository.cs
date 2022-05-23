using DDDSampleWebApi.Models;

namespace DDDSampleWebApi.Persistence.Repositories;

public interface IItemsRepository
{
    Task<List<Items>> GetItemsAsync();
    Task AddItems(List<Items> items);
    int GetPointsById(int itemId);

}