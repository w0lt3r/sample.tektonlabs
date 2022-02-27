using AutoMapper;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using sample.tektonlabs.core.Handlers;
using sample.tektonlabs.core.Interfaces;
using sample.tektonlabs.core.Models;
using sample.tektonlabs.core.Profiles;
using System;
using System.Threading.Tasks;
using Xunit;

namespace sample.tektonlabs.core.unittest
{
    public class GetProductByKeyHandlerTest
    {
        private GetProductByKeyHandler CreateGetProductByKeyHandler(IMock<IAppCache> appCacheMock, IMock<IRepository<Product>> repositoryMock, IMock<IExternalPriceProvider> externalPriceProviderMock)
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(typeof(CommonProfiles)));
            var mapperMock = new Mock<Mapper>(configuration);
            return new GetProductByKeyHandler(
                appCacheMock.Object,
                repositoryMock.Object,
                externalPriceProviderMock.Object,
                mapperMock.Object
                );
        }
        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async void Handle_KeyIsNurllOrWhiteSpace_ThrowNullArgumentException(string key)
        {
            var appCacheMock = new Mock<IAppCache>();
            var productRepositoryMock = new Mock<IRepository<Product>>();
            var externalPriceProviderMock = new Mock<IExternalPriceProvider>();
            var handler = CreateGetProductByKeyHandler(appCacheMock, productRepositoryMock, externalPriceProviderMock);

            var response = handler.Handle(new() { Key = key }, default);

            await Assert.ThrowsAnyAsync<ArgumentNullException>(async () => await response);
            appCacheMock.Verify(c => c.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, Task<Product>>>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Never);
            productRepositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Never);
            externalPriceProviderMock.Verify(x => x.GetPrice(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async void Handle_ProductNotFound_ReturnNull()
        {
            var nonValidKey = "nonValidKey";
            var appCacheMock = new Mock<IAppCache>();
            Product nullProduct = null;
            appCacheMock.Setup(x => x.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, Task<Product>>>(), It.IsAny<MemoryCacheEntryOptions>())).ReturnsAsync(nullProduct);
            var productRepositoryMock = new Mock<IRepository<Product>>();
            var externalPriceProviderMock = new Mock<IExternalPriceProvider>();
            var handler = CreateGetProductByKeyHandler(appCacheMock, productRepositoryMock, externalPriceProviderMock);

            var response = await handler.Handle(new() { Key = nonValidKey }, default);

            appCacheMock.Verify(c => c.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, Task<Product>>>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
            externalPriceProviderMock.Verify(x => x.GetPrice(It.IsAny<string>()), Times.Never);
        }
        [Fact]
        public async void Handle_ProductFound_ReturnProduct()
        {
            var validKey = "validKey";
            var appCacheMock = new Mock<IAppCache>();
            Product product = new Product { Key = validKey };
            appCacheMock.Setup(x => x.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, Task<Product>>>(), It.IsAny<MemoryCacheEntryOptions>())).ReturnsAsync(product);
            var productRepositoryMock = new Mock<IRepository<Product>>();
            var externalPriceProviderMock = new Mock<IExternalPriceProvider>();
            double anyValidPrice = 12.3;
            externalPriceProviderMock.Setup(x => x.GetPrice(It.IsAny<string>())).ReturnsAsync(anyValidPrice);
            var handler = CreateGetProductByKeyHandler(appCacheMock, productRepositoryMock, externalPriceProviderMock);

            var response = await handler.Handle(new() { Key = validKey }, default);

            appCacheMock.Verify(c => c.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, Task<Product>>>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
            externalPriceProviderMock.Verify(x => x.GetPrice(It.IsAny<string>()), Times.Once);
            Assert.Equal(anyValidPrice, response.Price);
        }
    }
}