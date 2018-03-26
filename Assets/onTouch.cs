using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class onTouch : MonoBehaviour {

    static string output;
    static string tmpMouseStatus;
    static string tmpTrial;

    static float tmptime = 0;

    static System.IO.StreamWriter file = null;

    static float setupDistanceInit = 6.0f;
    static float setupSizeInit = 0.25f;
    static float setupHeightInit = 0.0f;
    static float setupDistanceDelta = 0f;
    static float setupSizeDelta = 0f;
    static int setupDistanceIterations = 0;
    static int setupSizeIterations = 0;
    static string dogName = "";
    static bool sendAlerts = true;

    static bool inTrainingMode = false;
    static bool showGreen = true;
    static bool showYellow = false;
    static bool showBlue = false;

    static List<Dictionary<string, float>> trialList;
    static int trialIndex;

    float button_press_time = 0.0f;

    static DateTime lastFileTime;

    public GameObject blueButton = null;
    public GameObject yellowButton = null;
    public GameObject greenButton = null;

    static string nextButton = "";

    static bool hitBlue = false;
    static bool hitYellow = false;
    static bool hitGreen = false;
    static float startTime = 0.0f;
    static float endTime = 0.0f;

    static Canvas configUI = null;
    static Canvas alertUI = null;
    static Canvas trainingUI = null;

    static string lastButtonPressed = null;

    static int ignoreTouch = -1;

    AudioSource beepAudio;

    // Use this for initialization
    void Start () {
        //beepAudio = GetComponent<AudioSource>();
        //if (beepAudio != null) beepAudio.clip.LoadAudioData();

        blueButton = GameObject.Find("blue");
        yellowButton = GameObject.Find("yellow");
        greenButton = GameObject.Find("green");

        nextButton = "blue";

        Cursor.visible = false;

        if (blueButton != null && yellowButton != null && greenButton != null)
        {
            setupDistanceInit = PlayerPrefs.GetFloat("setupDistanceInit", 6.0f);
            
            setupSizeInit = PlayerPrefs.GetFloat("setupSizeInit", 0.25f);
            GameObject.Find("blue").GetComponent<SphereCollider>().radius = setupSizeInit;
            setupHeightInit = PlayerPrefs.GetFloat("setupHeightInit", 0.0f);
            setupDistanceDelta = PlayerPrefs.GetFloat("setupDistanceDelta", 1.0f);
            setupSizeDelta = PlayerPrefs.GetFloat("setupSizeDelta", 0.25f);
            setupDistanceIterations = PlayerPrefs.GetInt("setupDistanceIterations", 2);
            setupSizeIterations = PlayerPrefs.GetInt("setupSizeIterations", 2);
            dogName = PlayerPrefs.GetString("dogName", "fido");
            sendAlerts = PlayerPrefs.GetInt("sendAlerts", 0) == 1 ? true : false;

            inTrainingMode = PlayerPrefs.GetInt("inTrainingMode", 0) == 1 ? true : false;
            showGreen = PlayerPrefs.GetInt("showGreen", 0) == 1 ? true : false;
            showYellow = PlayerPrefs.GetInt("showYellow", 0) == 1 ? true : false;
            showBlue = PlayerPrefs.GetInt("showBlue", 0) == 1 ? true : false;

            //updateSetup ();
        }

        if (configUI == null)
        {
            configUI = GameObject.Find("configCanvas").GetComponent<Canvas>();
            configUI.enabled = false;
        }

        if (alertUI == null)
        {
            alertUI = GameObject.Find("alertCanvas").GetComponent<Canvas>();
            alertUI.enabled = false;
        }
        if (trainingUI == null)
        {
            trainingUI = GameObject.Find("inTrainingModeCanvas").GetComponent<Canvas>();
            trainingUI.enabled = false;
        }

        updateSetup();

    }

    // Update is called once per frame
    void Update () {

        //if (configUI.enabled) return;

        RaycastHit hit = new RaycastHit();

        button_press_time = Time.time;

        Input.multiTouchEnabled = true;

        tmpTrial = trialIndex.ToString("0");

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            //if (ignoreTouch == -1) ignoreTouch = touch.fingerId;
            //if (touch.fingerId == ignoreTouch) continue;
            if (alertUI.enabled)
            {
                if (Time.time - tmptime > 2.0)
                {
                    alertUI.enabled = false;
                }
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out hit))
            {
                string hitName = hit.transform.gameObject.name;
                beepAudio = hit.transform.gameObject.GetComponent<AudioSource>();
                if (beepAudio != null) beepAudio.clip.LoadAudioData();
                if ((hitName != "miss") && (hitName == nextButton))
                {
                    if (beepAudio != null)
                    {
                        beepAudio.Play();
                    }

                    if (nextButton == "blue") nextButton = "yellow";
                    else if (nextButton == "yellow") nextButton = "green";
                    else
                    {
                        if (!inTrainingMode || showBlue)
                        {
                            nextButton = "blue";
                        } else
                        {
                            if (showYellow)
                            {
                                nextButton = "yellow";
                            }
                            else
                            {
                                nextButton = "green";
                            }
                        }
                        
                        nextTrial();

                        tmptime = Time.time;

                        if (sendAlerts && !inTrainingMode)
                        {
                            alertUI.enabled = true;

                            Thread mailThread = new Thread(new ThreadStart(mono_gmail.send));
                            mailThread.Start();
                        }
                    }
                }
                output = tmpTrial + ", " + touch.fingerId + ", " + touch.position[0] + ", " + touch.position[1] + ", " + hitName + ", " + touch.phase + ", " + button_press_time + ", " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
                file.WriteLine(output);
                //if ((hitName == "blue") && (touch.phase == TouchPhase.Began))
                //{
                //    hitBlue = true;
                //    startTime = button_press_time;
                //} else if ((hitName == "yellow") && hitBlue)
                //{
                //    hitYellow = true;
                //} else if ((hitName == "green") && (hitBlue) && !hitYellow)
                //{
                //    hitBlue = false;
                //} else if ((hitName == "green") && (hitYellow) && (touch.phase == TouchPhase.Ended))
                //{
                //    hitGreen = true;
                //    endTime = button_press_time;
                //}

                //if (hitBlue && hitYellow && hitGreen && (endTime - startTime < 60))
                //{
                //    hitBlue = false;
                //    hitYellow = false;
                //    hitGreen = false;
                //    if (sendAlerts)
                //    {
                //        alertUI.enabled = true;

                //        Thread mailThread = new Thread(new ThreadStart(mono_gmail.send));
                //        mailThread.Start();
                //    }
                //}
            }
            
                //debug.log (output);
            }

        //Debug.Log (System.DateTime.UtcNow.Ticks);
        if (Input.GetKeyUp(KeyCode.Slash))
        {
            if (!configUI.enabled)
            {
                enableConfigUI();
            }
            else
            {
                setupDistanceInit = float.Parse(GameObject.Find("distanceInput").GetComponent<InputField>().text);
                setupSizeInit = float.Parse(GameObject.Find("sizeInput").GetComponent<InputField>().text);
                setupHeightInit = float.Parse(GameObject.Find("heightInput").GetComponent<InputField>().text);
                setupDistanceDelta = float.Parse(GameObject.Find("distanceDelta").GetComponent<InputField>().text);
                setupSizeDelta = float.Parse(GameObject.Find("sizeDelta").GetComponent<InputField>().text);
                setupDistanceIterations = int.Parse(GameObject.Find("distanceIterations").GetComponent<InputField>().text);
                setupSizeIterations = int.Parse(GameObject.Find("sizeIterations").GetComponent<InputField>().text);
                dogName = GameObject.Find("dognameInput").GetComponent<InputField>().text;
                showGreen = GameObject.Find("greenOnOffInput").GetComponent<Toggle>().isOn;
                showYellow = GameObject.Find("yellowOnOffInput").GetComponent<Toggle>().isOn;
                showBlue = GameObject.Find("blueOnOffInput").GetComponent<Toggle>().isOn;

                if (inTrainingMode != GameObject.Find("trainingOnOffInput").GetComponent<Toggle>().isOn)
                {
                    inTrainingMode = GameObject.Find("trainingOnOffInput").GetComponent<Toggle>().isOn;
                    GameObject.Find("sendalertsInput").GetComponent<Toggle>().isOn = !inTrainingMode;
                }
                sendAlerts = GameObject.Find("sendalertsInput").GetComponent<Toggle>().isOn;

                PlayerPrefs.SetFloat("setupDistanceInit", setupDistanceInit);
                PlayerPrefs.SetFloat("setupSizeInit", setupSizeInit);
                PlayerPrefs.SetFloat("setupHeightInit", setupHeightInit);
                PlayerPrefs.SetFloat("setupDistanceDelta", setupDistanceDelta);
                PlayerPrefs.SetFloat("setupSizeDelta", setupSizeDelta);
                PlayerPrefs.SetFloat("setupDistanceIterations", setupDistanceIterations);
                PlayerPrefs.SetFloat("setupSizeIterations", setupSizeIterations);
                PlayerPrefs.SetString("dogName", dogName);
                PlayerPrefs.SetInt("sendAlerts", sendAlerts ? 1 : 0);
                PlayerPrefs.SetInt("inTrainingMode", inTrainingMode ? 1 : 0);
                PlayerPrefs.SetInt("showGreen", showGreen ? 1 : 0);
                PlayerPrefs.SetInt("showYellow", showYellow ? 1 : 0);
                PlayerPrefs.SetInt("showBlue", showBlue ? 1 : 0);

                updateSetup();
                configUI.enabled = false;
            }
        }


        file.Flush();
    }

    void updateSetup()
    {
        PlayerPrefs.SetFloat("setupDistanceInit", setupDistanceInit);
        PlayerPrefs.SetFloat("setupSizeInit", setupSizeInit);
        PlayerPrefs.SetFloat("setupHeightInit", setupHeightInit);
        PlayerPrefs.SetFloat("setupDistanceDelta", setupDistanceDelta);
        PlayerPrefs.SetFloat("setupSizeDelta", setupSizeDelta);
        PlayerPrefs.SetFloat("setupDistanceIterations", setupDistanceIterations);
        PlayerPrefs.SetFloat("setupSizeIterations", setupSizeIterations);
        PlayerPrefs.SetString("dogName", dogName);
        PlayerPrefs.SetInt("sendAlerts", sendAlerts ? 1 : 0);
        PlayerPrefs.SetInt("inTrainingMode", inTrainingMode ? 1 : 0);
        PlayerPrefs.SetInt("showGreen", showGreen ? 1 : 0);
        PlayerPrefs.SetInt("showYellow", showYellow ? 1 : 0);
        PlayerPrefs.SetInt("showBlue", showBlue ? 1 : 0);

        blueButton.GetComponent<SphereCollider>().radius = 5;
        yellowButton.GetComponent<SphereCollider>().radius = 5;
        greenButton.GetComponent<SphereCollider>().radius = 5;

        if (inTrainingMode)
        {
            trainingUI.enabled = true;
            yellowButton.SetActive(showYellow);
            blueButton.SetActive(showBlue);
            if (showBlue)
            {
                nextButton = "blue";
            } else if (showYellow)
            {
                nextButton = "yellow";
            } else
            {
                nextButton = "green";
            }
        } else
        {
            trainingUI.enabled = false;
            yellowButton.SetActive(true);
            blueButton.SetActive(true);
            nextButton = "blue";
        }

        float height, distance, size;

        trialList = new List<Dictionary<string, float>>();

        height = setupHeightInit;
        distance = setupDistanceInit;
        for (int j = 0; j < setupDistanceIterations; j++)
        {
            size = setupSizeInit;
            for (int k = 0; k < setupSizeIterations; k++)
            {
                Dictionary<string, float> trial = new Dictionary<string, float>();
                trial.Add("distance", distance);
                trial.Add("size", size);
                trial.Add("height", height);
                trialList.Add(trial);

                size += setupSizeDelta;
            }
            distance += setupDistanceDelta;
        }

        trialIndex = -1;
        nextTrial();

        if (file != null) file.Close();

        string basename = "C:/Users/allison/Desktop/dogdata/";
        string filename = basename + dogName + ".csv";
        int count = 1;
        while (System.IO.File.Exists(filename))
        {
            count++;
            filename = basename + dogName + count + ".csv";
        }

        file = new System.IO.StreamWriter(filename);
        file.WriteLine("Distance, " + setupDistanceInit + ", " + setupDistanceDelta + ", " + setupDistanceIterations);
        file.WriteLine("Size, " + setupSizeInit + ", " + setupSizeDelta + ", " + setupSizeIterations);
        file.WriteLine("Height, " + +setupHeightInit);
        file.WriteLine("trialNum, touchID, mouseX, mouseY, hitCircle, mouseState, eventTime, worldTime");
    }

    void nextTrial()
    {
        trialIndex++;
        //trialIndex = 0;
        if (trialIndex >= trialList.Count)
        {
            enableConfigUI();
            return;
        }

        float size = trialList[trialIndex]["size"];
        float distance = trialList[trialIndex]["distance"];
        float height = trialList[trialIndex]["height"];

        blueButton.transform.localScale = new Vector2(size, size);
        yellowButton.transform.localScale = new Vector2(size, size);
        greenButton.transform.localScale = new Vector2(size, size);

        float xPos = blueButton.GetComponent<SphereCollider>().bounds.extents.x + (distance / 2.0f);
        blueButton.transform.position = new Vector2(-2 * xPos, -height);
        yellowButton.transform.position = new Vector2(0, -height);
        greenButton.transform.position = new Vector2(2 * xPos, -height);
    }

    void checkFile()
    {
        DateTime when = lastFileTime;
        TimeSpan ts = DateTime.Now.Subtract(when);
        if (ts.TotalHours > 6)
        {
            if (file != null) file.Close();
            lastFileTime = DateTime.Now;
            updateSetup();
        }
    }

    void enableConfigUI()
    {
        GameObject.Find("distanceInput").GetComponent<InputField>().text = setupDistanceInit.ToString();
        GameObject.Find("sizeInput").GetComponent<InputField>().text = setupSizeInit.ToString();
        GameObject.Find("heightInput").GetComponent<InputField>().text = setupHeightInit.ToString();
        GameObject.Find("distanceDelta").GetComponent<InputField>().text = setupDistanceDelta.ToString();
        GameObject.Find("sizeDelta").GetComponent<InputField>().text = setupSizeDelta.ToString();
        GameObject.Find("distanceIterations").GetComponent<InputField>().text = setupDistanceIterations.ToString();
        GameObject.Find("sizeIterations").GetComponent<InputField>().text = setupSizeIterations.ToString();
        GameObject.Find("dognameInput").GetComponent<InputField>().text = dogName;
        GameObject.Find("sendalertsInput").GetComponent<Toggle>().isOn = sendAlerts;
        GameObject.Find("trainingOnOffInput").GetComponent<Toggle>().isOn = inTrainingMode;

        configUI.enabled = true;
    }

    void OnApplicationQuit()
    {
        if (file != null) file.Close();
    }

}
