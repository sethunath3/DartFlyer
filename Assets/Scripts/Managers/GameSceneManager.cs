using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSceneManager {
 
    private static Dictionary<string, string> parameters;
 
    public static void LoadScene(string sceneName, Dictionary<string, string> parameters = null) {
        GameSceneManager.parameters = parameters;
        SceneManager.LoadScene(sceneName);
    }
 
    public static void LoadScene(string sceneName, string paramKey, string paramValue) {
        GameSceneManager.parameters = new Dictionary<string, string>();
        GameSceneManager.parameters.Add(paramKey, paramValue);
        SceneManager.LoadScene(sceneName);
    }
 
    public static Dictionary<string, string> getSceneParameters() {
        return parameters;
    }
 
    public static string getParam(string paramKey) {
        if (parameters == null) return "";
        return parameters[paramKey];
    }
 
    public static void setParam(string paramKey, string paramValue) {
        if (parameters == null)
            GameSceneManager.parameters = new Dictionary<string, string>();
        GameSceneManager.parameters.Add(paramKey, paramValue);
    }
 
}
