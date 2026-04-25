using Domain.ValueObjects.Base;
using Domain.ValueObjects.Validators;

namespace Domain.ValueObjects;

public class TrainingTitle(string title) : ValueObject<string>(new TrainingTitleValidator(), title);