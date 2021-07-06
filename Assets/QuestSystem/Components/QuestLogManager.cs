using QuestSystem.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    public class QuestLogManager : MonoBehaviour
    {

        [SerializeField] GameObject logPager;
        [SerializeField] Transform activeLogs;
        [SerializeField] Transform completedLogs;

        [SerializeField] GameObject detailsPage;
        [SerializeField] Transform detailText;

        [SerializeField] GameObject questLogButtonPrefab;

        [SerializeField] GameObject questLogTextPrefab;

        [SerializeField] Dictionary<string, Logs> questLogs = new Dictionary<string,Logs>(); //TODO: savable



        // Start is called before the first frame update
        void Start()
        {

        }

        public void AddQuestLog(string questLogName,  QuestState state, string logText)
        {
            if (!questLogs.ContainsKey(questLogName))
            { 
                questLogs.Add(questLogName, new Logs(state));
            }

            questLogs[questLogName].LogTexts.Add(logText);
        }

        private void setupQuestLog()
        {
            ClearChildren(activeLogs);
            ClearChildren(completedLogs);

            foreach (var log in questLogs)
            {
                GameObject newLog = null;
                switch (log.Value.State)
                {
                    case QuestState.Inactive:
                        break;
                    case QuestState.Active:
                        newLog = Instantiate(questLogButtonPrefab, activeLogs);
                        break;
                    case QuestState.Failed:
                    case QuestState.Passed:
                        newLog = Instantiate(questLogButtonPrefab, completedLogs);
                        break;
                    default:
                        break;
                }
                if(newLog != null)
                {
                    newLog.GetComponent<Button>().onClick.AddListener(delegate { OnClickLog(log.Key); });
                    newLog.transform.GetChild(0).GetComponent<Text>().text = log.Key;
                }
            }
        }

        private void OnClickLog(string key)
        {
            foreach (string logTxt in questLogs[key].LogTexts)
            {
                GameObject newTxt = Instantiate(questLogTextPrefab, detailText);
                newTxt.transform.GetChild(0).GetComponent<Text>().text = logTxt;
            }
        }

        private void ClearChildren(Transform transform)
        {
            foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }
        }

        public void OpenLogPager()
        {
            logPager.SetActive(true);
            setupQuestLog();
        }

        public void CloseLogDetails()
        {
            detailsPage.SetActive(false);
        }

    }

    public class Logs
    {
        [SerializeField] QuestState state;
        [SerializeField] List<string> logTexts = new List<string>();

        public Logs(QuestState state, List<string> logTexts = null)
        {
            this.state = state;
            if( logTexts != null)
                this.logTexts = logTexts;
        }

        public QuestState State { get => state; set => state = value; }
        public List<string> LogTexts { get => logTexts; set => logTexts = value; }
    }

}