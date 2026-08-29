============================================================
   Cental Car Rental — ASP.NET Core MVC (.NET 8)
   Website + ASP.NET Core Identity + SQL Server
============================================================

╔══════════════════════════════════════════════════════════╗
║  SUPER QUICK START (no manual DB commands needed)          ║
╚══════════════════════════════════════════════════════════╝

STEP 1 — Copy template assets into wwwroot/
   From the original Cental template, copy:
      img/  ->  wwwroot/img/     (KEEP the vehicle-*.jpg files that
                                  are already there — they are real car
                                  photos. Just add the template's other
                                  images like carousel-*, team-*, etc.)
      lib/  ->  wwwroot/lib/

STEP 2 — Open CentalMvc.csproj in Visual Studio
   NuGet packages restore automatically (needs internet first time).

STEP 3 — Press F5.  That's it.
   The database is created and seeded AUTOMATICALLY on startup.
   No migration commands needed.

   Website:    https://localhost:7188/
   Login:      /Auth/Login
   Admin area: /Admin


╔══════════════════════════════════════════════════════════╗
║  IMPORTANT — the auto-recreate flag                        ║
╚══════════════════════════════════════════════════════════╝
In appsettings.json:

   "DatabaseSettings": { "RecreateOnStartup": true }

• TRUE  = every time the app starts it DELETES and REBUILDS the
          database fresh (fixes any broken/old data). Great for the
          first run and while developing.
• FALSE = keep your data between runs.

>>> Leave it TRUE for now so everything works immediately.
>>> Once it runs fine, set it to FALSE so your bookings/users persist.


╔══════════════════════════════════════════════════════════╗
║  SEEDED ACCOUNTS                                           ║
╚══════════════════════════════════════════════════════════╝
   ADMIN     ->  admin@cental.com  /  Admin@123
   CUSTOMER  ->  john@example.com  /  John@123


╔══════════════════════════════════════════════════════════╗
║  NuGet Packages (already in the .csproj)                   ║
╚══════════════════════════════════════════════════════════╝
   Microsoft.AspNetCore.Identity.EntityFrameworkCore  8.0.8
   Microsoft.AspNetCore.Identity.UI                   8.0.8
   Microsoft.EntityFrameworkCore.SqlServer            8.0.8
   Microsoft.EntityFrameworkCore.Tools                8.0.8
   Microsoft.EntityFrameworkCore.Design               8.0.8


╔══════════════════════════════════════════════════════════╗
║  FEATURES                                                  ║
╚══════════════════════════════════════════════════════════╝
   [x] Real car photos for all 6 seeded cars
   [x] ASP.NET Core Identity (hashed passwords, roles)
   [x] Login + Register + Logout + Remember me
   [x] Role-based authorization on all admin pages
   [x] Booking requires login; booking linked to the user
   [x] "My Bookings" page for customers
   [x] Car details + star ratings + reviews (login to post)
   [x] Working Contact form -> admin "Messages" inbox
   [x] Admin dashboard with live stats
   [x] Cars / Bookings / Users / Messages management
   [x] Auto DB create + seed on startup (no commands)
   [x] Dark neon theme across site + dashboard


╔══════════════════════════════════════════════════════════╗
║  CONTROLLERS (one per concern)                            ║
╚══════════════════════════════════════════════════════════╝
   HomeController      -> public site + booking + reviews + contact
   AuthController      -> Login / Register / Logout / AccessDenied
   AdminController     -> dashboard          [Authorize Admin]
   CarsController      -> car CRUD           [Authorize Admin]
   BookingsController  -> manage bookings    [Authorize Admin]
   UsersController     -> manage users       [Authorize Admin]
   MessagesController  -> contact inbox      [Authorize Admin]


╔══════════════════════════════════════════════════════════╗
║  CONNECTION STRING (appsettings.json)                      ║
╚══════════════════════════════════════════════════════════╝
   LocalDB (default):
     Server=(localdb)\MSSQLLocalDB;Database=CentalDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
   SQL Server Express:
     Server=.\SQLEXPRESS;Database=CentalDb;Trusted_Connection=True;TrustServerCertificate=True

Good luck! 🚗
