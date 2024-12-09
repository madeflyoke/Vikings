namespace Components.Animation.Interfaces
{
    public interface IAnimatorValueChanger
    {
        public void SetParameterValue(string parameterName, float value);
        public float GetParameterValue(string parameterName);
    }
}
