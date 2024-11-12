using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toci.Haia.Database.Persistence;

namespace Toci.Haia.Cry
{
    public class Seed
    {
        private ComedyDbContext cDbContext = new ComedyDbContext();

        public void SeedUsers()
        {
            for (int i = 0; i < 30; i++)
            {
                cDbContext.Users.Add(new User() { PasswordHash = "haslo" + i, Username = "user_seed_" + i, Email = $"seed{i}@seed.com"});
            }

            cDbContext.SaveChanges();
        }
    }
}
