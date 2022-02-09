using UnityEditor;
using UnityEngine;

public class Login : EditorWindow
{
    private string username = "";
    private string email = "";
    
    [MenuItem("Git/Login")]
    public static void LoginWindow()
    {
        GetWindow<Login>();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
            GUILayout.Label("Username: ", GUILayout.Width(70));
            username = GUILayout.TextField(username);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
            GUILayout.Label("email: ", GUILayout.Width(70));
            email = GUILayout.TextField(email);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Login"))
        {
            Main.ExecuteProcessTerminal("config user.name " + username, "git");
            Main.ExecuteProcessTerminal("config user.email " + email, "git");
        }
    }
}
