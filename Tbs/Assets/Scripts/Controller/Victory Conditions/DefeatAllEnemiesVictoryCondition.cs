using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatAllEnemiesVictoryCondition : BaseVictoryCondition
{
    protected override void CheckForGameOver()
    {
        // In the case of enemy and hero
        // team wiped out hero team loses.
        base.CheckForGameOver();
        if (Victor == Alliances.None && PartyDefeated(Alliances.Enemy))
            Victor = Alliances.Hero;
    }
}
