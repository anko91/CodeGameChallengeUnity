using System.Collections;
using System.Collections.Generic;
using CodeGameChallengeInterfaces;
using UnityEngine;

public class MyBotProxy : BotMonoBehaviourProxy
{
    protected override IPlayer InstantiateBot()
    {
        return new MyBot();
    }
}
