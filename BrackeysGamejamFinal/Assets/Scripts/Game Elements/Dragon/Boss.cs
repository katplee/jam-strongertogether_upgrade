using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Things to decide on:
 * *what will we do with the boss??
 * *will there be one boss per level or one overall??
 */

public class Boss : Dragon
{
    public override ElementType Type
    {
        get { return ElementType.BOSS; }
    }
}
