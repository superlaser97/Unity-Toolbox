using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugCommandMethods
{
    public void SetFPS(string parameter)
    {
        int targetFPS = int.Parse(parameter);
        Mathf.Clamp(targetFPS, 10, 120);
        Application.targetFrameRate = targetFPS;
        DebugTools.Log("Curr Target Framerate: " + Application.targetFrameRate);
    }

    public void GameObjectSetActive(string parameter)
    {
        List<string> param = parameter.Split(',').ToList();
        GameObject[] allGO = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject targetGO = null;

        if (allGO.Any(x => x.name == param[0]))
        {
            targetGO = Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name == param[0]).First();
        }
        else
        {
            DebugTools.Log("Cant find GameObject " + parameter[0]);
            return;
        }

        if (targetGO)
        {
            targetGO.SetActive(bool.Parse(param[1]));
            DebugTools.Log(parameter[0] + " is set " + parameter[1]);
        }
    }

    public void SendMessageToAllGO(string parameter)
    {
        GameObject[] allGO = Resources.FindObjectsOfTypeAll<GameObject>();

        if (parameter.Contains(","))
        {
            List<string> parameters = parameter.Split(',').ToList();
            foreach (GameObject go in allGO)
            {
                go.SendMessage(parameters[0], parameters[1], SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            string param = parameter;
            foreach (GameObject go in allGO)
            {
                go.SendMessage(param, SendMessageOptions.DontRequireReceiver);
            }
        }
        DebugTools.Log("Send Message Done");
    }

    public void ListCmd()
    {
        string text = "";
        text += "Entire Command List";
        text += Environment.NewLine;
        text += "=======================";

        foreach (string cmdDesc in DebugTools.Instance.GetEntireCommandList())
        {
            text += Environment.NewLine;
            text += cmdDesc;
            text += Environment.NewLine;
        }
        text += "=======================";

        DebugTools.Log(text, DebugTools.DebugLevel.COMMAND);
    }

    public void SetConsoleFontSize(string parameter)
    {
        DebugTools.Instance.SetDebugFontSize(Mathf.Clamp(int.Parse(parameter), 5, 100));
    }
}
