namespace Domain.FitnessClub.Base;

public abstract class Entity<TId>(TId id) where TId : struct
{
    public TId Id { get; } = id;

    protected Entity() : this(default!) { }
}