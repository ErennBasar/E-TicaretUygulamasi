using System.Net;
using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Repositories.Customer;
using ECommerceAPI.Application.Repositories.Order;
using ECommerceAPI.Application.Repositories.Product;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //private readonly IProductService _productService;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        // private readonly IOrderWriteRepository _orderWriteRepository;
        // private readonly ICustomerWriteRepository _customerWriteRepository;
        // private readonly IOrderReadRepository _orderReadRepository;

        public ProductsController(
            //IProductService productService,
            IProductWriteRepository productWriteRepository, 
            IProductReadRepository productReadRepository
            // IOrderWriteRepository orderWriteRepository,
            // ICustomerWriteRepository customerWriteRepository,
            // IOrderReadRepository orderReadRepository
            )
        {
           // _productService = productService;
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            // _orderWriteRepository = orderWriteRepository;
            // _customerWriteRepository = customerWriteRepository;
            // _orderReadRepository = orderReadRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(_productReadRepository.GetAll(false));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id,false));
        }

        [HttpPost]
        public async Task<IActionResult> Post(VmCreateProduct model)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
            });
            await _productWriteRepository.SaveAsync();
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

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
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
