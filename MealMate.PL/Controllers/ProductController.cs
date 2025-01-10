using MealMate.BLL.Dtos.Product;
using MealMate.BLL.IServices;
using MealMate.BLL.IServices.Hubs;
using MealMate.BLL.IServices.Redis;
using MealMate.BLL.Services.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.Annotations;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductAppService _productAppService;
        private readonly IStoreAppService _storeAppService;
        private readonly IReserveCartCacheService _reserveCartCacheService;
        private readonly IHubContext<ProductHub, IProductHubClient> _productHubContext;


        public ProductController(IProductAppService productAppService, IStoreAppService storeAppService, IHubContext<ProductHub, IProductHubClient> productHubContext, IReserveCartCacheService reserveCartCacheService)
        {
            _productAppService = productAppService;
            _storeAppService = storeAppService;
            _productHubContext = productHubContext;
            _reserveCartCacheService = reserveCartCacheService;
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetListProductByCategory(string category)
        {
            var products = await _productAppService.GetListProductByCategoryAsync(category);
            return Ok(products);
        }

        [HttpGet("promotion/{id}")]
        public async Task<IActionResult> GetListProductByPromotionID(Guid id)
        {
            var products = await _productAppService.GetListProductByPromotionIDAsync(id);
            return Ok(products);
        }

        [HttpGet("promotion")]
        public async Task<IActionResult> GetListProductHavePromotion()
        {
            var promotedProducts = await _productAppService.GetListProductHavePromotionAsync();
            return Ok(promotedProducts);
        }

        [HttpGet("store/{id}")]
        public async Task<IActionResult> GetListProductByStoreID(Guid id)
        {
            var products = await _productAppService.GetListProductByStoreIDAsync(id);
            return Ok(products);
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProductByID(Guid id)
        {
            var product = await _productAppService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpGet("atstore/{productid}")]
        [SwaggerOperation(
            Summary = "Which store have this product (Return ATDto)"
        )]
        public async Task<IActionResult> GetProductInformationAtStoreByID(Guid productid)
        {
            var productsAtStore = await _reserveCartCacheService.GetAtByProductIDAsync(productid);
            return Ok(productsAtStore);
        }

        [HttpGet("atstore/product/{storeid}")]
        [SwaggerOperation(
            Summary = "Get all products with stock at a store"
        )]
        public async Task<IActionResult> GetProductInformationAtStore(Guid storeid)
        {
            var productsAtStore = await _reserveCartCacheService.GetAtByStoreIdAsync(storeid);
            return Ok(productsAtStore);
        }

        [HttpGet("atstore/{productid}/{storeid}")]
        [SwaggerOperation(
           Summary = "Get product at store"
       )]
        public async Task<IActionResult> GetProductAtStoreByIDs(Guid productid, Guid storeid)
        {
            var productAtStore = await _reserveCartCacheService.GetAtByProductIDAndStoreIDAsync(productid, storeid);
            return Ok(productAtStore);
        }

        [HttpGet("transaction/{transactionId}")]
        public async Task<IActionResult> GetAllItemsByBillId(Guid transactionId)
        {
            var items = await _productAppService.GetAllItemsByBillIdAsync(transactionId);
            return Ok(items);
        }


        [HttpGet("top5products/{year}")]
        public async Task<IActionResult> GetTop5RevenueProduct(int year)
        {
            var results = await _productAppService.GetTempTop5ProductsAsync(year);
            return Ok(results);
        }


        [HttpPost("chatbot")]
        [SwaggerOperation(
            Summary = "Get products by list of product name"
        )]
        public async Task<IActionResult> GetProductsByListName([FromBody] List<string> productNames)
        {
            var products = await _productAppService.GetProductsByListNameAsync(productNames);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreationDto productDto)
        {
            var result = await _productAppService.CreateProductAsync(productDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductUpdateDto productUpdateDto)
        {
            var result = await _productAppService.UpdateProductAsync(id, productUpdateDto);
            return Ok(result);
        }

        [HttpPut("addtostore/{productid}/{storeid}/{amount}")]
        public async Task<IActionResult> IncreaseProductAtStore(Guid productid, Guid storeid, int amount)
        {
            var result = await _storeAppService.UpdateAmountAtAsync(productid, storeid, amount);

            // Notify only the specific product-store group
            var groupName = $"{productid}_{storeid}";
            await _productHubContext.Clients.Group(groupName).ReceiveChangeStock(productid, result.NumberAtStore);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productAppService.DeleteProductAsync(id);
            return Ok(new { data = true });
        }

        [HttpDelete("{productid}/{storeid}")]
        public async Task<IActionResult> DeleteProductAtStore(Guid productid, Guid storeid)
        {
            await _productAppService.DeleteProductAtStoreAsync(productid, storeid);
            return Ok(new { data = true });
        }

    }
}
