using ABCRetailWebApp.Services;

namespace ABCRetailWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container
            builder.Services.AddControllersWithViews();

            // Use the existing "AzureStorage" connection string for both Table and Blob services
            var connectionString = configuration.GetConnectionString("AzureStorage");
            builder.Services.AddSingleton(new TableStorageService(connectionString));
            builder.Services.AddSingleton(new BlobService(connectionString));
            builder.Services.AddSingleton<QueueService>(sp =>
            {
                var queueConnectionString = configuration.GetConnectionString("AzureStorage");
                return new QueueService(queueConnectionString, "order-events");
            });

            builder.Services.AddSingleton<AzureFileShareService>(sp =>
            {
                var connectionString = configuration.GetConnectionString("AzureStorage");
                    return new AzureFileShareService(connectionString, "documents");


            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}
