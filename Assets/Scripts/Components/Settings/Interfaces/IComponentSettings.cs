namespace Components.Settings.Interfaces
{
    public interface IComponentSettings
    {
#if UNITY_EDITOR
        public void OnManualValidate();
#endif
    }
}
