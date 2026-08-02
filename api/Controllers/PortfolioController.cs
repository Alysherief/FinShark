using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PortfolioController : ControllerBase
    { 
        private readonly UserManager<AppUser> _userManager;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IStockRepository _stockRepository;

        public PortfolioController(
            UserManager<AppUser> userManager,
            IStockRepository stockRepository,
            IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();

            var appUser = await _userManager.FindByNameAsync(username);

            if (appUser == null)
            {
                return Unauthorized("User not found.");
            }

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            return Ok(userPortfolio);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if (stock == null)
            {
                return NotFound("Stock not found.");
            }

            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id
            };

            // Check if the stock is already in the user's portfolio
            var existingEntry = await _portfolioRepository.GetUserPortfolio(appUser);
            if (existingEntry.Any(p => p.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Stock is already in the portfolio.");
            }

            // Add the stock to the user's portfolio
            await _portfolioRepository.CreateAsync(portfolioModel);
            if(portfolioModel == null)
            {
                return StatusCode(500, "An error occurred while adding the stock to the portfolio.");
            }
            else
            {
                return Created();
            }

            
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            var portfolioEntry = userPortfolio.FirstOrDefault(p => p.Symbol.ToLower() == symbol.ToLower());
            var filteredstock = userPortfolio.Where(p => p.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (filteredstock.Count == 1)
            {
                await _portfolioRepository.DeleteAsync(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock not found in the portfolio.");
            }
            return Ok();
        }
    }
}