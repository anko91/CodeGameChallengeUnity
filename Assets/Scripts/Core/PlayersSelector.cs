using System.Collections.Generic;
using UnityEngine;
using CodeGameChallengeInterfaces;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayersSelector : MonoBehaviour
{
    private List<IPlayer> allPlayers = new List<IPlayer>();
    [SerializeField]
    private List<BotMonoBehaviourProxy> bots = new List<BotMonoBehaviourProxy>();
    
    public void LoadPlayers(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.DLL:
                foreach (var file in Directory.GetFiles("Players/"))
                {
                    if (file.Contains(".dll"))
                    {
                        Debug.Log(file);
                        var player = GetPlayerFromPath(file);
                        Debug.Log(player.Name);
                        allPlayers.Add(player);
                    }
                }
                Debug.Log("Total Players:" + allPlayers.Count);
                break;
            case GameMode.Inner:
                for(var i = 0; i < bots.Count; i++)
                {
                    bots[i] = Instantiate(bots[i], transform);
                    bots[i].Initialize();
                }
                break;
        }
    }

    public int PlayersAmount(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.DLL:
                return allPlayers.Count;
            default:
            case GameMode.Inner:
                return bots.Count;
        }
    }

    public IPlayer GetOwner(int i, GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.DLL:
                return allPlayers[i];
            default:
            case GameMode.Inner:
                return bots[i].Bot;
        }
    }

    public IPlayer GetPlayerFromPath(string path)
    {
        var bytes = System.Text.Encoding.ASCII.GetBytes(path);
        var assembly = System.Reflection.Assembly.LoadFrom(path);
        
        foreach (var type in assembly.DefinedTypes)
        {
            foreach (var nextInterface in type.GetInterfaces())
            {
                if (nextInterface == typeof(IPlayer))
                {
                    Debug.Log("Assembly:" + assembly.FullName + " |Type:" + type.FullName);
                    /*var player = System.Activator.CreateComInstanceFrom(assembly.FullName, 
                        type.FullName,
                        System.Text.Encoding.ASCII.GetBytes(path),
                        System.Configuration.Assemblies.AssemblyHashAlgorithm.None);*/
                    return (IPlayer)(assembly.CreateInstance(type.FullName));
                }
            }
        }
        return null;
    }
}
