using DDDSampleWebApi.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DDDSampleWebApi.Persistence;

public class SeedData
{
    // private readonly ApiContext _context;
    //
    // public SeedData(ApiContext context)
    // {
    //     _context = context;
    // }

    private void AddItems()
    {
        var list = new List<Items>();
        var item1 = new Items()
        {
            Id = 1, Name = "Water", Points = 4
        };
        var item2 = new Items()
        {
            Id = 2, Name = "Food", Points = 3
        };
        var item3 = new Items()
        {
            Id = 3, Name = "Medication", Points = 2
        };
        var item4 = new Items()
        {
            Id = 4, Name = "Stick", Points = 1
        };
        list.Add(item1);
        list.Add(item2);
        list.Add(item3);
        list.Add(item4);
    }
}