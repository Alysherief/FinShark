using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock.Comment;
using api.Models;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Mappers;
using api.Dtos.Stock;


namespace api.Dtos.Stock
{
    public class CreateStockRequestDto
    {
       public string Symbol { get; set; } = string.Empty;
       public string CompanyName { get; set; } = string.Empty;
       public decimal Purchase { get; set; } 
       public decimal LastDiv { get; set; } 
       public string Industry { get; set; } = string.Empty;
       public long MarketCap { get; set; }
       
      
      
    }
}