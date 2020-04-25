using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshPro _text;

    [SerializeField]
    private Transform _followTarget;

    [SerializeField]
    private SpriteRenderer _healthBar;

    public void ShowMessage(string text, float time)
    {
        _text.text = text;
        _text.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(MesssageRoutine(time));
    }

    public void UpdateHealth(int health, int maxHealth)
    {
        var scale = _healthBar.transform.localScale;
        scale.x = Mathf.Clamp01((float)health / (float)maxHealth);
        _healthBar.transform.localScale = scale;
        if (health <= 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    private IEnumerator MesssageRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        _text.gameObject.SetActive(false);
    }

    public void Follow(Transform target)
    {
        _followTarget = target;
    }

    private void Update()
    {
        if (_followTarget != null)
        {
            transform.position = _followTarget.position;
        }
    }

}
