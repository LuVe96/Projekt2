using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectManager : MonoBehaviour
{
    public static HitEffectManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);  // the Singelton Obj gets not deleted when change szene
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [HideInInspector]
    public List<OnHitEffect> currentHitEffects { get; private set; } = new List<OnHitEffect>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddEffect(OnHitEffect onHitEffect, float duration)
    {
        StartCoroutine(AddEffectVorDuration( onHitEffect, duration));
    }

    IEnumerator AddEffectVorDuration(OnHitEffect onHitEffect, float duration)
    {
        currentHitEffects.Add(onHitEffect);
        yield return new WaitForSeconds(duration);

        // Code to execute after the delay
        currentHitEffects.Remove(onHitEffect);
    }
}

public struct OnHitEffect {
    public OnHitEffectType onHitEffectType;
    public float effectTime;
    public float damageOverTime;

    public OnHitEffect(OnHitEffectType onHitEffectType, float effectTime, float damageOverTime)
    {
        this.onHitEffectType = onHitEffectType;
        this.effectTime = effectTime;
        this.damageOverTime = damageOverTime;
    }
}
