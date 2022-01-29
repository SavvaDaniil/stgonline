using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using STG.Component;

namespace STG
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;//int.MaxValue
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });


            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
            });

            services.AddDbContext<Data.ApplicationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<Data.ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddAuthentication("UserCookie")
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "UserCookie";
            })
                .AddCookie("UserCookie", "UserCookie", config =>
                {
                    //config.Cookie.Name = "ApplicationCookie";
                    //config.LoginPath = "/Home/Login";
                    config.Cookie.Name = "UserCookie";
                    config.LoginPath = new PathString("/Home/Login");
                    config.ExpireTimeSpan = TimeSpan.FromMinutes(14400);
                    config.AccessDeniedPath = "/Error/UnAuthorized";
                    config.LogoutPath = "/Home/Login";
                })

            //services.AddAuthentication("AdminCookie")
                .AddCookie("AdminCookie", "AdminCookie", config =>
                {
                    //config.Cookie.Name = "AdminCookie";
                    //config.LoginPath = "/admin/login";
                    config.LoginPath = new PathString("/admin/login");
                    config.ExpireTimeSpan = TimeSpan.FromMinutes(14400);
                    config.AccessDeniedPath = "/Error/UnAuthorized";
                    config.LogoutPath = "/admin/login";
                })

                .AddJwtBearer("UserJWT", "UserJWT", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthUserJWTOptions.ISSUER,

                        ValidateAudience = true,
                        ValidAudience = AuthUserJWTOptions.AUDIENCE,

                        ValidateLifetime = false,

                        // установка ключа безопасности
                        IssuerSigningKey = AuthUserJWTOptions.GetSymmetricSecurityKey(),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy =>
                    policy.RequireRole("Admin")
                    
                );
                options.AddPolicy("RequireUserRole", policy =>
                    policy.RequireRole("User").RequireClaim("UserCookie")
                );
                options.AddPolicy("RequireUserJWTRole", policy =>
                    policy.RequireRole("User").RequireClaim("UserJWT")
                );
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();//для перехвата domain
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                    context.Context.Response.Headers.Add("Expires", "-1");
                    context.Context.Response.Headers["Cache-Control"] = "no-cache, no-store";
                    context.Context.Response.Headers["Pragma"] = "no-cache";
                    context.Context.Response.Headers["Expires"] = "-1";
                }
            });

            /*
            app.UseWhen(context => 
            {
                context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = null;
            });
            */

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("index2", "index2", new { controller = "Home", action = "Index2" });
                routes.MapRoute("login","login", new {controller = "Home", action = "Login"});
                routes.MapRoute("registration", "registration", new { controller = "Home", action = "Registration" });
                routes.MapRoute("forget", "forget", new { controller = "Home", action = "Forget" });
                routes.MapRoute("privacy", "privacy", new { controller = "Home", action = "Privacy" });
                routes.MapRoute("agreement", "agreement", new { controller = "Home", action = "Agreement" });
                routes.MapRoute("test", "test", new { controller = "Home", action = "Test" });


                //routes.MapRoute("auth-registration", "auth/registration", new { controller = "Auth", action = "Registration" });
                //routes.MapRoute("lessons","lessons", new { controller = "Lesson", action = "Lessons" });

                routes.MapRoute("profile", "profile", new { controller = "Home", action = "Profile" });
                routes.MapRoute("packages", "packages", new { controller = "Package", action = "Packages" });

                routes.MapRoute("teachers", "teachers", new { controller = "Teacher", action = "Teachers" });


                routes.MapRoute("admin", "admin", new { area = "Admin", controller = "Admin", action = "Index" });
                routes.MapRoute("admin/login", "admin/login", new { area = "Admin", controller = "Admin", action = "Login" });
                routes.MapRoute("admin/logout", "admin/logout", new { area = "Admin", controller = "Admin", action = "Logout" });

                routes.MapRoute("admin/teachers", "admin/teachers", new { area = "Admin", controller = "Admin", action = "Teachers" });
                routes.MapRoute("admin/styles", "admin/styles", new { area = "Admin", controller = "Admin", action = "Styles" });
                routes.MapRoute("admin/lessontypes", "admin/lessontypes", new { area = "Admin", controller = "Admin", action = "LessonTypes" });
                routes.MapRoute("admin/videos", "admin/videos", new { area = "Admin", controller = "Admin", action = "Videos" });
                routes.MapRoute("admin/packages", "admin/packages", new { area = "Admin", controller = "Admin", action = "Packages" });
                routes.MapRoute("admin/users", "admin/users", new { area = "Admin", controller = "Admin", action = "Users" });
                routes.MapRoute("admin/admins", "admin/admins", new { area = "Admin", controller = "Admin", action = "Admins" });
                routes.MapRoute("admin/homeworks", "admin/homeworks", new { area = "Admin", controller = "Admin", action = "Homeworks" });
                routes.MapRoute("admin/mentoring", "admin/mentoring", new { area = "Admin", controller = "Admin", action = "Mentoring" });
                routes.MapRoute("admin/statements", "admin/statements", new { area = "Admin", controller = "Admin", action = "Statements" });
                //routes.MapRoute("admin/teacher", "admin/teacher", new { area = "Admin", controller = "Admin", action = "Teacher"  });
                //routes.MapRoute("admin/teacher", "admin/teacher", "{area:exists}/{controller=Admin}/{action=Teacher}/{id?}");

                routes.MapRoute("api/packagelesson/add", "api/packagelesson/add", new { controller = "ApiPackageLesson", action = "add" });

                routes.MapRoute("admin/lessons", "admin/lessons", new { area = "Admin", controller = "Admin", action = "Lessons" });


                routes.MapRoute("secret", "secret", new { controller = "Home", action = "Secret" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
