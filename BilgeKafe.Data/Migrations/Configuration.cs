namespace BilgeKafe.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BilgeKafe.Data.KafeVeri>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BilgeKafe.Data.KafeVeri context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            if (!context.Urunler.Any())
            {
                context.Urunler.Add(new Urun() { UrunAd = "Kola", BirimFiyat = 5.99m });
                context.Urunler.Add(new Urun() { UrunAd = "Çay", BirimFiyat = 4.50m });
            }
        }
    }
}
