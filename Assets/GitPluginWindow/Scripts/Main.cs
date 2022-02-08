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
    public static void MainWindow()
    {
        GetWindow<Main>();
    }
    
    private void OnEnable()
    {
        ExecuteProcessTerminal("add .", "git");
        ExecuteProcessTerminal("commit -m \"second commit\"", "git");
        ExecuteProcessTerminal("push", "git");
    }

    private void OnGUI()
    {
        
    }
    
    private string ExecuteProcessTerminal(string argument, string term)
    {
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = term,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WorkingDirectory = Environment.CurrentDirectory
            };

            Process myProcess = new Process
            {
                StartInfo = startInfo
            };
            
            UnityEngine.Debug.Log("============== Start Executing [" + argument + "] ==============="); 
            startInfo.Arguments =  argument; 
            myProcess.StartInfo = startInfo; 
            myProcess.Start(); 
            string output = myProcess.StandardOutput.ReadToEnd(); 
            Debug.Log(output); 
            myProcess.WaitForExit(); 
            Debug.Log("============== End ===============");
            return output;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e);
            return null;
        }
    }
}
