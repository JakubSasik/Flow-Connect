using System;
using System.Collections.Generic;
using UnityEngine;

public static class HighScoreStore
{
    [Serializable]
    private class FloatListWrapper { public List<float> values = new(); }

    [Serializable]
    private class IntListWrapper { public List<int> values = new(); }

    // ---------- TIME (float) ----------
    public static List<float> LoadTimes(int difficulty)
    {
        string key = $"topTimes_{difficulty}";
        if (!PlayerPrefs.HasKey(key))
            return new List<float>();

        string json = PlayerPrefs.GetString(key, "");
        if (string.IsNullOrEmpty(json))
            return new List<float>();

        var w = JsonUtility.FromJson<FloatListWrapper>(json);
        return (w != null && w.values != null) ? w.values : new List<float>();
    }

    public static void SaveTimes(int difficulty, List<float> times)
    {
        string key = $"topTimes_{difficulty}";
        var w = new FloatListWrapper { values = times };
        PlayerPrefs.SetString(key, JsonUtility.ToJson(w));
        PlayerPrefs.Save();
    }

    public static void AddTime(int difficulty, float newTime)
    {
        var times = LoadTimes(difficulty);
        times.Add(newTime);
        times.Sort();                 // najmenší čas = najlepší
        if (times.Count > 10) times.RemoveRange(10, times.Count - 10);
        SaveTimes(difficulty, times);
    }

    // ---------- CLICKS (int) ----------
    public static List<int> LoadClicks(int difficulty)
    {
        string key = $"topClicks_{difficulty}";
        if (!PlayerPrefs.HasKey(key))
            return new List<int>();

        string json = PlayerPrefs.GetString(key, "");
        if (string.IsNullOrEmpty(json))
            return new List<int>();

        var w = JsonUtility.FromJson<IntListWrapper>(json);
        return (w != null && w.values != null) ? w.values : new List<int>();
    }

    public static void SaveClicks(int difficulty, List<int> clicks)
    {
        string key = $"topClicks_{difficulty}";
        var w = new IntListWrapper { values = clicks };
        PlayerPrefs.SetString(key, JsonUtility.ToJson(w));
        PlayerPrefs.Save();
    }

    public static void AddClicks(int difficulty, int newClicks)
    {
        var clicks = LoadClicks(difficulty);
        clicks.Add(newClicks);
        clicks.Sort();                // najmenej klikov = najlepší
        if (clicks.Count > 10) clicks.RemoveRange(10, clicks.Count - 10);
        SaveClicks(difficulty, clicks);
    }

    // ---------- RESET ----------
    public static void ResetAll()
    {
        for (int d = 0; d < 3; d++)
        {
            PlayerPrefs.DeleteKey($"topTimes_{d}");
            PlayerPrefs.DeleteKey($"topClicks_{d}");
        }
        PlayerPrefs.Save();
    }
}