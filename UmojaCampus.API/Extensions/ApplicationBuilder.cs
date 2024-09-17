using UmojaCampus.Business.Data;

namespace UmojaCampus.API.Extensions
{
    public static class ApplicationBuilder
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                //The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts
                app.UseHsts();
            }

            return app;
        }

        public static IApplicationBuilder DatabaseInitializer(this IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var databaseInitializer = services.GetRequiredService<IDatabaseSeeder>();
                databaseInitializer.SeedAsync().Wait();
            }

            return app;
        }
    }
}
