using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Exception use case for always hit
// and always miss cases when attacking/
public class MatchException : BaseException
{
    public readonly Unit attacker;
    public readonly Unit target;

    public MatchException(Unit attacker, Unit target) : base(false)
    {
        this.attacker = attacker;
        this.target = target;
    }
}
