using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HR.Utilities.UI
{
    public class UIMeterImage : UIMeter
    {
        [SerializeField] Image image;

        protected override void ValueUpdated(object o=null)
        {
            image.fillAmount = GetValue();
        }
    }
}

