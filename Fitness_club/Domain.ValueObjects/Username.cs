using Domain.ValueObjects.Base;
using Domain.ValueObjects.Validators;

namespace Domain.ValueObjects;

public class Username : ValueObject<string>
{
    public Username(string value) : base(new UsernameValidator(), value)
    {
    }
}