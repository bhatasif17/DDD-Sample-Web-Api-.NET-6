using DDDSampleWebApi.Models;

namespace DDDSampleWebApi.DTOs;

public class TradeItemsDto
{
    public FromHiker From  { get; set; }
    public ToHiker To { get; set; }
}

public class ToHiker
{
    public string Username { get; set; }
    public List<TradeItem> TradeItems { get; set; }
}
public class FromHiker
{
    public string Username { get; set; }
    public List<TradeItem> TradeItems { get; set; }
}
public class TradeItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
}