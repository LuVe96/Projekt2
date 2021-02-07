using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMagicHandler : MonoBehaviour
{

    public ParticleSystem particle;
    public MagicWaveItem magicWaveItem;
    float scaleVal = 0f;
    float sizeVal = 0f;


    public AnimationCurve animCurve;
    ParticleSystem.MainModule main;

    void Start()
    {
        main = particle.main;
        StartCoroutine(ScaleRoutine());
    }



    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(scaleVal * Time.deltaTime, scaleVal * Time.deltaTime, 0);
        //particle.gameObject.transform.localScale += new Vector3(scaleVal, scaleVal, 0) * Time.deltaTime;
        var shape = particle.shape;
        shape.radius += scaleVal * Time.deltaTime;
        main.startSizeMultiplier = sizeVal;


    }

    private IEnumerator ScaleRoutine()
    {

        float timeStamp = Time.time;
        float duration = 5f;
        while (Time.time < timeStamp + duration)
        {
            float t = (Time.time - timeStamp) / duration;
            t = animCurve.Evaluate(t);

            // xPos will move from 0 to 12, non linearly, following the animation curve, and this over 5 seconds
            scaleVal = Mathf.LerpUnclamped(1f, 5f, t);
            sizeVal = Mathf.LerpUnclamped(1f, 3f, t);
            if(Time.time > timeStamp + duration - 1.5f)
            {
                scaleVal = 0;
                main.loop = false;
            }
            yield return null;
        }


        Destroy(gameObject);
    }
}
