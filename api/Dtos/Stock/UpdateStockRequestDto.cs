using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class UpdateStockRequestDto
    {

       [Required]
       [MaxLength(10, ErrorMessage = "Symbol cannot exceed 10 characters.")]
       public string Symbol { get; set; } = string.Empty;
       [Required]
       [MaxLength(10, ErrorMessage = "Company name cannot exceed 10 characters.")]
       public string CompanyName { get; set; } = string.Empty;
       [Required]
       [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be a positive value.")]
       public decimal Purchase { get; set; } 
       [Required]
       [Range(0.001, 100)]
       public decimal LastDiv { get; set; } 
       [Required]
       [MaxLength(10, ErrorMessage = "Industry cannot exceed 10 characters.")]
       public string Industry { get; set; } = string.Empty;
       [Required]
       [Range(0, long.MaxValue, ErrorMessage = "Market cap must be a positive value.")]
       public long MarketCap { get; set; }
    }
}