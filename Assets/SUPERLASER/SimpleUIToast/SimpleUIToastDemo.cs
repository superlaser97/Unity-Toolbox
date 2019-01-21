using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Low Zhi Heng
/// Date of Creation: 1 Nov 2018
/// Last Updated: 19 Jan 2019
/// </summary>

namespace SUPERLASER
{
    public class SimpleUIToastDemo : MonoBehaviour
    {
        public void ToastA()
        {
            SimpleUIToast.ShowToast("Toast A");
        }
        
        public void ToastB()
        {
            SimpleUIToast.ShowToast("Toast B");
        }
    }
}


