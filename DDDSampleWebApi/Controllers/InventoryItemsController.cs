using DDDSampleWebApi.Models;
using DDDSampleWebApi.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DDDSampleWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryItemsController : ControllerBase
{
    private readonly IItemsRepository _itemsRepository;

    public InventoryItemsController(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
       return Ok(await _itemsRepository.GetItemsAsync());
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(List<Items> items)
    {
        await _itemsRepository.AddItems(items);
        return Created($"{HttpContext.Request.Scheme}/{HttpContext.Request.Host}/",items);
    }
}