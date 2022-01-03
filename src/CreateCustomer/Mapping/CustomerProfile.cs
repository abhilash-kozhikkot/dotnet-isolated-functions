using AutoMapper;
using CreateCustomer.Contracts;
using CreateCustomer.Model;

namespace CreateCustomer.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CreateCustomerRequest, Customer>();
        }
    }
}
