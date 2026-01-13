using TA_API.Core.Dtos;

namespace TA_API.Core.Utils
{
    public static class DataUtils
    {
        public static List<ProductDto> Products =>
        [
            new ProductDto(1, "Product 1", 10.0m),
            new ProductDto(2, "Product 2", 20.0m),
            new ProductDto(3, "Product 3", 30.0m),
            new ProductDto(4, "Product 4", 40.0m),
            new ProductDto(5, "Product 5", 50.0m),
            new ProductDto(6, "Product 6", 60.0m),
        ];
    }
}
