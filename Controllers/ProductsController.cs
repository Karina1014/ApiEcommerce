using ApiEcommerce.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Models;

namespace ApiEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        private readonly IMapper _mapper;
        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    //Metodo get por IdProduct
    [HttpGet("{IdProduct:int}", Name = "GetProduct")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetProduct(int IdProduct)
    {
      var product = _productRepository.GetProduct(IdProduct);
      if (product == null)
      {
        return NotFound($"El producto con el id {IdProduct} no existe");
      }
      var productDto = _mapper.Map<ProductDto>(product);
      return Ok(productDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateProduct([FromBody] CreateProductDto createProductDto)
    {
      if (createProductDto == null)
      {
        return BadRequest(ModelState);
      }
      if (_productRepository.ProductExists(createProductDto.Name))
      {
        ModelState.AddModelError("CustomError", "El producto ya existe");
        return BadRequest(ModelState);
      }
      if (!_categoryRepository.CategoryExists(createProductDto.IdCategory))
      {
        ModelState.AddModelError("CustomError", $"La categoría con el {createProductDto.IdCategory} no existe");
        return BadRequest(ModelState);
      }
      var product = _mapper.Map<Product>(createProductDto);
      if (!_productRepository.CreateProduct(product))
      {
        ModelState.AddModelError("CustomError", $"Algo salió mal al guardar el registro {product.Name}");
        return StatusCode(500, ModelState);
      }
      var createdProduct = _productRepository.GetProduct(product.IdProduct);
      var productoDto = _mapper.Map<ProductDto>(createdProduct);
      return CreatedAtRoute("GetProduct", new { IdProduct = product.IdProduct }, productoDto);
    }
    
     [HttpGet("searchProductByCategory/{categoryId:int}", Name = "GetProductsForCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetProductsForCategory(int categoryId)
    {
      var products = _productRepository.GetProductsForCategory(categoryId);
      if (products.Count == 0)
      {
        return NotFound($"Los productos con la categoría {categoryId} no existen");
      }
      var productsDto = _mapper.Map<List<ProductDto>>(products);
      return Ok(productsDto);
    }

    //Busqueda de productos
    [HttpGet("searchProductByNameDescription/{searchTerm}", Name = "SearchProducts")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult SearchProducts(string searchTerm)
    {
      var products = _productRepository.SearchProducts(searchTerm);
      if (products.Count == 0)
      {
        return NotFound($"Los productos con el nombre o descripción '{searchTerm}' no existen");
      }
      var productsDto = _mapper.Map<List<ProductDto>>(products);
      return Ok(productsDto);
    }

    //Metodo de compra del producto
    [HttpPatch("buyProduct/{name}/{quantity:int}", Name = "BuyProduct")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult BuyProduct(string name, int quantity)
    {
      if (string.IsNullOrWhiteSpace(name) || quantity <= 0)
      {
        return BadRequest("El nombre del producto o la cantidad no son válidos");
      }
      var foundProduct = _productRepository.ProductExists(name);
      if (!foundProduct)
      {
        return NotFound($"El producto con el nombre {name} no existe");
      }
      if (!_productRepository.BuyProduct(name, quantity))
      {
        ModelState.AddModelError("CustomError", $"No se pudo comprar el producto {name} o la cantidad solicitada es mayor al stock disponible");
        return BadRequest(ModelState);
      }
      var units = quantity == 1 ? "unidad" : "unidades";
      return Ok($"Se compro {quantity} {units} del producto '{name}'");
    }

    //Actualizar el registro del producto
    [HttpPut("{IdProduct:int}", Name = "UpdateProduct")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateProduct(int IdProduct, [FromBody] UpdateProductDto updateProductDto)
    {
      if (updateProductDto == null)
      {
        return BadRequest(ModelState);
      }
      if (!_productRepository.ProductExists(IdProduct))
      {
        ModelState.AddModelError("CustomError", "El producto no existe");
        return BadRequest(ModelState);
      }
      if (!_categoryRepository.CategoryExists(updateProductDto.IdCategory))
      {
        ModelState.AddModelError("CustomError", $"La categoría con el {updateProductDto.IdCategory} no existe");
        return BadRequest(ModelState);
      }
      var product = _mapper.Map<Product>(updateProductDto);
      product.IdProduct = IdProduct;
      if (!_productRepository.UpdateProduct(product))
      {
        ModelState.AddModelError("CustomError", $"Algo salió mal al actualizar el registro {product.Name}");
        return StatusCode(500, ModelState);
      }
      return NoContent();
    }

    //Eliminar registro del producto
    [HttpDelete("{productId:int}", Name = "DeleteProduct")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeleteProduct(int productId)
    {
      if (productId == 0)
      {
        return BadRequest(ModelState);
      }

      var product = _productRepository.GetProduct(productId);
      if (product == null)
      {
        return NotFound($"El producto con el id {productId} no existe");
      }
      if (!_productRepository.DeleteProduct(product))
      {
        ModelState.AddModelError("CustomError", $"Algo salió mal al eliminar el registro {product.Name}");
        return StatusCode(500, ModelState);
      }
      return NoContent();
    }


    }
}
