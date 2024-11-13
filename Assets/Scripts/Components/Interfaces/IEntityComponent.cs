using System;

namespace Components.Interfaces
{
    public interface IEntityComponent : IDisposable
    {
        public void InitializeComponent();
    }
}
