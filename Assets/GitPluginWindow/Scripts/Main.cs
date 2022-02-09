using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Main : EditorWindow
{
    public Vector2 scrollPos = Vector2.zero;
    public List<bool> checks = new List<bool>() {true};
    
    [MenuItem("Git/Main")]
    public static void MainWindow()
    {
        GetWindow<Main>();
    }
    
    private void OnEnable()
    {
        
    }

    private void OnGUI()
    {
        scrollPos = GUI.BeginScrollView(new Rect(new Vector2(5,30), new Vector2(250, 500)), scrollPos,
            new Rect(new Vector2(0, 0), new Vector2(240, maxSize.y)), false, false);

        checks[0] = GUI.Toggle(new Rect(new Vector2(5,5), new Vector2(maxSize.x,10)), checks[0], "123");
        
        GUI.EndScrollView();
        
        if (GUILayout.Button("Add"))
        {
            ExecuteProcessTerminal("add .", "git");
        }
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
                CreateNoWindow = true,
                WorkingDirectory = Environment.CurrentDirectory
            };

            Process myProcess = new Process
            {
                StartInfo = startInfo
            };
            startInfo.Arguments =  argument; 
            myProcess.StartInfo = startInfo; 
            myProcess.Start(); 
            string output = myProcess.StandardOutput.ReadToEnd(); 
            Debug.Log(output); 
            myProcess.WaitForExit();
            return output;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e);
            return null;
        }
    }
}
