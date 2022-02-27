using AutoMapper;
using LazyCache;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using sample.tektonlabs.core.Interfaces;
using sample.tektonlabs.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample.tektonlabs.core.Handlers
{
    public class GetProductByKeyRequest : IRequest<ProductResponse>
    {
        public string Key { get; set; }
    }
    public class GetProductByKeyHandler : IRequestHandler<GetProductByKeyRequest, ProductResponse>
    {
        private readonly IAppCache _appCache;
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _repository;
        private readonly IExternalPriceProvider _externalPriceProvider;
        public GetProductByKeyHandler(IAppCache appCache, IRepository<Product> repository, IExternalPriceProvider externalPriceProvider, IMapper mapper)
        {
            _appCache = appCache;
            _repository = repository;
            _externalPriceProvider = externalPriceProvider;
            _mapper = mapper;
        }

        public async Task<ProductResponse> Handle(GetProductByKeyRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Key)) throw new ArgumentNullException(nameof(request.Key));
            Func<ICacheEntry, Task<Product>> getProduct = (cacheEntry) => _repository.GetAsync(request.Key);
            var product = await _appCache.GetOrAddAsync(request.Key, (cacheEntry) => _repository.GetAsync(request.Key), new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1) });
            var productReponse = _mapper.Map<ProductResponse>(product);
            if (product != null)
            {
                productReponse.Price = await _externalPriceProvider.GetPrice(request.Key);
            }
            return productReponse;
        }
    }
}
