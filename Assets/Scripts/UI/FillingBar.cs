using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FillingBar : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        private float _maxValue;
        
        public void Initialize(float maxValue)
        {
            _maxValue = maxValue;
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
        
        public void SetColor(Color color)
        {
            _fillImage.color = color;
        }
        
        public void SetFillingValue(float currentValue)
        {
            _fillImage.fillAmount = GetPercent(currentValue);
        }

        private float GetPercent(float currentValue)
        {
            return currentValue/_maxValue;
        }
    }
}
