using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestLogManager : MonoBehaviour
    {

        [SerializeField] GameObject logPager;
        [SerializeField] GameObject detailsPage;



        // Start is called before the first frame update
        void Start()
        {

        }

        public void OpenLogPager()
        {
            logPager.SetActive(true);
        }

        public void CloseLogDetails()
        {
            detailsPage.SetActive(false);
        }

    }

}