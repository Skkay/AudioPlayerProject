using System;
using AudioPlayerProject.Areas.Identity.Data;
using AudioPlayerProject.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(AudioPlayerProject.Areas.Identity.IdentityHostingStartup))]
namespace AudioPlayerProject.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AudioPlayerProjectContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AudioPlayerProjectContextConnection")));

                services.AddDefaultIdentity<AudioPlayerProjectUser>(options => 
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredUniqueChars = 0;
                })
                    .AddEntityFrameworkStores<AudioPlayerProjectContext>();
            });
        }
    }
}