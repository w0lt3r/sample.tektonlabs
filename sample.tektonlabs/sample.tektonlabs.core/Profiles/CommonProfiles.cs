using AutoMapper;
using sample.tektonlabs.core.Models;
using sample.tektonlabs.core.Requests;

namespace sample.tektonlabs.core.Profiles;

public class CommonProfiles : Profile
{
    public CommonProfiles()
    {
        CreateMap<InsertProductRequest, Product>();
        CreateMap<UpdateProductRequest, Product>();
        CreateMap<Product, ProductResponse>();
    }
}

