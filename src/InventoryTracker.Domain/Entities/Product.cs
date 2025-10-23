namespace InventoryTracker.Domain.Entities
{
    /// <summary>
    /// مدل اصلی "کالا" که به جدول Products نگاشت می‌شود.
    /// </summary>
    public class Product
    {
        /// <summary>کلید اصلی (Identity) در دیتابیس.</summary>
        public int Id { get; set; }

        /// <summary>نام کالا (اجباری).</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>کد انبار/اسکيو برای رهگیری (Unique ترجیحاً).</summary>
        public string SKU { get; set; } = string.Empty;

        /// <summary>قیمت واحد کالا.</summary>
        public decimal Price { get; set; }

        /// <summary>موجودی فعلی در انبار.</summary>
        public int Quantity { get; set; }
    }
}
