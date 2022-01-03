using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CreateCustomer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using CreateCustomer.Mapping;
using FluentValidation;
using CreateCustomer.Contracts;

namespace CreateCustomer
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults() 
                .ConfigureServices(services =>
                {
                    // Add Required Services Here
                    services.AddAutoMapper(options =>
                        options.AddProfile<CustomerProfile>());
                    services.AddDbContext<CustomerDbContext>(
                        options =>
                            options.UseInMemoryDatabase("customers"));
                    services.AddValidatorsFromAssembly(
                        typeof(CreateCustomerRequest).Assembly);
                })
                .Build();

            host.Run();
        }
    }
}