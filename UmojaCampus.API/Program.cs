using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Reflection;
using UmojaCampus.API.ActionFilters;
using UmojaCampus.API.Extensions;
using UmojaCampus.API.Middleware;
using UmojaCampus.Business.Extensions;

var builder = WebApplication.CreateBuilder(args);

try
{
    Log.Information("Application started successfully...");

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddServices(builder.Configuration);
    builder.Services.AddBusinessServices();
    builder.Services.AddIdentityConfiguration();
    builder.Services.AddJwtAuthentication();
    builder.Services.AddAuthorization();

    //Configure global model state validation
    builder.Services.AddMvc(options =>
    {
        options.Filters.Add(typeof(ModelValidationFilter));
    });

    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), lifetime: ServiceLifetime.Transient);
    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.UseExceptionHandling(app.Environment);
    app.UseHttpsRedirection();
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseStaticFiles();
    app.UseCors("AllowUmojaCampusBlazorApp");
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.DatabaseInitializer();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal($"Application failed to start: {ex.Message}");
}
finally
{
    Log.Information("Application shut down...");
    Log.CloseAndFlush();
}

