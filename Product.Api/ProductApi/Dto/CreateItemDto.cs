using System.ComponentModel.DataAnnotations;

namespace ProductApi.Dto;
public record CreateItemDto([Required] string name,[Required] double price);