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


    }
}
