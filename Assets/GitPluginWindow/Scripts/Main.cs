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
    public List<GitElement> checks = new List<GitElement>();
    private string text = "Commit text";
    [MenuItem("Git/Main")]
    public static void MainWindow()
    {
        GetWindow<Main>();
    }
    
    private void OnEnable()
    {
        UpdateUI();
    }

    private void OnGUI()
    {
        GUILayout.BeginScrollView(new Vector2(0, 0), GUILayout.MaxHeight(200));

        foreach (var check in checks)
        {
            check.Enable = GUILayout.Toggle(check.Enable, check.path);
        }
        
        GUILayout.EndScrollView();

        text = GUILayout.TextArea(text, GUILayout.Height(100));
        
        if (GUILayout.Button("Add"))
        {
            foreach (var gitElement in checks)
            {
                Debug.Log(ExecuteProcessTerminal("add " + gitElement.path.Substring(2), "git"));
            }
            UpdateUI();
        }

        if (GUILayout.Button("Commit"))
        {
            Debug.Log(ExecuteProcessTerminal("commit -m \"" + text + "\"", "git"));
        }

        if (GUILayout.Button("Push"))
        {
            Debug.Log(ExecuteProcessTerminal("push", "git"));
            UpdateUI();
        }

        if (GUILayout.Button("Pull"))
        {
            ExecuteProcessTerminal("pull", "git");
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
            myProcess.WaitForExit();
            return output;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e);
            return null;
        }
    }

    private void UpdateUI()
    {
        checks = new List<GitElement>();
        var str = ExecuteProcessTerminal("diff --name-status --diff-filter=ARM", "git").Split('\n');
        foreach (var s in str)
        {
            try
            {
                if (s[0] == 'M' || s[0] == 'A' || s[0] == 'R')
                {
                    checks.Add(new GitElement(true, s));
                }
            }
            catch
            {
                
            }
        }
    }
}

public class GitElement
{
    public bool Enable;
    public string path;
    public GitElement(bool en, string pat)
    {
        Enable = en;
        path = pat;
    }
}
