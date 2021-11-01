using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Reshop.Application.Interfaces.Category;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security.GoogleRecaptcha;
using Reshop.Application.Senders;
using Reshop.Application.Services.Category;
using Reshop.Application.Services.Product;
using Reshop.Application.Services.Shopper;
using Reshop.Application.Services.User;
using Reshop.Domain.Interfaces.Category;
using Reshop.Domain.Interfaces.Product;
using Reshop.Domain.Interfaces.Shopper;
using Reshop.Domain.Interfaces.User;
using Reshop.Infrastructure.Context;
using Reshop.Infrastructure.Repository.Category;
using Reshop.Infrastructure.Repository.Product;
using Reshop.Infrastructure.Repository.Shopper;
using Reshop.Infrastructure.Repository.User;
using System;
using Reshop.Application.Interfaces.Conversation;
using Reshop.Application.Interfaces.Discount;
using Reshop.Application.Services.Conversation;
using Reshop.Application.Services.Discount;
using Reshop.Domain.Interfaces.Conversation;
using Reshop.Domain.Interfaces.Discount;
using Reshop.Infrastructure.Repository.Conversation;
using Reshop.Infrastructure.Repository.Discount;

namespace Reshop.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Db Context

            services.AddDbContext<ReshopDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("ReshopDbConnection")));

            #endregion

            #region IoC

            //services 
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IShopperService, ShopperService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IOriginService, OriginService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IColorService, ColorService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IQuestionService, QuestionService>();

            //repository
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IShopperRepository, ShopperRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IOriginRepository, OriginRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IColorRepository, ColorRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();

            services.AddSingleton<IMessageSender, MessageSender>();

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
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.AccessDeniedPath = "/";
            });

            #endregion

            #region Cryptography

            services.AddDataProtection()
                .DisableAutomaticKeyGeneration()
                .SetDefaultKeyLifetime(new TimeSpan(15, 0, 0, 0));

            #endregion

            #region Quartz

            //services.AddSingleton<IJobFactory, JobFactory>();
            //services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            //services.AddSingleton<RemoveCartJob>();
            //services.AddSingleton(new JobSchedule(typeof(RemoveCartJob), "0/5 * * * * ?")); // 0 0 4 15 * ? *  At 04:00:00am, on the 15th day, every month

            //services.AddHostedService<QuartzHostedService>();

            #endregion

            #region Configure

            // should ssl site
            services.Configure<MvcOptions>(options => { options.Filters.Add(new RequireHttpsAttribute()); });

            services.Configure<GoogleReCaptchaKey>(configuration.GetSection("GoogleRecaptcha"));

            #endregion

            #region Prevent Cross-Site Request

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "X-CSRF-TOKEN-RESHOP";
                options.HeaderName = "X-CSRF-TOKEN-RESHOP";
                options.FormFieldName = "X-CSRF-TOKEN-RESHOP";
            });

            #endregion


            return services;
        }
    }
}
