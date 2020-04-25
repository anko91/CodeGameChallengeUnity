using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeGameChallengeInterfaces;

public class HumanPlayerProxy : BotMonoBehaviourProxy
{
    protected override IPlayer InstantiateBot()
    {
        return new HumanPlayer();
    }
}
