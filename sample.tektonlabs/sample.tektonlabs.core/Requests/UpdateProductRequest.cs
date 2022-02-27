using MediatR;
using sample.tektonlabs.core.Models;
using System.ComponentModel.DataAnnotations;

namespace sample.tektonlabs.core.Requests;

public class UpdateProductRequest : IRequest
{
    [Required]
    public string Key { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime ExpireOn { get; set; }
    [Required]
    [MinLength(1)]
    public List<ProductProvider> Providers { get; set; }
}
