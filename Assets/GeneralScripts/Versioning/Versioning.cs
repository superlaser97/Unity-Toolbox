using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUPERLASER
{
    public class Versioning
    {
        public static string GetVersion(bool ignoreVersionPringFlag = false)
        {
            TextAsset txt = (TextAsset)Resources.Load("Versioning", typeof(TextAsset));
            string version = txt.text.Split('-')[0];
            string versionPrintFlag = txt.text.Split('-')[1];

            if (bool.Parse(versionPrintFlag) == true || ignoreVersionPringFlag)
                return version;
            else
                return string.Empty;
        }
    }
}
