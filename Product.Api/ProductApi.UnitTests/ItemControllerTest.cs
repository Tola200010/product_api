using Castle.Components.DictionaryAdapter;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApi.Controllers;
using ProductApi.Dto;
using ProductApi.Entities;
using ProductApi.Repositories.Contracts;
using Xunit;
using Xunit.Abstractions;

namespace ProductApi.UnitTests;

public class ItemControllerTest
{
	private readonly ITestOutputHelper _testOutputHelper;
	private readonly Mock<IItemRepository> _itemRepositoryMock = new();
	private readonly Random _random = new Random();

	public ItemControllerTest(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}

	[Fact]
	public async Task UnitOfWork_StateUnderTest_ReturnNotFound()
	{
		// arrange
		_itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Item)null!);
		var controller = new ItemController(_itemRepositoryMock.Object);
		// act
		var result = await controller.GetAsync(Guid.NewGuid());
		// asset
		Assert.IsType<NotFoundResult>(result.Result);
	}
	[Fact]
	public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
	{
		var expectedItem = CreateRandomItem();
		_itemRepositoryMock.Setup(repo=>repo.GetAsync(It.IsAny<Guid>()))
			.ReturnsAsync(expectedItem);
		var controller = new ItemController(_itemRepositoryMock.Object);
		var result = await controller.GetAsync(Guid.NewGuid());
		result.Value.Should().BeEquivalentTo(
			expectedItem,
			options=>
				options
					.WithMapping<ItemDto>(
					e=>e.ItemId,s=>s.Id)
					.WithMapping<ItemDto>(e=>e.Name,s=>s.Name)
					.WithMapping<ItemDto>(e=>e.Price,s=>s.Price)
					.WithMapping<ItemDto>(e=>e.CreatedDate,s=>s.CreatedDate)
			);
	}
	[Fact]
	public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
	{
		var expectedItems = new[]
		{
			CreateRandomItem(),
			CreateRandomItem(),
			CreateRandomItem()
		};
		_itemRepositoryMock.Setup(repo => repo.GetAllAsync())
			.ReturnsAsync(expectedItems);
		var controller = new ItemController(_itemRepositoryMock.Object);
		var actionResult = await controller.GetAllAsync();
		var okObjectResult = actionResult.Result as OkObjectResult;
		var actualItems = okObjectResult!.Value as IEnumerable<ItemDto>;
		Assert.Equal(expectedItems.Length, actualItems.Count());
		foreach (var expectedItem in expectedItems)
		{
			var actualItem = actualItems.FirstOrDefault(x => x.Id == expectedItem.ItemId);
			Assert.NotNull(actualItem);
			Assert.Equal(expectedItem.Name, actualItem.Name);
			Assert.Equal(expectedItem.Price, actualItem.Price);
			Assert.Equal(expectedItem.CreatedDate, actualItem.CreatedDate);
		}	
	}
	[Fact]
	public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
	{
		var createItem = new CreateItemDto(Guid.NewGuid().ToString(), _random.Next(1000));
		var controller = new ItemController(_itemRepositoryMock.Object);
		var actionResult = await controller.PostAsync(createItem);
		var createdAtActionResult = actionResult.Result as CreatedAtActionResult;
		Assert.NotNull(createdAtActionResult);
		Assert.Equal("GetAsync", createdAtActionResult.ActionName);
		var createdItem = createdAtActionResult.Value as ItemDto;
		Assert.NotNull(createdItem);
		createdItem.Id.Should().NotBeEmpty();
		createdItem.Name.Should().BeEquivalentTo(createItem.Name);
		createdItem.Price.Should().Be(createItem.Price);
		createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromMilliseconds(1000));
	}
	[Fact]
	public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
	{
		var existingItem = CreateRandomItem();
		_itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
			.ReturnsAsync(existingItem);
		var itemId = existingItem.ItemId;
		var itemToUpdate = new UpdateItemDto(Guid.NewGuid().ToString(), existingItem.Price + 3);
		var controller = new ItemController(_itemRepositoryMock.Object);
		var actionResult = await controller.UpdateAsync(itemId,itemToUpdate);
		actionResult.Should().BeOfType<NoContentResult>();
	}
	[Fact]
	public async Task DeleteItemAsync_WithExistingItem_ReturnNoContent()
	{
		var existingItem = CreateRandomItem();
		_itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
			.ReturnsAsync(existingItem);
		var itemId = existingItem.ItemId;
		var controller = new ItemController(_itemRepositoryMock.Object);
		var actionResult = await controller.DeleteAsync(itemId);
		actionResult.Should().BeOfType<NoContentResult>();
	}
	private Item CreateRandomItem()
	{
		return new Item
		{
			ItemId = Guid.NewGuid(),
			Name = Guid.NewGuid().ToString(),
			Price = _random.Next(1000),
			CreatedDate = DateTimeOffset.Now
		};
	}
}