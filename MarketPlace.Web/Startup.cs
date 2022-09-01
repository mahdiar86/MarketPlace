using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using MarketPlace.Application.Services.Implementation;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.Context;
using MarketPlace.Data.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.DataProtection;

namespace MarketPlace.Web
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
            services.AddControllersWithViews();

            #region Service Config

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHelper, PasswordHelper>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ISellerWalletService, SellerWalletService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<INewletterService, NewsletterService>();
            services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

            #endregion

            #region Database Config

            services.AddDbContext<MarketPlaceDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MarketPlaceConnection"));
            },ServiceLifetime.Transient);

            #endregion

            #region Authentication 

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.ExpireTimeSpan = TimeSpan.FromDays(21);
            });

            #endregion

            #region Html Encoder 

            services.AddSingleton<HtmlEncoder>(
                HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));

            #endregion

            #region Data Protection

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory()+"\\wwwroot\\Auth\\"))
                .SetApplicationName("MahdiarProject")
                .SetDefaultKeyLifetime(TimeSpan.FromDays(30));

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
