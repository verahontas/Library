using Castle.Core.Configuration;
using Library.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;

namespace Library.Service.Test
{
	public class TestStartup
	{
		
		public TestStartup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Dependency injection beállítása az adatbázis kontextushoz
			services.AddDbContext<LibraryContext>(options =>
				options.UseInMemoryDatabase("LibraryTest"));

			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			// adatok inicializációja
			var dbContext = serviceProvider.GetRequiredService<LibraryContext>();
			dbContext.Database.EnsureCreated();
			//dbContext.Books.AddRange(LibraryIntegrationTest.BookData);
			//dbContext.Tomes.AddRange(LibraryIntegrationTest.TomeData);
			//dbContext.Loans.AddRange(LibraryIntegrationTest.LoanData);
			dbContext.SaveChanges();
		}
	}
}
