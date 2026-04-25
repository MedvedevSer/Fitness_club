using Domain.ValueObjects.Base;
using Domain.ValueObjects.Validators;

namespace Domain.ValueObjects;

public class Description(string description) : ValueObject<string>(new DescriptionValidator(), description);