using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using achieve_backend.Models;
using Microsoft.Extensions.Options;
using achieve_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace achieve_backend
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
			Configuration["AchieveDBSettings:ConnectionString"] = Configuration["DB_CONN_STRING"];

			services.Configure<AchieveDBSettings>(
				Configuration.GetSection(nameof(AchieveDBSettings)));

			services.AddSingleton<IAchieveDBSettings>(sp =>
				sp.GetRequiredService<IOptions<AchieveDBSettings>>().Value);

			services.AddSingleton<UserService>();
			services.AddSingleton<DomainService>();

			DefineDomains(services.BuildServiceProvider().GetService<DomainService>());

			services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());

			services.AddAuthentication("Bearer")
		   .AddJwtBearer("Bearer", options =>
		   {
			   options.Authority = "http://localhost:5000";
			   options.RequireHttpsMetadata = false;

			   options.Audience = "achieve-api";
		   });

			services.AddCors(options =>
			{
				// this defines a CORS policy called "default"
				options.AddPolicy("default", policy =>
				{
					policy.WithOrigins("http://localhost:5003")
						.AllowAnyHeader()
						.AllowAnyMethod();
				});
			});

			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});


			services.AddAuthentication();

			services.AddMvc(options =>
			{
				var policy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
				options.Filters.Add(new AuthorizeFilter(policy));
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseStaticFiles();

			app.UseCors("default");

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		public void DefineDomains(DomainService ds)
		{
			DomainModel.KeyLength = int.Parse(Configuration["DOMAIN_KEY_LENGTH"]);
			List<DomainModel> domains = new List<DomainModel>() { new DomainModel("it108.local", "IT108") };
			DomainsConfig.DefineDomains(domains, Configuration, ds);
		}
	}
}
