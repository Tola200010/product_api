using Microsoft.AspNetCore.Mvc;
using ProductApi.Dto;
using ProductApi.Entities;
using ProductApi.Repositories.Contracts;

namespace ProductApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemRepository _itemRepository;

    public ItemController(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _itemRepository.GetAllAsync());
    }
    [HttpGet("/id")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        return Ok(await _itemRepository.GetAsyn(id));
    }
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CreateItemDto createItem)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        var item = new Item()
        {
            ItemId = Guid.NewGuid(),
            Name = createItem.name,
            Price = createItem.price,
            CreatedDate = DateTimeOffset.Now
        };
        await _itemRepository.CreateItemAsync(item);
        return Created(nameof(GetAsync),new{id = item.ItemId});
    }
    [HttpDelete("/id")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
       var item = await _itemRepository.GetAsyn(id);
       if(item is null) return BadRequest();
       await _itemRepository.DeleteAsync(id);
       return Ok(item);
    }
}