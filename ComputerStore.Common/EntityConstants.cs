
namespace ComputerStore.Common
{
    public static class EntityConstants
    {
        public static class User
        {
            public const int UsernameMaxLength = 50;
            public const int UsernameMinLength = 3;
            public const int PasswordHashLength = 256;
            public const int EmailMaxLength = 100;
        }

        public static class Category
        {
            public const int NameMaxLength = 50;
            public const int DescriptionMaxLength = 200;
        }

        public static class Manufacturer
        {
            public const int NameMaxLength = 50;
            public const int CountryMaxLength = 60;
            public const int WebsiteMaxLength = 200;
        }

        public static class PcPart
        {
            public const int NameMaxLength = 100;
            public const int DescriptionMaxLength = 1000;
            public const int ImagePathMaxLength = 500;
            public const string PriceColumnType = "decimal(18,2)";
            public const decimal MinPrice = 0.01m;
            public const decimal MaxPrice = 999999.99m;
            public const int MinStock = 0;
        }

        public static class Order
        {
            public const string TotalPriceColumnType = "decimal(18,2)";
        }

        public static class OrderItem
        {
            public const string UnitPriceColumnType = "decimal(18,2)";
            public const int MinQuantity = 1;
            public const int MaxQuantity = 999;
        }
    }
}