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
		var actualItems = await controller.GetAllAsync();
		actualItems.Result.Should().BeOfType<OkResult>();
		actualItems.Should().BeEquivalentTo(
			expectedItems
			// First item
		);
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