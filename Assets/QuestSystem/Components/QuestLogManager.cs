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
        public static QuestLogManager Instance = null;
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

        [SerializeField] GameObject logPager;
        [SerializeField] Transform activeLogContent;
        [SerializeField] Transform completedLogContent;

        [SerializeField] GameObject detailsPage;
        [SerializeField] Transform detailContent;
        [SerializeField] Text detailsHeaderText;

        [SerializeField] GameObject questLogButtonPrefab;
        [SerializeField] GameObject questLogTextPrefab;

        [SerializeField] Dictionary<string, Logs> questLogs = new Dictionary<string,Logs>(); //TODO: savable



        // Start is called before the first frame update
        void Start()
        {

        }

        public void AddQuestLog(string questLogName, string logText)
        {
            if (!questLogs.ContainsKey(questLogName)) return;

            questLogs[questLogName].LogTexts.Add(logText);
        }

        public void AddQuestAsLog(string questLogName)
        {
            if (!questLogs.ContainsKey(questLogName))
            {
                questLogs.Add(questLogName, new Logs(QuestState.Active));
            }
        }

        public void CloseQuestAsLog(string questLogName, QuestState state)
        {
            if (!questLogs.ContainsKey(questLogName))
            {
                questLogs.Add(questLogName, new Logs(state));
            }
        }

        private void setupQuestLog()
        {
            ClearChildren(activeLogContent);
            ClearChildren(completedLogContent);

            foreach (var log in questLogs)
            {
                GameObject newLog = null;
                switch (log.Value.State)
                {
                    case QuestState.Inactive:
                        break;
                    case QuestState.Active:
                        newLog = Instantiate(questLogButtonPrefab, activeLogContent);
                        break;
                    case QuestState.Failed:
                    case QuestState.Passed:
                        newLog = Instantiate(questLogButtonPrefab, completedLogContent);
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
            ClearChildren(detailContent);
            detailsHeaderText.text = key;
            foreach (string logTxt in questLogs[key].LogTexts)
            {
                GameObject newTxt = Instantiate(questLogTextPrefab, detailContent);
                newTxt.transform.GetChild(0).GetComponent<Text>().text = logTxt;
            }
            detailsPage.SetActive(true);
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