namespace DynamicDbContextWeb.Models;

public interface IStateEntity : IAddedEntity, IDeletedEntity, IModifiedEntity
{
}