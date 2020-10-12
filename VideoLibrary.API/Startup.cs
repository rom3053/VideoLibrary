using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VideoLibrary.Data;

namespace VideoLibrary.API
{
    public class Startup
    {
        private const string AllCors = "All";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddSingleton<VideoStore>();
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Dev"));
            services.AddCors(options => options.AddPolicy(AllCors, build =>
                                                                    build.AllowAnyHeader()
                                                                    .AllowAnyMethod()
                                                                    .AllowAnyOrigin()));
            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 1073741824;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(AllCors);
            app.UseRouting();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
