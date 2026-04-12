using System.ComponentModel.DataAnnotations;
using ComputerStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ComputerStore.Data.Dtos;

namespace ComputerStore.Data.Seeder
{
    public static class JsonSeeder
    {
        /// <summary>
        /// Seeds Categories, Manufacturers and PcParts from JSON files
        /// found in <paramref name="seedDataPath"/>.
        /// Entries that fail validation are silently skipped.
        /// </summary>
        public static void Seed(ComputerStoreDbContext ctx, string seedDataPath)
        {
            SeedCategories(ctx, seedDataPath);
            SeedManufacturers(ctx, seedDataPath);
            SeedParts(ctx, seedDataPath);
        }

        // ── Categories ────────────────────────────────────────────────
        private static void SeedCategories(ComputerStoreDbContext ctx, string basePath)
        {
            string file = Path.Combine(basePath, "categories.json");
            if (!File.Exists(file)) return;

            var dtos = Deserialize<List<CategoryDto>>(file);
            if (dtos == null) return;

            // Load existing names once to avoid per-row DB trips
            var existing = ctx.Categories
                              .AsNoTracking()
                              .Select(c => c.Name)
                              .ToHashSet(StringComparer.OrdinalIgnoreCase);

            int added = 0;
            foreach (var dto in dtos)
            {
                if (!IsValid(dto)) continue;
                if (existing.Contains(dto.Name!)) continue;

                ctx.Categories.Add(new Category
                {
                    Name        = dto.Name!,
                    Description = dto.Description,
                });
                existing.Add(dto.Name!);
                added++;
            }

            if (added > 0) ctx.SaveChanges();
        }

        // ── Manufacturers ─────────────────────────────────────────────
        private static void SeedManufacturers(ComputerStoreDbContext ctx, string basePath)
        {
            string file = Path.Combine(basePath, "manufacturers.json");
            if (!File.Exists(file)) return;

            var dtos = Deserialize<List<ManufacturerDto>>(file);
            if (dtos == null) return;

            var existing = ctx.Manufacturers
                              .AsNoTracking()
                              .Select(m => m.Name)
                              .ToHashSet(StringComparer.OrdinalIgnoreCase);

            int added = 0;
            foreach (var dto in dtos)
            {
                if (!IsValid(dto)) continue;
                if (existing.Contains(dto.Name!)) continue;

                ctx.Manufacturers.Add(new Manufacturer
                {
                    Name    = dto.Name!,
                    Country = dto.Country,
                    Website = dto.Website,
                });
                existing.Add(dto.Name!);
                added++;
            }

            if (added > 0) ctx.SaveChanges();
        }

        // ── PC Parts ──────────────────────────────────────────────────
        private static void SeedParts(ComputerStoreDbContext ctx, string basePath)
        {
            string file = Path.Combine(basePath, "parts.json");
            if (!File.Exists(file)) return;

            var dtos = Deserialize<List<PcPartDto>>(file);
            if (dtos == null) return;

            // Build lookup dictionaries so we don't hit the DB for every part
            var categories = ctx.Categories
                                .AsNoTracking()
                                .ToDictionary(c => c.Name, c => c.Id,
                                              StringComparer.OrdinalIgnoreCase);

            var manufacturers = ctx.Manufacturers
                                   .AsNoTracking()
                                   .ToDictionary(m => m.Name, m => m.Id,
                                                 StringComparer.OrdinalIgnoreCase);

            var existingParts = ctx.PcParts
                                   .AsNoTracking()
                                   .Select(p => p.Name)
                                   .ToHashSet(StringComparer.OrdinalIgnoreCase);

            int added = 0;
            foreach (var dto in dtos)
            {
                if (!IsValid(dto)) continue;
                if (existingParts.Contains(dto.Name!)) continue;

                // Resolve names → IDs; skip the part if either is unknown
                if (!categories.TryGetValue(dto.CategoryName!, out int catId)) continue;
                if (!manufacturers.TryGetValue(dto.ManufacturerName!, out int mfrId)) continue;

                ctx.PcParts.Add(new PcPart
                {
                    Name           = dto.Name!,
                    Description    = dto.Description,
                    Price          = dto.Price,
                    Stock          = dto.Stock,
                    ImagePath      = dto.ImagePath,
                    CategoryId     = catId,
                    ManufacturerId = mfrId,
                    CreatedAt      = DateTime.UtcNow,
                });
                existingParts.Add(dto.Name!);
                added++;
            }

            if (added > 0) ctx.SaveChanges();
        }

        // ── Helpers ───────────────────────────────────────────────────

        /// <summary>Deserialises a JSON file; returns null on any error.</summary>
        private static T? Deserialize<T>(string path)
        {
            try
            {
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Validates a DTO using DataAnnotations.
        /// Returns false (and silently ignores) if any attribute fails.
        /// </summary>
        private static bool IsValid(object dto)
        {
            var ctx     = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            return Validator.TryValidateObject(dto, ctx, results, validateAllProperties: true);
        }
    }
}
