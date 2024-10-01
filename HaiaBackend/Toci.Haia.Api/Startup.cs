using Microsoft.EntityFrameworkCore;
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
            services.AddDbContext<ComedyDbContext>();
            //    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddScoped<ICommentService, CommentService>();



            services.AddControllers();
        }

    }
}
