using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HR.Utilities.UI
{
    public class UIMeterSlider : UIMeter
    {
        [SerializeField] Slider slider;

        protected override void ValueUpdated(object o=null)
        {
            slider.value = GetValue();
        }
    }
}

