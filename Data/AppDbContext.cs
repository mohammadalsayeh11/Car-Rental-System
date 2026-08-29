using CentalMvc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CentalMvc.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<AboutInfo> AboutInfos => Set<AboutInfo>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);  // required for Identity

        builder.Entity<Car>().Property(c => c.PricePerDay).HasColumnType("decimal(18,2)");

        // ---- Seed cars ----
        builder.Entity<Car>().HasData(
            new Car { Id = 1, Name = "VW Golf VII", Brand = "Volkswagen", CarType = "Hatchback", ModelYear = 2023, PricePerDay = 45, Transmission = "Manual", FuelType = "Petrol", Seats = 5, ImageUrl = "/img/vehicle-1.jpg", IsAvailable = true, Description = "Compact, fuel-efficient and fun to drive.", CreatedAt = new DateTime(2025,1,1) },
            new Car { Id = 2, Name = "Audi A1 S-Line", Brand = "Audi", CarType = "Sedan", ModelYear = 2024, PricePerDay = 70, Transmission = "Automatic", FuelType = "Petrol", Seats = 5, ImageUrl = "/img/vehicle-2.jpg", IsAvailable = true, Description = "Premium compact with sporty styling.", CreatedAt = new DateTime(2025,1,1) },
            new Car { Id = 3, Name = "Toyota Camry", Brand = "Toyota", CarType = "Sedan", ModelYear = 2023, PricePerDay = 60, Transmission = "Automatic", FuelType = "Hybrid", Seats = 5, ImageUrl = "/img/vehicle-3.jpg", IsAvailable = true, Description = "Reliable and comfortable family sedan.", CreatedAt = new DateTime(2025,1,1) },
            new Car { Id = 4, Name = "BMW 320 ModernLine", Brand = "BMW", CarType = "Sedan", ModelYear = 2024, PricePerDay = 95, Transmission = "Automatic", FuelType = "Diesel", Seats = 5, ImageUrl = "/img/vehicle-4.jpg", IsAvailable = true, Description = "Executive sedan with dynamic handling.", CreatedAt = new DateTime(2025,1,1) },
            new Car { Id = 5, Name = "Mercedes GLC", Brand = "Mercedes", CarType = "SUV", ModelYear = 2024, PricePerDay = 120, Transmission = "Automatic", FuelType = "Petrol", Seats = 5, ImageUrl = "/img/vehicle-5.jpg", IsAvailable = true, Description = "Luxury SUV with premium interior.", CreatedAt = new DateTime(2025,1,1) },
            new Car { Id = 6, Name = "Porsche 911", Brand = "Porsche", CarType = "Sports", ModelYear = 2024, PricePerDay = 260, Transmission = "Automatic", FuelType = "Petrol", Seats = 2, ImageUrl = "/img/vehicle-6.jpg", IsAvailable = true, Description = "Iconic sports car, pure performance.", CreatedAt = new DateTime(2025,1,1) }
        );

        // ---- Seed reviews (so home page ratings are real) ----
        builder.Entity<Review>().HasData(
            new Review { Id = 1, CarId = 1, UserId = "seed", UserName = "Ahmad K.", Rating = 5, Comment = "Great little car, very economical on fuel.", CreatedAt = new DateTime(2025,2,1) },
            new Review { Id = 2, CarId = 1, UserId = "seed", UserName = "Sara M.", Rating = 4, Comment = "Smooth drive, would rent again.", CreatedAt = new DateTime(2025,2,5) },
            new Review { Id = 3, CarId = 2, UserId = "seed", UserName = "Omar T.", Rating = 5, Comment = "Premium feel for the price.", CreatedAt = new DateTime(2025,2,10) },
            new Review { Id = 4, CarId = 3, UserId = "seed", UserName = "Lina H.", Rating = 4, Comment = "Comfortable and reliable family car.", CreatedAt = new DateTime(2025,2,12) },
            new Review { Id = 5, CarId = 4, UserId = "seed", UserName = "Yousef A.", Rating = 5, Comment = "Loved the handling, felt luxurious.", CreatedAt = new DateTime(2025,2,15) },
            new Review { Id = 6, CarId = 5, UserId = "seed", UserName = "Dana S.", Rating = 5, Comment = "Spacious SUV, perfect for trips.", CreatedAt = new DateTime(2025,2,18) },
            new Review { Id = 7, CarId = 6, UserId = "seed", UserName = "Khaled R.", Rating = 5, Comment = "Unbelievable performance. Worth every penny.", CreatedAt = new DateTime(2025,2,20) }
        );

        // ---- Seed blog posts (so home page blog + Read More work) ----
        builder.Entity<BlogPost>().HasData(
            new BlogPost { Id = 1, Title = "Rental Cars: how to check driving fines?", Author = "Martin.C", CommentsCount = 6, Summary = "A quick guide to checking and settling any driving fines during your rental period.", Content = "When renting a car, it's important to know how to check for any traffic fines that may have been issued during your rental period. Most rental companies will notify you, but you can also check with the local traffic department using the vehicle plate number. Always settle fines promptly to avoid extra administrative charges from the rental company.", ImageUrl = "/img/blog-1.jpg", PublishedDate = new DateTime(2025,12,30), CreatedAt = new DateTime(2025,12,30) },
            new BlogPost { Id = 2, Title = "Rental cost of sport and other cars", Author = "Martin.C", CommentsCount = 6, Summary = "Understand what drives the daily price of sports cars versus everyday vehicles.", Content = "Sports cars command higher daily rental rates due to their higher purchase price, insurance premiums, and maintenance costs. Everyday sedans and hatchbacks are far more affordable and practical for city driving. Choosing the right category depends on your trip: a weekend getaway may justify a sports car, while a family trip is better served by an SUV or sedan.", ImageUrl = "/img/blog-2.jpg", PublishedDate = new DateTime(2025,12,25), CreatedAt = new DateTime(2025,12,25) },
            new BlogPost { Id = 3, Title = "Documents required for car rental", Author = "Martin.C", CommentsCount = 6, Summary = "The essential documents you need to have ready before picking up your rental.", Content = "To rent a car you typically need a valid driving license, a national ID or passport, and a credit or debit card for the deposit. International renters may also need an International Driving Permit. Having these ready speeds up the pickup process and avoids delays at the counter.", ImageUrl = "/img/blog-3.jpg", PublishedDate = new DateTime(2025,12,27), CreatedAt = new DateTime(2025,12,27) }
        );

        // ---- Seed team members ----
        builder.Entity<TeamMember>().HasData(
            new TeamMember { Id = 1, Name = "Martin Doe", Role = "Support Manager", ImageUrl = "/img/team-1.jpg", FacebookUrl = "#", TwitterUrl = "#", InstagramUrl = "#", LinkedInUrl = "#", DisplayOrder = 1, IsActive = true, CreatedAt = new DateTime(2025,1,1) },
            new TeamMember { Id = 2, Name = "Sarah Lee", Role = "Booking Specialist", ImageUrl = "/img/team-2.jpg", FacebookUrl = "#", TwitterUrl = "#", InstagramUrl = "#", LinkedInUrl = "#", DisplayOrder = 2, IsActive = true, CreatedAt = new DateTime(2025,1,1) },
            new TeamMember { Id = 3, Name = "John Smith", Role = "Fleet Advisor", ImageUrl = "/img/team-3.jpg", FacebookUrl = "#", TwitterUrl = "#", InstagramUrl = "#", LinkedInUrl = "#", DisplayOrder = 3, IsActive = true, CreatedAt = new DateTime(2025,1,1) },
            new TeamMember { Id = 4, Name = "Emma Jones", Role = "Customer Care", ImageUrl = "/img/team-4.jpg", FacebookUrl = "#", TwitterUrl = "#", InstagramUrl = "#", LinkedInUrl = "#", DisplayOrder = 4, IsActive = true, CreatedAt = new DateTime(2025,1,1) }
        );

        // ---- Seed about info (single row) ----
        builder.Entity<AboutInfo>().HasData(
            new AboutInfo
            {
                Id = 1,
                Heading = "About Us",
                Intro = "We are a trusted car rental service offering a wide range of well-maintained vehicles for every occasion. From compact city cars to premium SUVs and sports cars, we make renting simple, affordable and reliable.",
                VisionTitle = "Our Vision",
                VisionText = "To make quality car rental accessible and effortless for everyone.",
                MissionTitle = "Our Mission",
                MissionText = "To deliver reliable vehicles and outstanding customer service every time.",
                BodyText = "Our fleet is regularly serviced and our booking process is fully online, so you can reserve a car in just a few clicks and pick it up hassle-free. We pride ourselves on transparent pricing and around-the-clock support.",
                YearsOfExperience = 17,
                Features = "Wide range of vehicles\nTransparent pricing\nEasy online booking\n24/7 customer support",
                FounderName = "William Burgess",
                FounderTitle = "Cental Founder",
                UpdatedAt = new DateTime(2025,1,1)
            }
        );
    }
}
