using PlatformService.Models;
namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            //context.Database.EnsureCreated();
            if (!context.Platforms.Any())
            {
                Console.WriteLine("Seeding data...");
                context.Platforms.AddRange(
                    new Platform
                    {
                        Name= "DotNet",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },
                    new Platform
                    {
                        Name = "Angular",
                        Publisher = "Google",
                        Cost = "Free"
                    },
                    new Platform
                    {
                        Name = "Java",
                        Publisher = "Oracle",
                        Cost = "Free"
                    }
                );

                context.SaveChanges();
            }
            else
                Console.WriteLine("Already have data");

            return;
        }
    }
}
