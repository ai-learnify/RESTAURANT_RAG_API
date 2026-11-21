using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Entities;
using RestaurantApi.Data;

namespace RestaurantApi.Controllers;

[ApiController, Route("api/menu")]
public sealed class MenuController(DataContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll()
    {
        var products = await db.Products.OrderBy(p => p.Id).ToListAsync();

        return products;
    }
}