using AutoMapper;
using CreateCustomer.Contracts;
using CreateCustomer.Data;
using CreateCustomer.Extensions;
using CreateCustomer.Model;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CreateCustomer.Function
{
    public class CreateCustomer
    {
        private readonly ILogger<CreateCustomer> _logger;
        private readonly IValidator<CreateCustomerRequest> _validator;
        private readonly CustomerDbContext _context;
        private readonly IMapper _mapper;

        public CreateCustomer(ILogger<CreateCustomer> logger,
            IValidator<CreateCustomerRequest> validator,
            CustomerDbContext context,
            IMapper mapper)
        {
            _logger = logger;
            _validator = validator;
            _context = context;
            _mapper = mapper;
        }

        [Function("createCustomer")]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "customers")]
            HttpRequestData request)
        {
            _logger.LogInformation("CreateCustomer");

            var createCustomerRequest = await request.ReadFromJsonAsync<CreateCustomerRequest>();

            if (createCustomerRequest == null)
            {
                return await request.BadRequestAsync("Request can not be empty or null.");
            }

            var validationResults = await _validator.ValidateAsync(createCustomerRequest);
            if (!validationResults.IsValid)
            {
                return await request.BadRequestAsync(validationResults.Errors);
            }

            var customer = _mapper.Map<Customer>(createCustomerRequest);
            await SaveCustomer(customer);

            return await request.Ok(customer);
        }

        private async Task SaveCustomer(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }
    }
}
