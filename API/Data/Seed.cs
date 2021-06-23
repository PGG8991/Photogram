using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return; // If we have users already, stop

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData); // turn json to a List of AppUsers
            if (users == null) return;
            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password"));

                await context.Users.AddAsync(user);
            }

            await context.SaveChangesAsync();
        }
    }
}