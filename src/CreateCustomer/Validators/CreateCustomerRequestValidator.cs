using CreateCustomer.Contracts;
using FluentValidation;

namespace CreateCustomer.Validators
{
    public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
    {
        public CreateCustomerRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(10);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(20);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(250);  
        }
    }
}
