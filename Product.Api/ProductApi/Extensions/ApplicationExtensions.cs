using ProductApi.Dto;
using ProductApi.Entities;

namespace ProductApi.Extensions;

public static class ApplicationExtensions
{
	public static ItemDto AsDto(this Item item)
	{
		var itemDto = new ItemDto(item.ItemId, item.Name ?? "", item.Price, item.CreatedDate);
		return itemDto;
	}
}