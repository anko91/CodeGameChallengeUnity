using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private PlayerNote _playerNotePrefab;

    private List<PlayerNote> _allPlayers = new List<PlayerNote>();

    public void InitLeaderboard(List<ControlledActor> actors)
    {
        foreach(var actor in actors)
        {
            var playerNote = Instantiate(_playerNotePrefab, transform);
            playerNote.SetActor(actor);
            playerNote.gameObject.SetActive(true);
            _allPlayers.Add(playerNote);
        }
    }

    public string Winner()
    {
        var maxIndex = 0;
        for(var i = 0; i < _allPlayers.Count; i++)
        {
            if(_allPlayers[i].Score > _allPlayers[maxIndex].Score)
            {
                maxIndex = i;
            }
        }
        return _allPlayers[maxIndex].Actor.Owner.Name;
    }

    public void UpdateLeaderboard()
    {
        foreach(var note in _allPlayers)
        {
            note.SetActor(note.Actor);
        }
        Debug.Log("Leaderboard Updated");
    }
}
