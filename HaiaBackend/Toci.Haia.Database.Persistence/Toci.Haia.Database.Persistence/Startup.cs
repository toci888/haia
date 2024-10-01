using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toci.Haia.Database.Persistence
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Dodaj konfigurację DbContext z poprawnym połączeniem do bazy danych
            services.AddDbContext<ComedyDbContext>(options =>
                options.UseNpgsql("Host=localhost;Database=Toci.Haia;Username=postgres;Password=beatka"));
                //options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Inne usługi...
        }
    }

}
