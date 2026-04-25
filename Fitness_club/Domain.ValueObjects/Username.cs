using Domain.ValueObjects.Base;
using Domain.ValueObjects.Validators;

namespace Domain.ValueObjects;

public class Username(string name) : ValueObject<string>(new UsernameValidator(), name);