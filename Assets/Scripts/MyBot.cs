using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeGameChallengeInterfaces;

public class MyBot : IPlayer
{
    public string Name => "My bot";

    public void Tick(IControlledActor actor, IGameContext context)
    {
    }
}
