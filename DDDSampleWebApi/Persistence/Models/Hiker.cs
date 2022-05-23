using System.ComponentModel.DataAnnotations.Schema;

namespace DDDSampleWebApi.Models;

public class Hiker
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Age { get; set; }
    public char Gender { get; set; }
    public string Username { get; set; }
    public string Secret { get; set; }
    public bool IsIll { get; set; }
    public int ReportCount { get; set; }
    public DateTime Date { get; set; }
    public Location Location { get; set; }
    public List<Item> Items { get; set; }

    public Hiker()
    {
        Items = new List<Item>();
        Date = DateTime.Now;
    }
}

public class Item
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}

public class Location
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}

public class Items
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Points { get; set; }
}

public class ReportedHiker
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public Hiker Hiker { get; set; }
    public int ReportedBy { get; set; }
    public string Illness { get; set; }
    public DateTime Date { get; set; }
}