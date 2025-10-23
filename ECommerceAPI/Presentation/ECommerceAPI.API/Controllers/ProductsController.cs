using System.Net;
using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Features.Commands.Product.CreateProduct;
using ECommerceAPI.Application.Features.Commands.Product.RemoveProduct;
using ECommerceAPI.Application.Features.Commands.Product.UpdateProduct;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ECommerceAPI.Application.Features.Queries.Product.GetAllProduct;
using ECommerceAPI.Application.Features.Queries.Product.GetByIdProduct;
using ECommerceAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using File = ECommerceAPI.Domain.Entities.File;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")] // program.cs de default scheme olarak belirtildi
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
        
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest) // ok
        {
            GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest  updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok(); // geriye bir şey döndürmemize neden gerek duymadık ?
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromQuery] RemoveProductCommandRequest  removeProductCommandRequest)
        {
            RemoveProductCommandResponse response = await _mediator.Send(removeProductCommandRequest);
            return Ok();
        }

        [HttpPost("[action]")] // ...com/api/products?id=1 (query string yapısı) Upload metodunu servis üzerinden çağırdığımız için id olmayadabilir
        public async Task<IActionResult> Upload([FromQuery] string id)
        {
            var command = new UploadProductImageCommandRequest
            {
                Id = id,
                Files = Request.Form.Files // Dosyaları Request'ten manuel olarak alıyoruz.
            };
            
            UploadProductImageCommandResponse response = await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("[action]/{Id}")] // ...com/api/products/1 (route data)
        public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
        {
            List<GetProductImagesQueryResponse> response = await _mediator.Send(getProductImagesQueryRequest);
            return Ok(response);
        }

        [HttpDelete("[action]/{Id}")] // Id route'dan geliyor
        public async Task<IActionResult> DeleteProductImage([FromRoute] string Id, [FromQuery] string imageId)
        {
            var command = new RemoveProductImageCommandRequest
            {
                Id = Id,
                imageId = imageId
            };
            RemoveProductImageCommandResponse response = await _mediator.Send(command);
            
            // Geriye handler'dan dönen response nesnesini dönmek daha iyi bir pratiktir.
            return Ok(response);
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
