using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNote : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Text _actorName;

    [SerializeField]
    private UnityEngine.UI.Text _actorScore;

    [SerializeField]
    private UnityEngine.UI.Image _background;

    private ControlledActor _actor;
    public ControlledActor Actor => _actor;

    public int Score => _actor.Info.score;

    public void SetActor(ControlledActor actor)
    {
        _actor = actor;
        transform.SetSiblingIndex(1000000 - actor.Score);
        _background.color = actor.GetColor();
        _actorName.text = actor.Info.name;
        _actorScore.text = actor.Score.ToString();
    }
}
