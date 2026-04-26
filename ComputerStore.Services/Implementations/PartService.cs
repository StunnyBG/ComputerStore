using ComputerStore.Data;
using ComputerStore.Data.Models;
using ComputerStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Services.Implementations;

public class PartService : IPartService
{
    // ── Parts ────────────────────────────────────────────────────────

    public List<PcPart> GetAll(int? categoryId = null)
    {
        using var ctx = DbContextFactory.Create();
        var query = ctx.PcParts
                       .Include(p => p.Category)
                       .Include(p => p.Manufacturer)
                       .AsNoTracking()
                       .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        // Return sorted by Name so BinarySearch works correctly in the UI layer.
        return query.OrderBy(p => p.Name).ToList();
    }

    public PcPart? GetById(int id)
    {
        using var ctx = DbContextFactory.Create();
        return ctx.PcParts
                  .Include(p => p.Category)
                  .Include(p => p.Manufacturer)
                  .AsNoTracking()
                  .FirstOrDefault(p => p.Id == id);
    }

    public void Add(PcPart part)
    {
        using var ctx = DbContextFactory.Create();
        ctx.PcParts.Add(part);
        ctx.SaveChanges();
    }

    public void Update(PcPart updated)
    {
        using var ctx = DbContextFactory.Create();
        var p            = ctx.PcParts.First(x => x.Id == updated.Id);
        p.Name           = updated.Name;
        p.Description    = updated.Description;
        p.Price          = updated.Price;
        p.Stock          = updated.Stock;
        p.ImagePath      = updated.ImagePath;
        p.CategoryId     = updated.CategoryId;
        p.ManufacturerId = updated.ManufacturerId;
        ctx.SaveChanges();
    }

    public void Delete(int id)
    {
        using var ctx = DbContextFactory.Create();
        var part = ctx.PcParts.Find(id);
        if (part is null) return;
        ctx.PcParts.Remove(part);
        ctx.SaveChanges();
    }

    // ── Categories ───────────────────────────────────────────────────

    public List<Category> GetCategories()
    {
        using var ctx = DbContextFactory.Create();
        return ctx.Categories.AsNoTracking().OrderBy(c => c.Name).ToList();
    }

    public void AddCategory(string name, string? description)
    {
        using var ctx = DbContextFactory.Create();
        ctx.Categories.Add(new Category { Name = name.Trim(), Description = description });
        ctx.SaveChanges();
    }

    public void DeleteCategory(int id)
    {
        using var ctx = DbContextFactory.Create();
        var cat = ctx.Categories.Find(id);
        if (cat is null) return;
        ctx.Categories.Remove(cat);
        ctx.SaveChanges();
    }

    // ── Manufacturers ────────────────────────────────────────────────

    public List<Manufacturer> GetManufacturers()
    {
        using var ctx = DbContextFactory.Create();
        return ctx.Manufacturers.AsNoTracking().OrderBy(m => m.Name).ToList();
    }

    public void AddManufacturer(string name, string? country, string? website)
    {
        using var ctx = DbContextFactory.Create();
        ctx.Manufacturers.Add(new Manufacturer
        {
            Name    = name.Trim(),
            Country = country,
            Website = website,
        });
        ctx.SaveChanges();
    }

    public void DeleteManufacturer(int id)
    {
        using var ctx = DbContextFactory.Create();
        var mfr = ctx.Manufacturers.Find(id);
        if (mfr is null) return;
        ctx.Manufacturers.Remove(mfr);
        ctx.SaveChanges();
    }
}
