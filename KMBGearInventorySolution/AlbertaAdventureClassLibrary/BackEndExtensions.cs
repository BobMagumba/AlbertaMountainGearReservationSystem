using AlbertaAdventureClassLibrary.BLL;
using AlbertaAdventureClassLibrary.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbertaAdventureClassLibrary
{
    public static class BackEndExtensions
    {
        public static void AddBackendDependencies(this IServiceCollection services,
         Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<aamcgeardbContext>(options);

            services.AddTransient<AlbertaAdventureServices>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<aamcgeardbContext>();

                return new AlbertaAdventureServices(context);
            });
        }
    }
}
