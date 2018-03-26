//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading;
//using System;


//[RequireComponent(typeof(AudioSource))]
//public class Buttons : MonoBehaviour
//{
//	static Buttons nextButton;

//	static Buttons blueButton = null;
//	static Buttons yellowButton = null;
//	static Buttons greenButton = null;
	
//	static Vector3 mousePos;
//	static string output;
//	static string tmpMouseStatus;
//	static string tmpTrial;

//	static float tmptime = 0;

//	static System.IO.StreamWriter file = null;
//    //file.WriteLine(lines);
//    //file.Close();

//    static DateTime lastFileTime;

//	AudioSource beepAudio;
//	float button_press_time = 0.0f; // button_press_time variable to store our time

//	static float setupDistanceInit = 6.0f;
//	static float setupSizeInit = 0.25f;
//	static float setupHeightInit = 0.0f;
//	static float setupDistanceDelta = 0f;
//	static float setupSizeDelta = 0f;
//	static int setupDistanceIterations = 0;
//	static int setupSizeIterations = 0;
//	static string dogName = "";
//	static bool sendAlerts = true;

//	static List<Dictionary<string, float>> trialList;
//	static int trialIndex;

//	static Canvas configUI = null;
//	static Canvas alertUI = null;

//	// Use this for initialization
//	void Start () {
//		Cursor.visible = false;
//		beepAudio = GetComponent<AudioSource>();
//		if(beepAudio != null) beepAudio.clip.LoadAudioData ();

//		if (blueButton == null && gameObject.name.Equals ("blue")) {
//			blueButton = this;
//			nextButton = blueButton;
//		}

//		if (yellowButton == null && gameObject.name.Equals ("yellow")) {
//			yellowButton = this;
//		}

//		if (greenButton == null && gameObject.name.Equals ("green")) {
//			greenButton = this;
//		}

//		if (blueButton != null && yellowButton != null && greenButton != null)
//		{
//			setupDistanceInit = PlayerPrefs.GetFloat("setupDistanceInit", 6.0f);
//			setupSizeInit = PlayerPrefs.GetFloat("setupSizeInit", 0.25f);
//			setupHeightInit = PlayerPrefs.GetFloat("setupHeightInit", 0.0f);
//			setupDistanceDelta = PlayerPrefs.GetFloat("setupDistanceDelta", 1.0f);
//			setupSizeDelta = PlayerPrefs.GetFloat("setupSizeDelta", 0.25f);
//			setupDistanceIterations = PlayerPrefs.GetInt("setupDistanceIterations", 2);
//			setupSizeIterations = PlayerPrefs.GetInt("setupSizeIterations", 2);
//			dogName = PlayerPrefs.GetString("dogName", "fido");
//			sendAlerts = PlayerPrefs.GetInt("sendAlerts", 0) == 1 ? true : false;

//			//updateSetup ();
//		}

//		//if (configUI == null) {
//		//	configUI = GameObject.Find ("configCanvas").GetComponent<Canvas>();
//		//	configUI.enabled = false;
//		//}

//		//if (alertUI == null) {
//		//	alertUI = GameObject.Find ("alertCanvas").GetComponent<Canvas>();
//		//	alertUI.enabled = false;
//		//}
 
        
//	}

//    //void checkFile()
//    //{
//    //    DateTime when = lastFileTime;
//    //    TimeSpan ts = DateTime.Now.Subtract(when);
//    //    if(ts.TotalHours > 6)
//    //    {
//    //        if (file != null) file.Close();
//    //        lastFileTime = DateTime.Now;
//    //        updateSetup();
//    //    }
//    //}


//	// Update is called once per frame
//	void Update ()
//	{
//        PlayerPrefs.SetFloat("setupDistanceInit", setupDistanceInit);
//        PlayerPrefs.SetFloat("setupSizeInit", setupSizeInit);
//        PlayerPrefs.SetFloat("setupHeightInit", setupHeightInit);
//        PlayerPrefs.SetFloat("setupDistanceDelta", setupDistanceDelta);
//        PlayerPrefs.SetFloat("setupSizeDelta", setupSizeDelta);
//        PlayerPrefs.SetFloat("setupDistanceIterations", setupDistanceIterations);
//        PlayerPrefs.SetFloat("setupSizeIterations", setupSizeIterations);
//        PlayerPrefs.SetString("dogName", dogName);
//        PlayerPrefs.SetInt("sendAlerts", sendAlerts ? 1 : 0);
//        Input.multiTouchEnabled = true;
//        bool isEnabled = Input.multiTouchEnabled;
//        //if (Input.touchCount > 0)
//        //{
//        //    Touch touch = Input.GetTouch(0);
//        //    file.WriteLine(touch.position);

//        //}
//        //file.Flush();

//        //if (configUI.enabled) return; //don't do anything when config is displayed

//        button_press_time = Time.time; //Set button_press_time  to current time
//        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//        //bool buttonUp = Input.GetMouseButtonUp(0);
//        //bool buttonDown = Input.GetMouseButtonDown(0);
//        //bool buttonHeld = Input.GetMouseButton(0);

//        //if (buttonUp && alertUI.enabled && (Time.time - tmptime > 2.0))
//        //{
//        //    alertUI.enabled = false;
//        //    return;
//        //}

//        //tmpTrial = trialIndex.ToString("0");

//        //if (buttonUp || buttonHeld)
//        //{
//        //    if (nextButton == this)
//        //    {
//        //        if (beepAudio != null) beepAudio.Play();

//        //        if (nextButton == blueButton) nextButton = yellowButton;
//        //        else if (nextButton == yellowButton) nextButton = greenButton;
//        //        else
//        //        {
//        //            nextButton = blueButton;
//        //            nextTrial();

//        //            tmptime = Time.time;

//        //            if (sendAlerts)
//        //            {
//        //                alertUI.enabled = true;

//        //                Thread mailThread = new Thread(new ThreadStart(mono_gmail.send));
//        //                mailThread.Start();
//        //            }
//        //        }
//        //    }

//        //    tmpMouseStatus = "up";
//        //}
//        //if (buttonDown) tmpMouseStatus = "down";
//        //else if (Input.GetMouseButton(0)) tmpMouseStatus = "held";

//        bool enabled = Input.multiTouchEnabled;

        

//  //      if (this == blueButton)
//		//{
//		//	//Debug.Log (System.DateTime.UtcNow.Ticks);
//		//	if (Input.GetKeyUp (KeyCode.Slash))
//		//	{
//		//		if(!configUI.enabled)
//		//		{
//		//			enableConfigUI();
//		//		}
//		//		else
//		//		{
//		//			setupDistanceInit = float.Parse(GameObject.Find("distanceInput").GetComponent<InputField>().text);
//		//			setupSizeInit = float.Parse(GameObject.Find("sizeInput").GetComponent<InputField>().text);
//		//			setupHeightInit = float.Parse(GameObject.Find("heightInput").GetComponent<InputField>().text);
//		//			setupDistanceDelta = float.Parse(GameObject.Find("distanceDelta").GetComponent<InputField>().text);
//		//			setupSizeDelta = float.Parse(GameObject.Find("sizeDelta").GetComponent<InputField>().text);
//		//			setupDistanceIterations = int.Parse(GameObject.Find("distanceIterations").GetComponent<InputField>().text);
//		//			setupSizeIterations = int.Parse(GameObject.Find("sizeIterations").GetComponent<InputField>().text);
//		//			dogName = GameObject.Find("dognameInput").GetComponent<InputField>().text;
//		//			sendAlerts = GameObject.Find("sendalertsInput").GetComponent<Toggle>().isOn;

//		//			PlayerPrefs.SetFloat("setupDistanceInit", setupDistanceInit);
//		//			PlayerPrefs.SetFloat("setupSizeInit", setupSizeInit);
//		//			PlayerPrefs.SetFloat("setupHeightInit", setupHeightInit);
//		//			PlayerPrefs.SetFloat("setupDistanceDelta", setupDistanceDelta);
//		//			PlayerPrefs.SetFloat("setupSizeDelta", setupSizeDelta);
//		//			PlayerPrefs.SetFloat("setupDistanceIterations", setupDistanceIterations);
//		//			PlayerPrefs.SetFloat("setupSizeIterations", setupSizeIterations);
//		//			PlayerPrefs.SetString("dogName", dogName);
//		//			PlayerPrefs.SetInt("sendAlerts", sendAlerts ? 1 : 0);

//		//			//updateSetup();
//		//			configUI.enabled = false;
//		//		}
//		//	}
//		//}
//        //checkFile();
//	}

//	//void enableConfigUI()
//	//{
//	//	GameObject.Find ("distanceInput").GetComponent<InputField> ().text = setupDistanceInit.ToString ();
//	//	GameObject.Find ("sizeInput").GetComponent<InputField> ().text = setupSizeInit.ToString ();
//	//	GameObject.Find ("heightInput").GetComponent<InputField> ().text = setupHeightInit.ToString ();
//	//	GameObject.Find ("distanceDelta").GetComponent<InputField> ().text = setupDistanceDelta.ToString ();
//	//	GameObject.Find ("sizeDelta").GetComponent<InputField> ().text = setupSizeDelta.ToString ();
//	//	GameObject.Find ("distanceIterations").GetComponent<InputField> ().text = setupDistanceIterations.ToString ();
//	//	GameObject.Find ("sizeIterations").GetComponent<InputField> ().text = setupSizeIterations.ToString ();
//	//	GameObject.Find ("dognameInput").GetComponent<InputField> ().text = dogName;
//	//	GameObject.Find ("sendalertsInput").GetComponent<Toggle> ().isOn = sendAlerts;
		
//	//	configUI.enabled = true;
//	//}

//	//void OnApplicationQuit()
//	//{
//	//	if (file != null) file.Close ();
//	//}

//    //void OnMouseOver() {
//    //	if (configUI.enabled) return; //don't do anything when config is displayed

//    //	button_press_time = Time.time; //Set button_press_time  to current time
//    //	mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//    //	bool buttonUp = Input.GetMouseButtonUp(0);
//    //	bool buttonDown = Input.GetMouseButtonDown(0);
//    //	bool buttonHeld = Input.GetMouseButton(0);

//    //	if (buttonUp && alertUI.enabled && (Time.time - tmptime > 2.0))
//    //	{
//    //		alertUI.enabled = false;
//    //		return;
//    //	}

//    //	tmpTrial = trialIndex.ToString("0");

//    //	if (buttonUp || buttonHeld) {
//    //		if(nextButton == this)
//    //		{
//    //			if(beepAudio != null) beepAudio.Play();

//    //			if(nextButton == blueButton) nextButton = yellowButton;
//    //			else if(nextButton == yellowButton) nextButton = greenButton;
//    //			else
//    //			{
//    //				nextButton = blueButton;
//    //				nextTrial();

//    //				tmptime = Time.time;

//    //				if(sendAlerts)
//    //				{
//    //					alertUI.enabled = true;

//    //					Thread mailThread = new Thread(new ThreadStart(mono_gmail.send));
//    //					mailThread.Start();
//    //				}
//    //			}
//    //		}

//    //		tmpMouseStatus = "up";
//    //	}
//    //	if (buttonDown) tmpMouseStatus = "down";
//    //	else if (Input.GetMouseButton(0)) tmpMouseStatus = "held";

//    //       bool enabled = Input.multiTouchEnabled;

//    //       if (buttonUp || buttonDown || buttonHeld)
//    //       {
//    //           output = tmpTrial + ", " + mousePos.x + ", " + mousePos.y + ", " + gameObject.name + ", " + tmpMouseStatus + ", " + button_press_time;
//    //           file.WriteLine(output);
//    //           //debug.log (output);
//    //       }



//    //       if (buttonUp) file.Flush();
//    //}

//    //void updateSetup()
//    //{
//    //    float height, distance, size;

//    //    trialList = new List<Dictionary<string, float>>();

//    //    height = setupHeightInit;
//    //    distance = setupDistanceInit;
//    //    for (int j = 0; j < setupDistanceIterations; j++)
//    //    {
//    //        size = setupSizeInit;
//    //        for (int k = 0; k < setupSizeIterations; k++)
//    //        {
//    //            Dictionary<string, float> trial = new Dictionary<string, float>();
//    //            trial.Add("distance", distance);
//    //            trial.Add("size", size);
//    //            trial.Add("height", height);
//    //            trialList.Add(trial);

//    //            size += setupSizeDelta;
//    //        }
//    //        distance += setupDistanceDelta;
//    //    }

//    //    trialIndex = -1;
//    //    nextTrial();

//    //    if (file != null) file.Close();

//    //    string basename = "/Users/fido/Desktop/dogdata/";
//    //    string filename = basename + dogName + ".csv";
//    //    int count = 1;
//    //    while (System.IO.File.Exists(filename))
//    //    {
//    //        count++;
//    //        filename = basename + dogName + count + ".csv";
//    //    }

//    //    file = new System.IO.StreamWriter(filename);
//    //    file.WriteLine("Distance, " + setupDistanceInit + ", " + setupDistanceDelta + ", " + setupDistanceIterations);
//    //    file.WriteLine("Size, " + setupSizeInit + ", " + setupSizeDelta + ", " + setupSizeIterations);
//    //    file.WriteLine("Height, " + +setupHeightInit);
//    //    file.WriteLine("trialNum, mouseX, mouseY, hitCircle, mouseState, eventTime, worldTime");
//    //}

//    //void nextTrial()
//    //{
//    //    //trialIndex++;
//    //    trialIndex = 0;
//    //    if (trialIndex >= trialList.Count)
//    //    {
//    //        enableConfigUI();
//    //        return;
//    //    }

//    //    float size = trialList[trialIndex]["size"];
//    //    float distance = trialList[trialIndex]["distance"];
//    //    float height = trialList[trialIndex]["height"];

//    //    blueButton.transform.localScale = new Vector2(size, size);
//    //    yellowButton.transform.localScale = new Vector2(size, size);
//    //    greenButton.transform.localScale = new Vector2(size, size);

//    //    float xPos = blueButton.GetComponent<SphereCollider>().bounds.extents.x + (distance / 2.0f);
//    //    blueButton.transform.position = new Vector2(-2 * xPos, -height);
//    //    yellowButton.transform.position = new Vector2(0, -height);
//    //    greenButton.transform.position = new Vector2(2 * xPos, -height);
//    //}


//}
