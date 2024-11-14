using MealMate.BLL.Dtos.Product;
using MealMate.BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductAppService _productAppService;
        private readonly IStoreAppService _storeAppService;

        public ProductController(IProductAppService productAppService, IStoreAppService storeAppService)
        {
            _productAppService = productAppService;
            _storeAppService = storeAppService;
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
            var productsAtStore = await _storeAppService.GetAtByProductIDAsync(productid);
            return Ok(productsAtStore);
        }

        [HttpGet("atstore/product/{storeid}")]
        [SwaggerOperation(
            Summary = "Get all products with stock at a store"
        )]
        public async Task<IActionResult> GetProductInformationAtStore(Guid storeid)
        {
            var productsAtStore = await _storeAppService.GetAtByStoreIdAsync(storeid);
            return Ok(productsAtStore);
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
            return Ok(result);
            /*var existingProduct = await _storeAppService.GetAtByProductIDAndStoreIDAsync(productid, storeid);
            if (existingProduct == null)
            {
                return Ok(await _storeAppService.CreateAtAsync(productid, storeid, amount));
            }
            else
            {
                var result = await _storeAppService.UpdateAmountAtAsync(existingProduct, amount);
                return Ok(result);
            }*/
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
