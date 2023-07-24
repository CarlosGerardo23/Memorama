using Fusion;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoramaCard : NetworkBehaviour, IInteractable
{
    [SerializeField] private Color _onInteractColor;
    [SerializeField] private Color _normalColor;
    [SerializeField] private float _duration;

    [Networked(OnChanged = nameof(OnBallSpawned))]
    public NetworkBool spawned { get; set; }
    private SpriteRenderer _spriteRenderer;

    public static void OnBallSpawned(Changed<MemoramaCard> changed)
    {
        changed.Behaviour._spriteRenderer.color = Color.blue;
    }
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public override void Render()
    {
        _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, Color.white, Time.deltaTime);
    }
    public void OnInteract()
    {
        spawned = !spawned;
        //StartCoroutine(ChangeColor());
    }

  public IEnumerator ChangeColor()
    {
        _spriteRenderer.DOColor(_onInteractColor, _duration);
        yield return new WaitForSeconds(0.5f);
        _spriteRenderer.DOColor(_normalColor, _duration);
    }
}
