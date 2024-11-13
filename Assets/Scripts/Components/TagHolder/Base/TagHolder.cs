using Components.Interfaces;

namespace Components.TagHolder.Base
{
    public abstract class TagHolder : IEntityComponent
    {
        public virtual void InitializeComponent()
        {
            
        }

        public void Dispose()
        {
        }
    }
}
