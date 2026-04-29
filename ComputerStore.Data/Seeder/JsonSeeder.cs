// ══════════════════════════════════════════════════════════════════════
// DATA STRUCTURES REQUIREMENT — this file demonstrates 3 of the ≥3
// required data structures in a realistic setting:
//
//   1. Dictionary<string, int>  — O(1) name→ID lookups          line ~80
//   2. HashSet<string>          — O(1) duplicate detection       line ~55
//   3. List<T>                  — ordered, resizable collection  line ~45
//
// ══════════════════════════════════════════════════════════════════════
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using ComputerStore.Data.Models;
using ComputerStore.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ComputerStore.Data.Dtos;

namespace ComputerStore.Data.Seeder
{
    public static class JsonSeeder
    {
        /// <summary>
        /// Seeds Categories, Manufacturers, PcParts and Users from JSON files
        /// found in <paramref name="seedDataPath"/>.
        /// Entries that fail validation are silently skipped.
        /// </summary>
        public static void Seed(ComputerStoreDbContext ctx, string seedDataPath)
        {
            SeedCategories(ctx, seedDataPath);
            SeedManufacturers(ctx, seedDataPath);
            SeedParts(ctx, seedDataPath);
            SeedUsers(ctx, seedDataPath);
        }

        // ── Categories ────────────────────────────────────────────────
        private static void SeedCategories(ComputerStoreDbContext ctx, string basePath)
        {
            string file = Path.Combine(basePath, "categories.json");
            if (!File.Exists(file)) return;

            // DATA STRUCTURE: List<CategoryDto>
            // Deserialisation produces a List — ordered, index-accessible.
            // REQUIREMENT: List is one of the ≥3 data structures.
            var dtos = Deserialize<List<CategoryDto>>(file);
            if (dtos == null) return;

            // DATA STRUCTURE: HashSet<string>
            // HashSet gives O(1) Contains() — far faster than scanning a List
            // each iteration.  Used here to track which names already exist
            // so we avoid issuing a DB query per row.
            // REQUIREMENT: HashSet (Set) is one of the ≥3 data structures.
            var existing = ctx.Categories
                              .AsNoTracking()
                              .Select(c => c.Name)
                              .ToHashSet(StringComparer.OrdinalIgnoreCase);  // HashSet<string>

            int added = 0;
            foreach (var dto in dtos)
            {
                if (!IsValid(dto))              continue;
                if (existing.Contains(dto.Name!)) continue;   // O(1) HashSet lookup

                ctx.Categories.Add(new Category
                {
                    Name        = dto.Name!,
                    Description = dto.Description,
                });
                existing.Add(dto.Name!);   // keep the HashSet in sync
                added++;
            }

            if (added > 0) ctx.SaveChanges();
        }

        // ── Manufacturers ─────────────────────────────────────────────
        private static void SeedManufacturers(ComputerStoreDbContext ctx, string basePath)
        {
            string file = Path.Combine(basePath, "manufacturers.json");
            if (!File.Exists(file)) return;

            var dtos = Deserialize<List<ManufacturerDto>>(file);  // List<ManufacturerDto>
            if (dtos == null) return;

            var existing = ctx.Manufacturers
                              .AsNoTracking()
                              .Select(m => m.Name)
                              .ToHashSet(StringComparer.OrdinalIgnoreCase);  // HashSet<string>

            int added = 0;
            foreach (var dto in dtos)
            {
                if (!IsValid(dto))              continue;
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

            var dtos = Deserialize<List<PcPartDto>>(file);   // List<PcPartDto>
            if (dtos == null) return;

            // DATA STRUCTURE: Dictionary<string, int>
            // Maps category / manufacturer NAME → database ID.
            // Dictionary gives O(1) TryGetValue — avoids a DB round-trip
            // for every part in the JSON file.
            // REQUIREMENT: Dictionary is one of the ≥3 data structures.
            var categories = ctx.Categories
                                .AsNoTracking()
                                .ToDictionary(c => c.Name, c => c.Id,
                                              StringComparer.OrdinalIgnoreCase);   // Dictionary<string,int>

            var manufacturers = ctx.Manufacturers
                                   .AsNoTracking()
                                   .ToDictionary(m => m.Name, m => m.Id,
                                                 StringComparer.OrdinalIgnoreCase); // Dictionary<string,int>

            var existingParts = ctx.PcParts
                                   .AsNoTracking()
                                   .Select(p => p.Name)
                                   .ToHashSet(StringComparer.OrdinalIgnoreCase);    // HashSet<string>

            int added = 0;
            foreach (var dto in dtos)
            {
                if (!IsValid(dto))                   continue;
                if (existingParts.Contains(dto.Name!)) continue;

                // Dictionary.TryGetValue — O(1) lookup
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

        // ── Users ─────────────────────────────────────────────────────
        private static void SeedUsers(ComputerStoreDbContext ctx, string basePath)
        {
            string file = Path.Combine(basePath, "users.json");
            if (!File.Exists(file)) return;

            var dtos = Deserialize<List<UserDto>>(file);   // List<UserDto>
            if (dtos == null) return;

            // HashSet<string> — O(1) duplicate check for both username and email
            var existingUsernames = ctx.Users
                                       .AsNoTracking()
                                       .Select(u => u.Username)
                                       .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var existingEmails = ctx.Users
                                    .AsNoTracking()
                                    .Select(u => u.Email)
                                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

            int added = 0;
            foreach (var dto in dtos)
            {
                if (!IsValid(dto))                         continue;
                if (existingUsernames.Contains(dto.Username!)) continue;   // skip duplicates
                if (existingEmails.Contains(dto.Email!))       continue;

                // Parse role string — anything unrecognised defaults to Customer
                UserRole role = Enum.TryParse<UserRole>(dto.Role, ignoreCase: true, out var parsed)
                    ? parsed
                    : UserRole.Customer;

                ctx.Users.Add(new User
                {
                    Username     = dto.Username!,
                    Email        = dto.Email!,
                    PasswordHash = HashPassword(dto.Password!),
                    Role         = role,
                    CreatedAt    = DateTime.UtcNow,
                });

                existingUsernames.Add(dto.Username!);
                existingEmails.Add(dto.Email!);
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

        /// <summary>SHA-256 hash</summary>
        private static string HashPassword(string password) =>
            Convert.ToBase64String(
                SHA256.HashData(
                    Encoding.UTF8.GetBytes(password)));
    }
}
