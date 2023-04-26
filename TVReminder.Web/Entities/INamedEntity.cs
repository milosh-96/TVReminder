namespace TVReminder.Web.Entities
{
    public interface INamedEntity : IEntity
    {
        string Name { get; }
    }
}
