using Components.Interfaces;
using Units.Enums;

namespace Interfaces
{
    public interface IEntity
    {
        public IEntity AddEntityComponent(IEntityComponent unitEntityComponent);

        public TComponent GetEntityComponent<TComponent>() where TComponent : IEntityComponent;
    }

    public interface IReadOnlyEntity
    {
        public TComponent GetEntityComponent<TComponent>() where TComponent : IEntityComponent;
    }
}
