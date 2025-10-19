using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Repositories.Product;
using ECommerceAPI.Application.Repositories.ProductImageFile;
using MediatR;

namespace ECommerceAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;

public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
{
    readonly IProductReadRepository  _productReadRepository;
    readonly IProductImageFileWriteRepository  _productImageFileWriteRepository;
    readonly IStorageService  _storageService;

    public UploadProductImageCommandHandler(IProductReadRepository productReadRepository, IStorageService storageService, IProductImageFileWriteRepository productImageFileWriteRepository)
    {
        _productReadRepository = productReadRepository;
        _storageService = storageService;
        _productImageFileWriteRepository = productImageFileWriteRepository;
    }

    public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        List<(string FileName, string pathOrContainer)> result = await _storageService.UploadAsync("photo-images", request.Files);

        Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);
            
        await _productImageFileWriteRepository.AddRangeAsync(result.Select(x => new Domain.Entities.ProductImageFile
        {
            FileName = x.FileName,
            Path = x.pathOrContainer,
            Storage = _storageService.StorageName,
            Products = new List<Domain.Entities.Product>() {product}
        }).ToList());
        
        await _productImageFileWriteRepository.SaveAsync();
        return new();
    }
}