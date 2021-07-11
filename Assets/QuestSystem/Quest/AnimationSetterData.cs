using System.Collections;
using UnityEngine;

namespace QuestSystem.Quest
{
    [System.Serializable]
    public class AnimationSetterData : ActionNodeData
    {
        [SerializeField] CustomAnimations customAnimations;
        [SerializeField] AnimationClip clip;
        [SerializeField] CustomAnimationStateType type;

        public AnimationSetterData(string id)
        {
            this.uID = id;
        }

        public CustomAnimations CustomAnimations { get => customAnimations; set => customAnimations = value; }
        public AnimationClip Clip { get => clip; set => clip = value; }
        public CustomAnimationStateType Type { get => type; set => type = value; }

        public override void executeAction()
        {
            Debug.Log("Execute Animation Setter Action");

            if (customAnimations != null)
            {
                customAnimations.setAnimationClip(clip, Type);
            }

        }
    }
}