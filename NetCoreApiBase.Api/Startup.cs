using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using Microsoft.OpenApi.Models;
using NetCoreApiBase.Domain;
using Microsoft.AspNetCore.HttpOverrides;
using AutoMapper;
using NetCoreApiBase.Contracts;
using NetCoreApiBase.RepositoryADO;
using NetCoreApiBase.Api.Hubs;
using NetCoreApiBase.Api.Services;

namespace NetCoreApiBase.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string CorsSpecificationOrigins = "_corsSpecificationOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.ConfigureIISIntegration();//added

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
            });

            //add cache to all API. Be aware to use this.
            //services.AddResponseCaching();

            services.ConfigureRepositoryWrapper();

            services.AddControllers();

            services.AddAutoMapper(typeof(Startup));

            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;//mudei pra true
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = appSettings.ValidIssuer,
                    ValidAudience = appSettings.ValidAudience
                };
            });

            //services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database"));
            //services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("connectionString")));
            //services.AddScoped<DataContext, DataContext>();//ja é coberto no AddDbContext por default

            services.AddScoped<IEmployeeRepositoryADO, EmployeeRepositoryADO>();


            //adding service of Repository Context:
            services.AddDbContext<RepositoryContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("connectionStringEF")));


            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiNetCore Info", Version = "v1" }); });

            services.AddSignalR();

            services.AddHostedService<UpdateStockPriceHostedService>();

            //alternative way of configure CORS:
            services.ConfigureCors();//added
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Net Core V1");
            });
            //https://localhost:5001/swagger/v1/swagger.json  api json pelo postman
            //https://localhost:5001/swagger/index.html    api grafica pelo browser.
            //comand de install do swashbuckle:
            //Install-Package Swashbuckle.AspNetCore -Version 5.0.0-rc4

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHub<ChatHub>("/chatHub");
            });

            //alternative way of setup cors:
            //app.UseCors("CorsPolicyForDashboard");
            app.UseCors("CorsPolicy");

            app.UseSignalR(route => {
                route.MapHub<RealtimeBrokerHub>("/v1/realtimebrokerhub");
                route.MapHub<ChatMessageHub>("/v1/chatmessagehub");
            });
        }
    }
}
