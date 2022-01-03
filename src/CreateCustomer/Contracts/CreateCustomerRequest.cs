namespace CreateCustomer.Contracts
{
    public record CreateCustomerRequest(
        string Title,
        string FirstName,
        string LastName,
        string Email);
}
