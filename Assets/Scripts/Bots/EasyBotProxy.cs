using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeGameChallengeInterfaces;

public class EasyBotProxy : BotMonoBehaviourProxy
{
    protected override IPlayer InstantiateBot()
    {
        return new EasyBot();
    }
}
