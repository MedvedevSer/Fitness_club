using Domain.ValueObjects.Base;
using Domain.ValueObjects.Validators;

namespace Domain.ValueObjects;

public class Description : ValueObject<string>
{
    public static int MAX_LENGTH => 500;

    public Description(string value) : base(new DescriptionValidator(), value)
    {
    }
}