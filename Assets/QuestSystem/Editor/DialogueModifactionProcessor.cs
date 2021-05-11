using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace QuestSystem.Dialogue.Editor
{
    public class DialogueModifactionProcessor : AssetModificationProcessor
    {
        //to fix bug: when renaming asset file, the dialogue scriptable object gets not renamed automaticaly
        // that is done here manually
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {

            Dialogue dialogue = AssetDatabase.LoadMainAssetAtPath(sourcePath) as Dialogue;
            //check if modified File is of type dialogue
            if( dialogue == null)
            {
                return AssetMoveResult.DidNotMove;
            }

            // if file is Moved to other Directory, jump out
            if(Path.GetDirectoryName(sourcePath) != Path.GetDirectoryName(destinationPath)){
                return AssetMoveResult.DidNotMove;
            }

            //rename the dialogue scriptable object to new asset file name
            dialogue.name = Path.GetFileNameWithoutExtension(destinationPath);

            return AssetMoveResult.DidNotMove;
        }
    } 
}
