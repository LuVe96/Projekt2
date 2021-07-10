using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAnimations : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip idleAnimation;
    [SerializeField] AnimationClip attackAnimation;

    private AnimatorOverrideController animatorOverrideController;

    // Start is called before the first frame update
    void Start()
    {

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        if(idleAnimation != null)
        {
            animatorOverrideController[animatorOverrideController.animationClips[0].name] = idleAnimation;
        }

        if(attackAnimation != null)
        {
            animatorOverrideController[animatorOverrideController.animationClips[1].name] = attackAnimation;
        }
    }

  
}
