using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FadeOnTouch : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Coroutine _fadingCorountine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_fadingCorountine == null && collision.gameObject.GetComponent<PlayerController>()?.GetStandingPlatform() == transform)
        {
            _fadingCorountine = StartCoroutine(Fading());
        }
    }

    private IEnumerator Fading()
    {
        var color = _spriteRenderer.color;

        for (var alpha = 10; alpha >= 0; alpha -= 1)
        {
            color.a = (float)alpha / 10;
            _spriteRenderer.color = color;

            yield return new WaitForSeconds(.2f);
        }

        Destroy(gameObject);
    }
}