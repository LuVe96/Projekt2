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
            animator.SetFloat("ShotSpeedMultiplier", 0.5f);
        }

        if(attackAnimation != null)
        {
            animatorOverrideController[animatorOverrideController.animationClips[1].name] = attackAnimation;
        }
    }

    public void setAnimationClip(AnimationClip clip, CustomAnimationStateType type)
    {
        int index = 0;
        switch (type)
        {
            case CustomAnimationStateType.Idle:
                index = 0;
                break;
            case CustomAnimationStateType.Attack:
                index = 1;
                break;
            default:
                index = 0;
                break;
        }
        animatorOverrideController[new AnimatorOverrideController(animator.runtimeAnimatorController).animationClips[index].name] = clip;
    }
}

public enum CustomAnimationStateType
{
    Idle, Attack
}
