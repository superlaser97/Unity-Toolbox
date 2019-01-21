using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugCommandMethods
{
    public void SetFPS()
    {
        int targetFPS = int.Parse(DebugTools.Instance.consoleParam);
        Mathf.Clamp(targetFPS, 10, 120);
        Application.targetFrameRate = targetFPS;
        DebugTools.Log("Curr Target Framerate: " + Application.targetFrameRate);
    }

    public void GameObjectSetActive()
    {
        List<string> parameter = DebugTools.Instance.consoleParam.Split(',').ToList();
        GameObject[] allGO = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject targetGO = null;

        if (allGO.Any(x => x.name == parameter[0]))
        {
            targetGO = Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name == parameter[0]).First();
        }
        else
        {
            DebugTools.Log("Cant find GameObject " + parameter[0]);
            return;
        }

        if (targetGO)
        {
            targetGO.SetActive(bool.Parse(parameter[1]));
            DebugTools.Log(parameter[0] + " is set " + parameter[1]);
        }
    }

    public void SendMessageToAllGO()
    {
        GameObject[] allGO = Resources.FindObjectsOfTypeAll<GameObject>();

        if (DebugTools.Instance.consoleParam.Contains(","))
        {
            List<string> parameters = DebugTools.Instance.consoleParam.Split(',').ToList();
            foreach (GameObject go in allGO)
            {
                go.SendMessage(parameters[0], parameters[1], SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            string parameter = DebugTools.Instance.consoleParam;
            foreach (GameObject go in allGO)
            {
                go.SendMessage(parameter, SendMessageOptions.DontRequireReceiver);
            }
        }
        DebugTools.Log("Send Message Done");
    }

    public void ListCmd()
    {
        string text = "";
        text += Environment.NewLine;
        text += Environment.NewLine;
        text += "Entire Command List";
        text += Environment.NewLine;
        text += "=======================";

        foreach (string cmdDesc in DebugTools.Instance.GetEntireCommandDescList())
        {
            text += Environment.NewLine;
            text += Environment.NewLine;
            text += cmdDesc;
            text += Environment.NewLine;
        }
        text += "=======================";
        text += Environment.NewLine;
        text += Environment.NewLine;

        DebugTools.Log(text, DebugTools.DebugLevel.COMMAND);
    }

    public void SetConsoleFontSize()
    {
        DebugTools.Instance.SetDebugFontSize(Mathf.Clamp(int.Parse(DebugTools.Instance.consoleParam), 5, 100));
    }
}
