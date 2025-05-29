using UnityEngine;

public class EffectRenderer : MonoBehaviour
{
    private SpriteRenderer[] effects;

    private void Awake()
    {
        effects = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnDisable()
    {
        foreach (var effect in effects)
            effect.sprite = null;
    }
}