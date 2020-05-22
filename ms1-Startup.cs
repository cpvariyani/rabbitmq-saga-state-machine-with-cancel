using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using order_ms.Infra;
using rabbitmq_message;

namespace order_ms
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
            services.AddControllers();
        

            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<CancelOrderConsumer>();
                cfg.AddBus(provider => RabbitMqBus.ConfigureBus(provider));
            });

            services.AddMassTransitHostedService();

            services.AddDbContext<OrderDbContext>
        (o => o.UseSqlServer(Configuration.
         GetConnectionString("OrderingDatabase")));

            services.AddSingleton<IOrderDataAccess, OrderDataAccess>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
