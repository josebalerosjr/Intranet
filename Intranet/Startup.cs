using Hangfire;
using Intranet.Classes;
using Intranet.Controllers;
using Intranet.Data;
using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository;
using Intranet.DataAccess.Repository.CorpComm;
using Intranet.DataAccess.Repository.IRepository;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models;
using Intranet.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;

namespace Intranet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConfig = configuration;
        }

        public IConfiguration Configuration { get; }
        public static IConfiguration StaticConfig { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            #region variable declaration for appsettings and connections string

            var appSettings = Configuration.GetSection("AppSettings");
            var bdoInfo = Configuration.GetSection("BdoInfo");
            var bpiInfo = Configuration.GetSection("BpiInfo");
            var sbcInfo = Configuration.GetSection("SbcInfo");
            var mtrInfo = Configuration.GetSection("MtrInfo");
            var appLinks = Configuration.GetSection("AppLinks");
            var onlineimagelinks = Configuration.GetSection("OnlineImageLinks");

            var connection = Configuration.GetConnectionString("DevConnection");
            var Conn_CorpComm = Configuration.GetConnectionString("DevCorpComm");
            var connectionHangfire = Configuration.GetConnectionString("HangfireConnection");

            #endregion variable declaration for appsettings and connections string

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<ItemRegController>();

            #region context connetion string

            /**
             *  this service is responsible for getting the connection string
             *  from appsettings.json file
             */
            services.AddDbContext<EmailContext>(options =>
            options.UseSqlServer(connection));

            services.AddDbContext<SuggestionContext>(options =>
            options.UseSqlServer(connection));

            services.AddDbContext<InvLocationContext>(options =>
            options.UseSqlServer(connection));

            services.AddDbContext<InvTypeContext>(options =>
            options.UseSqlServer(connection));

            services.AddDbContext<InvUnitContext>(options =>
            options.UseSqlServer(connection));

            services.AddDbContext<InvManufacturerContext>(options =>
            options.UseSqlServer(connection));

            services.AddDbContext<ItemRegContext>(options =>
            options.UseSqlServer(connection));

            services.AddDbContext<ItemRegSHEContext>(options =>
            options.UseSqlServer(connection));

            services.AddDbContext<InvEmailContext>(options =>
            options.UseSqlServer(connection));

            services.AddDbContext<PaymentEntryContext>(options =>
            options.UseSqlServer(connection));

            services.AddDbContext<BdoPEContext>(options =>
            options.UseSqlServer(connection));

            services.AddDbContext<ImageCarouselContext>(options =>
            options.UseSqlServer(connection));

            #endregion context connetion string

            #region CorpCom connetion string

            /**
             *  this service is responsible for getting the connection string
             *  from appsettings.json file
             */
            services.AddDbContext<CorpCommDbContext>(options => options.UseSqlServer(Conn_CorpComm));

            #endregion CorpCom connetion string

            #region appsettings service

            /**
             *  this service gets the value from the appsettings.json file
             *  and put it in a array.
             */
            services.Configure<AppSettings>(appSettings);
            services.Configure<BdoInfo>(bdoInfo);
            services.Configure<BpiInfo>(bpiInfo);
            services.Configure<SbcInfo>(sbcInfo);
            services.Configure<MtrInfo>(mtrInfo);
            services.Configure<AppLinks>(appLinks);
            services.Configure<OnlineImageLinks>(onlineimagelinks);
            services.Configure<ConnectionString>(Configuration.GetSection("ConnectionStrings"));

            #endregion appsettings service

            #region access denied service

            /**
             *  this service is responsible in promting access denied if a user view
             *  an a page that he/she dont have an access.
             */
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = new PathString("/Account/AccessDenied");
                });

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            //        options =>
            //         {
            //             options.AccessDeniedPath = new PathString("/Account/AccessDenied");
            //         });

            #endregion access denied service

            #region Hangfire service

            // Start of Hangfire service
            //services.AddHangfire(configuration => configuration
            //    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            //    .UseSimpleAssemblyNameTypeSerializer()
            //    .UseRecommendedSerializerSettings()
            //    .UseSqlServerStorage(connection, new SqlServerStorageOptions
            //    {
            //        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //        QueuePollInterval = TimeSpan.Zero,
            //        UsePageLocksOnDequeue = true,
            //        DisableGlobalLocks = true
            //    }));
            //services.AddHangfireServer();

            services.AddHangfire(
                config => config.UseSqlServerStorage(connectionHangfire)
            );

            #endregion Hangfire service

            services.AddMvc()
                    .AddNToastNotifyToastr()
                    .AddControllersAsServices();

            services.AddScoped<IGenerateDailyCriticalItemReport, GenerateDailyCriticalItemReport>();
            services.AddScoped<IGenerateCalibrationDate, GenerateCalibrationDate>();
            services.AddScoped<IMondayReminder, MondayReminder>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddSingleton<EmailSender>();
            services.Configure<EmailOptions>(Configuration);

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseNToastNotify();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate<IGenerateDailyCriticalItemReport>(
                critItem => critItem.SendEmail(), Cron.Daily
            );

            RecurringJob.AddOrUpdate<IGenerateCalibrationDate>(
                CalDateItem => CalDateItem.SendMail(), Cron.Daily
            );

            RecurringJob.AddOrUpdate<IMondayReminder>(
                MondayReminder => MondayReminder.SendEmail(), Cron.Daily
            ); 

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "CorpComm",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}