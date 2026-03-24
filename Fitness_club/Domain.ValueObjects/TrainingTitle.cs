using Domain.ValueObjects.Base;
using Domain.ValueObjects.Validators;

namespace Domain.ValueObjects;

public class TrainingTitle : ValueObject<string>
{
    public TrainingTitle(string value) : base(new TrainingTitleValidator(), value)
    {
    }
}