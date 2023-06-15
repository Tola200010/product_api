using System.ComponentModel.DataAnnotations;

namespace ProductApi.Dto;
public record CreateItemDto([Required] string Name,[Required] double Price);
public record ItemDto(Guid Id,string Name,double Price, DateTimeOffset CreatedDate);
public record UpdateItemDto([Required] string Name,[Required] double Price);