using System;
using System.Collections.Generic;
using UnityEngine;

public static class DDHighScoreStore
{
    [Serializable]
    private class FloatListWrapper
    {
        public List<float> values = new();
    }

    public static List<float> LoadTimes(int level)
    {
        string key = $"DD_Times_{level}";
        if (!PlayerPrefs.HasKey(key))
            return new List<float>();

        var w = JsonUtility.FromJson<FloatListWrapper>(PlayerPrefs.GetString(key));
        return (w != null && w.values != null) ? w.values : new List<float>();
    }

    public static void SaveTime(int level, float time)
    {
        var times = LoadTimes(level);
        times.Add(time);
        times.Sort();

        if (times.Count > 10)
            times.RemoveRange(10, times.Count - 10);

        var w = new FloatListWrapper { values = times };
        PlayerPrefs.SetString($"DD_Times_{level}", JsonUtility.ToJson(w));
        PlayerPrefs.Save();
    }

    public static void ResetAll()
    {
        for (int i = 1; i <= 18; i++)
            PlayerPrefs.DeleteKey($"DD_Times_{i}");

        PlayerPrefs.Save();
    }
}