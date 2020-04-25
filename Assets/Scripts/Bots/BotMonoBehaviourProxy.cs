using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeGameChallengeInterfaces;

public abstract class BotMonoBehaviourProxy : MonoBehaviour
{
    protected abstract IPlayer InstantiateBot();

    private IPlayer bot;
    public IPlayer Bot => bot;

    public void Initialize()
    {
        bot = InstantiateBot();    
    }

    public string Name => bot.Name;

    public void Tick(IControlledActor actor, IGameContext context)
    {
        bot.Tick(actor, context);
    }

}
