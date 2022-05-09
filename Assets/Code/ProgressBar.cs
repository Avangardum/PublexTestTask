using System;
using UnityEngine;
using UnityEngine.UI;

namespace Avangardum.PublexTestTask
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _color;

        public float FillAmount
        {
            set
            {
                _color.fillAmount = value;
            }
        }

        public RectTransform RectTransform { get; private set; }

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }
    }
}