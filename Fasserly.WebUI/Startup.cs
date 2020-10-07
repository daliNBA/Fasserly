using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Database.Interface;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Security;
using Fasserly.WebUI.MiddleWare;
using Fasserly.WebUI.ViewModels;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MediatR;
using Fasserly.Infrastructure.Mediator.TrainingMediator;
using AutoMapper;

namespace Fasserly.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
                });
            });

            var connectionString = Configuration.GetConnectionString("FasserlyDatabase");
            services.AddDbContext<DatabaseContext>(option => {

                option.UseLazyLoadingProxies();
                option.UseSqlServer(connectionString, x => x.CommandTimeout(180));
                });
            services.AddAutoMapper(typeof(List.Handler));
            services.AddMediatR(typeof(List.Handler).Assembly);
            services.AddMvc();

            services.AddScoped(typeof(TrainingDataServices));
            services.AddScoped(typeof(Login));
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddControllersWithViews();
            services.AddControllers(opt =>
           {
               //Ajouter l'athentification security
               var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
               opt.Filters.Add(new AuthorizeFilter(policy));

           })
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Create>(); });
            services.AddControllersWithViews();

            //Add identity Core
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]));

            var builder = services.AddIdentityCore<UserFasserly>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddEntityFrameworkStores<DatabaseContext>();
            identityBuilder.AddSignInManager<SignInManager<UserFasserly>>();

            //Add Authorization training Owner Requiremnt
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IsTrainingOwner", policy =>
                {
                    policy.Requirements.Add(new IsOwnerRequirement());
                });
            });
            services.AddTransient<IAuthorizationHandler, IsOwnerRequirementHandler>();

            //Authorize connection with Token bearer (
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });


            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/build";
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleWare>();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            //Order is important
            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseSpaStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });

            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = "clientApp";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseReactDevelopmentServer(npmScript: "start");
            //    }
            //});
        }
    }
}
