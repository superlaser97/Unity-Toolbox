using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUPERLASER
{
    public class AsyncSceneLoaderDemo : MonoBehaviour
    {
        public void LoadTargetScene()
        {
            AsyncSceneLoader.AsyncLoadScene("DemoScene-End");
        }
    }
}


