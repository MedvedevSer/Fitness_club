namespace Domain.ValueObjects.Exceptions;

internal static class ExceptionMessages
{
    public const string USERNAME_NOT_NULL_OR_WHITE_SPACE = "Username must not be null, empty or consist only of white-space characters";
    public const string TITLE_NOT_NULL_OR_WHITE_SPACE = "Training title must not be null, empty or consist only of white-space characters";
    public const string DESCRIPTION_NOT_NULL_OR_WHITE_SPACE = "Description must not be null, empty or consist only of white-space characters";
    public const string DURATION_MUST_BE_POSITIVE = "Duration must be positive";
    public const string DURATION_EXCEEDS_LIMIT = "Duration cannot exceed 180 minutes";
    public const string START_TIME_REQUIRED = "Start time is required";
}