using System.Collections;
using System.Collections.Generic;
using CodeGameChallengeInterfaces;

public class MediumBotProxy : BotMonoBehaviourProxy
{
    protected override IPlayer InstantiateBot()
    {
        return new MediumBot();
    }
}
