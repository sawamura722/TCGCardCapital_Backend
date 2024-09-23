using Microsoft.EntityFrameworkCore;
using TCGCardCapital.Models;

namespace TCGCardCapital.Data
{
    public class RolesSeeder
    {
        public static void SeedRolesAndAdmin(TcgcardCapitalContext context)
        {
            try
            {
                // Ensure roles are seeded
                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new Role { RoleName = "USER" },
                        new Role { RoleName = "ADMIN" }
                    );
                    context.SaveChanges(); // Save to ensure roles are added
                }

                // Ensure the admin user is seeded
                if (!context.Users.Any(u => u.Email == "admin1@gmail.com"))
                {
                    var adminRoleId = context.Roles.SingleOrDefault(r => r.RoleName == "ADMIN")?.RoleId;

                    if (adminRoleId.HasValue)
                    {
                        context.Users.Add(
                            new User
                            {
                                Username = "admin1",
                                Email = "admin1@gmail.com",
                                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                                RoleId = adminRoleId.Value,
                                SubscriptionStatus = false,
                                CreatedAt = DateTime.UtcNow,
                                Point = 0
                            }
                        );
                        context.SaveChanges(); // Save changes after adding the user
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception (consider using a logging framework)
                throw new Exception("An error occurred while seeding the database: " + ex.Message);
            }
        }
    }
}
