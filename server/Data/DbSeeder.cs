using server.Models;

namespace server.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Seed Event Types
            if (!context.EventTypes.Any())
            {
                context.EventTypes.AddRange(

                    new EventType
                    {
                        Name = "Wedding",
                        Description = "Beautiful wedding planning services.",
                        BasePrice = 50000
                    },

                    new EventType
                    {
                        Name = "Birthday",
                        Description = "Amazing birthday party arrangements.",
                        BasePrice = 20000
                    },

                    new EventType
                    {
                        Name = "Corporate",
                        Description = "Professional corporate events.",
                        BasePrice = 80000
                    },

                    new EventType
                    {
                        Name = "Baby Shower",
                        Description = "Celebrate your special day.",
                        BasePrice = 25000
                    },

                    new EventType
                    {
                        Name = "Anniversary",
                        Description = "Make your anniversary memorable.",
                        BasePrice = 30000
                    }

                );

                context.SaveChanges();
            }

            // Seed Packages
            if (!context.Packages.Any())
            {
                var wedding = context.EventTypes.First(e => e.Name == "Wedding");
                var birthday = context.EventTypes.First(e => e.Name == "Birthday");
                var corporate = context.EventTypes.First(e => e.Name == "Corporate");

                context.Packages.AddRange(

                    new Package
                    {
                        EventTypeId = wedding.Id,
                        PackageName = "Silver",
                        Price = 50000,
                        Description = "Decoration + Basic Lighting",
                        IsTrending = false,
                        Images = new List<PackageImage>
                        {
                            new PackageImage { ImageUrl = "https://images.unsplash.com/photo-1519741497674-611481863552?w=800" },
                            new PackageImage { ImageUrl = "https://images.unsplash.com/photo-1465495976277-4387d4b0b4c6?w=800" }
                        }
                    },

                    new Package
                    {
                        EventTypeId = wedding.Id,
                        PackageName = "Gold",
                        Price = 100000,
                        Description = "Decoration + Catering + DJ",
                        IsTrending = true,
                        Images = new List<PackageImage>
                        {
                            new PackageImage { ImageUrl = "https://images.unsplash.com/photo-1519225421980-715cb0215aed?w=800" },
                            new PackageImage { ImageUrl = "https://images.unsplash.com/photo-1511795409834-ef04bbd61622?w=800" },
                            new PackageImage { ImageUrl = "https://images.unsplash.com/photo-1519167758481-83f550bb49b3?w=800" }
                        }
                    },

                    new Package
                    {
                        EventTypeId = wedding.Id,
                        PackageName = "Platinum",
                        Price = 200000,
                        Description = "Complete Premium Wedding Package",
                        IsTrending = true,
                        Images = new List<PackageImage>
                        {
                            new PackageImage { ImageUrl = "https://images.unsplash.com/photo-1519167758481-83f550bb49b3?w=800" },
                            new PackageImage { ImageUrl = "https://images.unsplash.com/photo-1465495976277-4387d4b0b4c6?w=800" },
                            new PackageImage { ImageUrl = "https://images.unsplash.com/photo-1522673607200-164d1b6ce486?w=800" }
                        }
                    },

                    new Package
                    {
                        EventTypeId = birthday.Id,
                        PackageName = "Birthday Gold",
                        Price = 30000,
                        Description = "Cake + Decoration + Photography",
                        IsTrending = true,
                        Images = new List<PackageImage>
                        {
                            new PackageImage { ImageUrl = "https://images.unsplash.com/photo-1530103862676-de8c9debad1d?w=800" },
                            new PackageImage { ImageUrl = "https://images.unsplash.com/photo-1464349095431-e9a21285b5f3?w=800" }
                        }
                    },

                    new Package
                    {
                        EventTypeId = corporate.Id,
                        PackageName = "Corporate Premium",
                        Price = 150000,
                        Description = "Conference + Lunch + Sound System",
                        IsTrending = false,
                        Images = new List<PackageImage>
                        {
                            new PackageImage { ImageUrl = "https://images.unsplash.com/photo-1511578314322-379afb476865?w=800" },
                            new PackageImage { ImageUrl = "https://images.unsplash.com/photo-1497366216548-37526070297c?w=800" }
                        }
                    }

                );

                context.SaveChanges();
            }
        }
    }
}