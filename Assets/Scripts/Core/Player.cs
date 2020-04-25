using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeGameChallengeInterfaces;

public class Player : IPlayer
{
    public string Name => "Easy test bot";

    public void Tick(IControlledActor actor, IGameContext context)
    {
        actor.Message("Hello");
    }
    
}
