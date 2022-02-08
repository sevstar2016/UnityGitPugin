using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Main : EditorWindow
{
    [MenuItem("Git/Main")]
    public static void OpenWindow()
    {
        GetWindow<Main>();
    }
    
    
    private void OnEnable()
    {
#if UNITY_EDITOR_OSX
        ExecuteProcessTerminal("ioreg -l | awk '/IOPlatformSerialNumber/ { print $4;}'", "/bin/bash");
#endif
#if UNITY_EDITOR_WIN
        ExecuteProcessTerminal("git add .", "/cmd");
        ExecuteProcessTerminal("git commit -m \"second commit\"", "/cmd");
        ExecuteProcessTerminal("git push", "/cmd");
#endif
    }
    private void ExecuteProcessTerminal(string argument, string term)
    {
        try
        {
            UnityEngine.Debug.Log("============== Start Executing [" + argument + "] ===============");
            ProcessStartInfo startInfo = new ProcessStartInfo(term)
            {
                WorkingDirectory = Environment.CurrentDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            Process myProcess = new Process
            {
                StartInfo = startInfo
            };
            myProcess.StartInfo.Arguments = argument;
            myProcess.Start();
            string output = myProcess.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log("Result for [" + argument + "] is : \n" + output);
            myProcess.WaitForExit();
            UnityEngine.Debug.Log("============== End ===============");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnGUI()
    {
        
    }
}
