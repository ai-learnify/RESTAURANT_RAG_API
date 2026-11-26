using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using RestaurantApi.Data;

namespace RestaurantApi.Tools;

public record MenuItemInfo(string Name, decimal Price);

public sealed class MenuTools(DataContext db)
{
    [Description("Returns the restaurant menu: dish name and price in GEL")]
    public async Task<List<MenuItemInfo>> GetMenu()
    {
        return await db.Products
            .Select(p => new MenuItemInfo(p.Name, p.Price))
            .ToListAsync();
    }
}