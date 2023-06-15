using Microsoft.AspNetCore.Mvc;
using ProductApi.Dto;
using ProductApi.Entities;
using ProductApi.Extensions;
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
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetAllAsync()
    {
	    var items = (await _itemRepository.GetAllAsync())
		    .Select(x => x.AsDto());
	    return Ok(items);
    }
    [HttpGet("/id")]
    public async Task<ActionResult<ItemDto>> GetAsync(Guid id)
    {
	    var item = await _itemRepository.GetAsync(id);
	    if (item == null)
	    {
		    return NotFound();
	    }

	    return item.AsDto();
    }
    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync([FromBody] CreateItemDto createItem)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        var item = new Item
        {
            ItemId = Guid.NewGuid(),
            Name = createItem.Name,
            Price = createItem.Price,
            CreatedDate = DateTimeOffset.Now
        };
        await _itemRepository.CreateItemAsync(item);
        return CreatedAtAction("GetAsync", new { id = item.ItemId }, item.AsDto());
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id,[FromBody] UpdateItemDto updateItem)
    {
	    if (!ModelState.IsValid) return BadRequest(updateItem);
        var existing = await _itemRepository.GetAsync(id);
        if (existing == null)
        {
	        return NotFound();
        }

        existing.Name = updateItem.Name;
        existing.Price = updateItem.Price;
        await _itemRepository.UpdateAsync(existing);
        return NoContent();
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
       var item = await _itemRepository.GetAsync(id);
       if(item is null) return NotFound();
       await _itemRepository.DeleteAsync(id);
       return NoContent();
    }
}