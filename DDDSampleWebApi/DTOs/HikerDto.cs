namespace DDDSampleWebApi.DTOs;

public class HikerDto
{
    public string Name { get; set; }
    public string Age { get; set; }
    public char Gender { get; set; }
    public string Username { get; set; }
    public LocationDto Location { get; set; }
    public List<ItemDto> Items { get; set; }

    public HikerDto()
    {
        Items = new List<ItemDto>();
    }
}
public class ItemDto
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}

public class LocationDto
{
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}