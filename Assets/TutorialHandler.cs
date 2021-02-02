using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialHandler : MonoBehaviour
{

    public GameObject tutorialPanlel;
    public VideoPlayer videoPlayer;
    public Text textField;

    public Slide[] slides;

    int currentSlideIndex = 0;


    public void NextSlide()
    {
        if(slides.Length > currentSlideIndex + 1)
        {
            videoPlayer.Pause();
            videoPlayer.clip = slides[currentSlideIndex + 1].videoClip;
            videoPlayer.Play();
            textField.text = slides[currentSlideIndex + 1].text;
            currentSlideIndex += 1;
        }
        else
        {
            ExitTutorial();
        }

    }

    private void ExitTutorial()
    {
        tutorialPanlel.SetActive(false);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            tutorialPanlel.SetActive(true);
            videoPlayer.clip = slides[0].videoClip;
            videoPlayer.Play();
            textField.text = slides[0].text;

        }
    }
}

[System.Serializable]
public class Slide
{
    public VideoClip videoClip;
    [TextArea(3, 10)]
    public string text;
}
