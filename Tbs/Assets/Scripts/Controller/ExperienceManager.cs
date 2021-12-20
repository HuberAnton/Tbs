using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Party = System.Collections.Generic.List<UnityEngine.GameObject>;


// This is the system that dishes out xp.
// It only occurs at the end of the battle.
// Ideally it should add events on turn change
// which add xp depending on the result of actions.
public static class ExperienceManager
{
    const float minLevelBonus = 1.5f;
    const float maxLevelBonus = 0.5f;

    public static void AwardExperience(int amount, Party party)
    {
        List<Rank> ranks = new List<Rank>(party.Count);
        
        for (int i = 0;i < party.Count; ++i)
        {
            Rank r = party[i].GetComponent<Rank>();
            if (r != null)
                ranks.Add(r);
        }

        int min = int.MinValue;
        int max = int.MaxValue;

        for(int i = ranks.Count - 1; i >= 0; --i)
        {
            // Gets the lowest and highest of the party levels.
            min = Math.Min(ranks[i].LVL, min);
            max = Mathf.Max(ranks[i].LVL, max);
        }

        float[] weights = new float[party.Count];
        float summedWeights = 0;
        for(int i = ranks.Count - 1; i >= 0; --i)
        {
            float percent = (float)(ranks[i].LVL - min) / (float)(max - min);
            weights[i] = Mathf.Lerp(minLevelBonus, maxLevelBonus, percent);
            summedWeights += weights[i];
        }


        for (int i = ranks.Count - 1; i >= 0; --i)
        {
            int subAmount = Mathf.FloorToInt((weights[i] / summedWeights) * amount);
            ranks[i].EXP += subAmount;
        }

    }
}
