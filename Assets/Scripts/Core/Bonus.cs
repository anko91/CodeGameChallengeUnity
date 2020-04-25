using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeGameChallengeInterfaces;

public class Bonus : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D _collider;

    [SerializeField]
    private int healValue;

    [SerializeField]
    private int bonusScoreValue;

    public float Radius => _collider.radius * transform.localScale.x;

    private void Awake()
    {
        GameController.Instance.RegisterBonus(this);
    }

    private void OnDestroy()
    {
        GameController.Instance.UnregisterBonus(this);
    }

    public BonusInfo GetInfo()
    {
        return new BonusInfo(position: transform.position.ToVector2(),
            radius: Radius,
            bonusHealValue: healValue,
            bonusScoreValue: bonusScoreValue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var actor = collision.gameObject.GetComponent<ControlledActor>();
        if (actor != null)
        {
            Collect(actor);
        }
    }

    public void Collect(ControlledActor actor)
    {
        actor.Heal(healValue);
        actor.AddScore(bonusScoreValue);
        Destroy(this.gameObject);
    }
}
