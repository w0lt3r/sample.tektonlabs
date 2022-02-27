using AutoMapper;
using LazyCache;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using sample.tektonlabs.core.Interfaces;
using sample.tektonlabs.core.Models;
using sample.tektonlabs.core.Requests;

namespace sample.tektonlabs.core.Handlers;

public class UpdateProductHandler : IRequestHandler<UpdateProductRequest>
{
    private readonly IRepository<Product> _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateProductHandler(IUnitOfWork unitOfWork, IRepository<Product> repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var existentProduct = await _repository.GetAsync(request.Key);
        if (existentProduct != null)
        {
            var product = _mapper.Map<Product>(request);
            await _repository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }
        else throw new ArgumentException($"A product with the key:{request.Key} does not exist.");
        return Unit.Value;
    }
}

