using ComputerStore.Data.Models;

namespace ComputerStore.Services.Interfaces;

public interface IPartService
{
    // ── Parts ────────────────────────────────────────────────────────
    /// <summary>Returns all parts, optionally filtered by category.</summary>
    List<PcPart> GetAll(int? categoryId = null);

    PcPart? GetById(int id);
    void    Add(PcPart part);
    void    Update(PcPart updated);
    void    Delete(int id);

    // ── Categories ───────────────────────────────────────────────────
    List<Category> GetCategories();
    void           AddCategory(string name, string? description);
    void           DeleteCategory(int id);

    // ── Manufacturers ────────────────────────────────────────────────
    List<Manufacturer> GetManufacturers();
    void               AddManufacturer(string name, string? country, string? website);
    void               DeleteManufacturer(int id);
}
