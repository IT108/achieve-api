using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using achieve_backend.Models;
using Microsoft.Extensions.Options;
using achieve_backend.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.AspNetIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

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
			services.Configure<AchieveDBSettings>(
				Configuration.GetSection(nameof(AchieveDBSettings)));

			services.AddSingleton<IAchieveDBSettings>(sp =>
				sp.GetRequiredService<IOptions<AchieveDBSettings>>().Value);

			services.AddSingleton<UserService>();

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
	}
}
