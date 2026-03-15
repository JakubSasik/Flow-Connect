using System;
using System.Collections.Generic;
using UnityEngine;

public static class FlowHighScoreStore
{
    [Serializable]
    private class ScoreEntry
    {
        public float timeLeft;
        public int   energy;
    }

    [Serializable]
    private class ScoreListWrapper { public List<ScoreEntry> values = new(); }

    public static void SaveScore(int level, float timeLeft, int energy)
    {
        string key  = $"TA_Scores_{level}";
        var    list = Load(key);

        list.Add(new ScoreEntry { timeLeft = timeLeft, energy = energy });

        
        list.Sort((a, b) => b.timeLeft.CompareTo(a.timeLeft));
        if (list.Count > 10) list.RemoveRange(10, list.Count - 10);

        var w = new ScoreListWrapper { values = list };
        PlayerPrefs.SetString(key, JsonUtility.ToJson(w));
        PlayerPrefs.Save();
    }

    public static List<(float timeLeft, int energy)> LoadScores(int level)
    {
        var result = new List<(float, int)>();
        var list   = Load($"TA_Scores_{level}");
        foreach (var e in list)
            result.Add((e.timeLeft, e.energy));
        return result;
    }

    public static void ResetAll()
    {
        for (int i = 1; i <= 6; i++)
            PlayerPrefs.DeleteKey($"TA_Scores_{i}");
        PlayerPrefs.Save();
    }

    static List<ScoreEntry> Load(string key)
    {
        if (!PlayerPrefs.HasKey(key)) return new List<ScoreEntry>();
        var w = JsonUtility.FromJson<ScoreListWrapper>(PlayerPrefs.GetString(key));
        return (w != null && w.values != null) ? w.values : new List<ScoreEntry>();
    }
}