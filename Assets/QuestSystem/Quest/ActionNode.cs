using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem.Quest
{
    public abstract class ActionNode : QuestNodeData
    {

        public  abstract void executeAction();
    }

}