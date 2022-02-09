using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Main : EditorWindow
{
    public Vector2 scrollPos = Vector2.zero;
    public List<GitElement> checks = new List<GitElement>();
    private string text = "Commit";
    [MenuItem("Git/Main")]
    public static void MainWindow()
    {
        GetWindow<Main>();
    }

    private void OnFocus()
    {
        UpdateUI();
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

        if (GUILayout.Button("Push"))
        {
            foreach (var gitElement in checks)
            {
                if (gitElement.Enable)
                {
                    ExecuteProcessTerminal("add " + gitElement.path.Substring(2), "git");
                }
            }
            ExecuteProcessTerminal("commit -m \"" + text + "\"", "git");
            ExecuteProcessTerminal("push", "git");
            UpdateUI();
        }

        if (GUILayout.Button("Pull"))
        {
            Debug.Log(ExecuteProcessTerminal("pull", "git"));
        }
    }
    
    public static string ExecuteProcessTerminal(string argument, string term)
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
        var str = ExecuteProcessTerminal("diff --name-status", "git").Split('\n').ToList();
        str.Remove(str[str.Count - 1]);
        var st = ExecuteProcessTerminal("ls-files --others --exclude-standard", "git").Split('\n').ToList();
        str.Remove(st[st.Count - 1]);
        for (int i = 0; i < st.Count; i++)
        {
            st[i] = "A	" + st[i];
        }
        str.AddRange(st);
        str.Remove(st[st.Count - 1]);
        foreach (var s in str)
        {
            try
            {
                checks.Add(new GitElement(true, s));
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
