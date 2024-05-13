using Components.Interfaces;
using Components.TagHolder.Base;
using Interfaces;

namespace Factories.Decorators
{
    public class TagHolderDecorator : IEntityDecorator
    {
        private TagHolder _tagHolder;
        
        public TagHolderDecorator(TagHolder tagHolder)
        {
            _tagHolder = tagHolder;
        }
        
        public IEntityComponent Decorate()
        {
            return _tagHolder;
        }
    }
}
