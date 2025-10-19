using System.Net;
using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Features.Commands.CreateProduct;
using ECommerceAPI.Application.Features.Queries.GetAllProduct;
using ECommerceAPI.Application.Repositories.Customer;
using ECommerceAPI.Application.Repositories.File;
using ECommerceAPI.Application.Repositories.InvoiceFile;
using ECommerceAPI.Application.Repositories.Order;
using ECommerceAPI.Application.Repositories.Product;
using ECommerceAPI.Application.Repositories.ProductImageFile;
using ECommerceAPI.Application.RequestParameters;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using File = ECommerceAPI.Domain.Entities.File;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //private readonly IProductService _productService;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        readonly IFileReadRepository  _fileReadRepository;
        readonly IFileWriteRepository  _fileWriteRepository;
        readonly IInvoiceFileReadRepository  _invoiceFileReadRepository;
        readonly IInvoiceFileWriteRepository  _invoiceFileWriteRepository;
        readonly IProductImageFileReadRepository   _productImageFileReadRepository;
        readonly IProductImageFileWriteRepository  _productImageFileWriteRepository;
        
        readonly IStorageService  _storageService;
        readonly IConfiguration _configuration;
        readonly IMediator _mediator;
        
        // private readonly IOrderWriteRepository _orderWriteRepository;
        // private readonly ICustomerWriteRepository _customerWriteRepository;
        // private readonly IOrderReadRepository _orderReadRepository;

        public ProductsController(
            //IProductService productService,
            IProductWriteRepository productWriteRepository, 
            IProductReadRepository productReadRepository,
            // IOrderWriteRepository orderWriteRepository,
            // ICustomerWriteRepository customerWriteRepository,
            // IOrderReadRepository orderReadRepository,
            IWebHostEnvironment webHostEnvironment, 
            
            IFileReadRepository fileReadRepository, 
            IFileWriteRepository fileWriteRepository, 
            IInvoiceFileReadRepository invoiceFileReadRepository, 
            IInvoiceFileWriteRepository invoiceFileWriteRepository, 
            IProductImageFileReadRepository productImageFileReadRepository, 
            IProductImageFileWriteRepository productImageFileWriteRepository, 
            IStorageService storageService, IConfiguration configuration, IMediator mediator)
        {
           // _productService = productService;
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _storageService = storageService;
            this._configuration = configuration;
            _mediator = mediator;
            // _orderWriteRepository = orderWriteRepository;
            // _customerWriteRepository = customerWriteRepository;
            // _orderReadRepository = orderReadRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id,false));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put(VmUpdateProduct model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Stock = model.Stock;
            product.Name = model.Name;
            product.Price = model.Price;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpPost("[action]")] // ...com/api/products?id=1 (query string yapısı) Upload metodunu servis üzerinden çağırdığımız için id olmayadabilir
        public async Task<IActionResult> Upload(string id)
        {
            List<(string FileName, string pathOrContainer)> result = await _storageService.UploadAsync("photo-images", Request.Form.Files);

            Product product = await _productReadRepository.GetByIdAsync(id);
            
            await _productImageFileWriteRepository.AddRangeAsync(result.Select(x => new ProductImageFile
            {
                FileName = x.FileName,
                Path = x.pathOrContainer,
                Storage = _storageService.StorageName,
                Products = new List<Product>() {product}
            }).ToList());
            
            await _productImageFileWriteRepository.SaveAsync();
            
            // var datas = await _storageService.UploadAsync("files",Request.Form.Files);
            //
            // await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            // {
            //     FileName = d.fileName,
            //     Path = d.pathOrContainerName,
            //     Storage = _storageService.StorageName
            // }).ToList());
            // await _productImageFileWriteRepository.SaveAsync();
            
            // await _invoiceFileWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceFile()
            // {
            //     FileName = d.fileName,
            //     Path = d.path,
            //     Price = new Random().Next()
            // }).ToList());
            // await _invoiceFileWriteRepository.SaveAsync();
            
            // await _fileWriteRepository.AddRangeAsync(datas.Select(d => new File()
            // {
            //     FileName = d.fileName,
            //     Path = d.path
            // }).ToList());
            // await _fileWriteRepository.SaveAsync();

            // var d1 = _fileReadRepository.GetAll(false);
            // var d2 = _invoiceFileReadRepository.GetAll(false);
            // var d3 = _productImageFileReadRepository.GetAll(false);
            
            return Ok();
        }

        [HttpGet("[action]/{id}")] // ...com/api/products/1 (route data)
        public async Task<IActionResult> GetProductImages(string id)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            await Task.Delay(600);
            return Ok(product.ProductImageFiles.Select(p => new
            {
                Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                p.FileName,
                p.Id
            }));
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage(string id, string imageId)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            ProductImageFile productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
            product.ProductImageFiles.Remove(productImageFile);

            await _productWriteRepository.SaveAsync();
            return Ok();
        }
        // [HttpGet]
        // public IActionResult GetProducts()
        // {
        //     var products = _productService.GetProducts();
        //     return Ok(products);
        // }

        // [HttpGet]
        // public async Task<IActionResult> Get() 
        // {
            // await _productWriteRepository.AddRangeAsync(new()
            // {
            //     new(){ Id = Guid.NewGuid(), Name = "Product 4", Price = 100, CreatedDate = DateTime.UtcNow, Stock = 10},
            //     new(){ Id = Guid.NewGuid(), Name = "Product 5", Price = 200, CreatedDate = DateTime.UtcNow, Stock = 20},
            //     new(){ Id = Guid.NewGuid(), Name = "Product 6", Price = 300, CreatedDate = DateTime.UtcNow, Stock = 30},
            // });
            // await _productWriteRepository.SaveAsync();
            
            // Product p = await _productReadRepository.GetByIdAsync("396c20b8-62d2-4fa1-993d-2867e9c7b2fc",false); // Tracking = false old. için yaptığım isim değişikliği kaydedilmedi
            // p.Name = "Eren";
            // await _productWriteRepository.SaveAsync();

            // Ortak entity'leri (BaseEntity içindekiler mesela) ortak bir yerden gödermemiz lazım teker teker yazmak yanlış olur.
            // Bunu da interceptor oluşturarak yapacağız
            
            // await _productWriteRepository.AddAsync(new() { Name = "C Product", Price = 1000, CreatedDate = DateTime.UtcNow, Stock = 10 });
            // await _productWriteRepository.SaveAsync();
            
            // var customerId = Guid.NewGuid();
            // await _customerWriteRepository.AddAsync(new() { Id = customerId, Name = "İclal" });
            //
            // await _orderWriteRepository.AddAsync(new() { Description = "Ev", Address = "Kocaeli, Kartepe", CustomerId = customerId });
            // await _orderWriteRepository.AddAsync(new() { Description = "bla bla bla 2", Address = "Ankara, Pursaklar", CustomerId = customerId});
            // await _orderWriteRepository.SaveAsync();
            
            // Üstte sipariş ve müşteri oluşturdum. Aşağıda ise adres değişikliği yaptım
            
            // Order order = await _orderReadRepository.GetByIdAsync("01998665-eb8f-7d8b-90cc-e741607e81d3");
            // order.Address = "İstanbul, Maltepe";
            // order.Description = "İş Yeri";
            // await _orderWriteRepository.SaveAsync();

        //     return Ok("Merhaba");
        // }

        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetById(string id)
        // {
        //     Product product = await _productReadRepository.GetByIdAsync(id);
        //     return Ok(product);
        // }
    }
}
