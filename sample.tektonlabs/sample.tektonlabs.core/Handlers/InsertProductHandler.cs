using AutoMapper;
using MediatR;
using sample.tektonlabs.core.Interfaces;
using sample.tektonlabs.core.Models;
using sample.tektonlabs.core.Requests;
using sample.tektonlabs.core.Validations;

namespace sample.tektonlabs.core.Handlers;

public class InsertProductHandler : IRequestHandler<InsertProductRequest>
{
    private readonly IRepository<Product> _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public InsertProductHandler(IUnitOfWork unitOfWork, IRepository<Product> repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(InsertProductRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        //sample usage of Fluent Validation
        //var validator = new ProductValidator();
        //var response = validator.Validate(new Product());
        var existentProduct = await _repository.GetAsync(request.Key);
        if (existentProduct == null)
        {
            await _repository.InsertAsync(_mapper.Map<Product>(request));
            await _unitOfWork.SaveChangesAsync();
        }
        else throw new ArgumentException($"A product with the key:{request.Key} already exists.");
        return Unit.Value;
    }
}

