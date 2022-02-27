using AutoMapper;
using Moq;
using sample.tektonlabs.core.Handlers;
using sample.tektonlabs.core.Interfaces;
using sample.tektonlabs.core.Models;
using sample.tektonlabs.core.Profiles;
using sample.tektonlabs.core.Requests;
using System;
using Xunit;

namespace sample.tektonlabs.core.unittest;

public class UpdateProductHandlerTest
{
    private UpdateProductHandler CreateUpdateProductHandler(IMock<IRepository<Product>> repositoryMock)
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(typeof(CommonProfiles)));
        var mapperMock = new Mock<Mapper>(configuration);
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        return new UpdateProductHandler(unitOfWorkMock.Object, repositoryMock.Object, mapperMock.Object);
    }
    [Fact]
    public async void Handle_RequestIsnull_ThrowArgumentNullException()
    {
        UpdateProductRequest nullRequest = null;
        var productRepositoryMock = new Mock<IRepository<Product>>();
        var handler = CreateUpdateProductHandler(productRepositoryMock);

        var response = handler.Handle(nullRequest, default);

        await Assert.ThrowsAnyAsync<ArgumentNullException>(async () => await response);
        productRepositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Never);
        productRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async void Handle_KeyExists_UpdateAsync()
    {
        UpdateProductRequest anyRequestWithExistentKey = new() { Key = "AnyExistentKey" };
        var productRepositoryMock = new Mock<IRepository<Product>>();
        productRepositoryMock.Setup(x => x.GetAsync(It.Is<string>(s => s == anyRequestWithExistentKey.Key))).ReturnsAsync(new Product());
        var handler = CreateUpdateProductHandler(productRepositoryMock);

        var response = await handler.Handle(anyRequestWithExistentKey, default);


        productRepositoryMock.Verify(x => x.GetAsync(It.Is<string>(s => s == anyRequestWithExistentKey.Key)), Times.Once);
        productRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Product>(s => s.Key == anyRequestWithExistentKey.Key)), Times.Once);
    }
    [Fact]
    public async void Handle_KeyDoesNotExist_ThrowArgumentException()
    {
        UpdateProductRequest anyRequestWithNonExistentKey = new() { Key = "NonExistentKey" };
        var productRepositoryMock = new Mock<IRepository<Product>>();
        Product nonExistentProduct = null;
        productRepositoryMock.Setup(x => x.GetAsync(It.Is<string>(s => s == anyRequestWithNonExistentKey.Key))).ReturnsAsync(nonExistentProduct);
        var handler = CreateUpdateProductHandler(productRepositoryMock);

        var response = handler.Handle(anyRequestWithNonExistentKey, default);

        await Assert.ThrowsAnyAsync<ArgumentException>(async () => await response);
        productRepositoryMock.Verify(x => x.GetAsync(It.Is<string>(s => s == anyRequestWithNonExistentKey.Key)), Times.Once);
        productRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Product>(s => s.Key == anyRequestWithNonExistentKey.Key)), Times.Never);
    }
}

