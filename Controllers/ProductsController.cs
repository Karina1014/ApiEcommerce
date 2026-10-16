using ApiEcommerce.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiEcommerce.Models.Dtos;

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

    }
}
