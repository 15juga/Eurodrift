﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{

    public Text timeText;
    public Text timeLeft;
    public Text boostPercent;
    public string barName;
    public GameObject player;
    public GameObject PauseUI;
    public float countDownS;

    private float timeSeconds;
    private float timeMinutes;
    private Image bar;
    private float boostValue;
    private float boost;
    private int life;
    private int laps;
    private bool GamePaused;
    private Transform[] canvas;

    // Start is called before the first frame update
    void Start()
    {
        timeSeconds = 0;
        timeMinutes = 0;
        setTimeText();

        StartCoroutine(setCar());
        PauseUI.SetActive(false);
        GamePaused = false;
        Cursor.visible = false;
}

IEnumerator setCar() {
        yield return new WaitForSeconds(0.1f);

        player.transform.rotation = Quaternion.Euler(0, 180, 0);
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void Awake()
    {
        bar = transform.Find(barName).GetComponent<Image>();
        bar.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Restart();
        }

        //Pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        

        //timer
        timeSeconds += Time.deltaTime;
        if(timeSeconds > 60)
        {
            timeSeconds = 0;
            timeMinutes++;
        }
        setTimeText();

        //CountDown
        countDownS -= Time.deltaTime; 
        countDown();
        if (countDownS <= 20)
        {

        }

        //Boost bar
        boostValue = player.GetComponent<VehicleControler>().boostMtr/4;
        boost = Mathf.Clamp(boostValue * 100, 0, 100);
        boostPercent.text = (boost.ToString("0.0") + "%");
        bar.fillAmount = boostValue;


        //Sending info if game is done
        GameObject canvas = GameObject.Find("Player");
        GameObject Portals = GameObject.FindGameObjectWithTag("EndPortal");
        laps = Portals.GetComponent<Portal>().lapsAmount;
        life = canvas.GetComponent<HP>().lives;

        if (countDownS <= 20)
            player.GetComponent<VehicleControler>().OutOfTime();
        else
            player.GetComponent<VehicleControler>().notOutOfTime();

        if (countDownS <= 0 || life <= 0)
        {
            PlayerPrefs.SetInt("Done", 1);
            EndGame();
        }
        else if (laps >= 3)
        {
            PlayerPrefs.SetInt("Done", 0);
            EndGame();
        }
    }

    void setTimeText()
    {
        if (timeMinutes == 0)
        {
            timeText.text = ("Time: " + timeSeconds.ToString("0.00") + " s");
        }
        else
        {
            timeText.text = "Time: " + timeMinutes.ToString("0") + " m " + timeSeconds.ToString("0.00") + " s";
        }
    }

    void countDown()
    {
        timeLeft.text = ("Time Left: " + countDownS.ToString("0.0") + " s");
    }

    public void Resume()
    {
        Cursor.visible = false;
        PauseUI.SetActive(false);
        GamePaused = false;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        Cursor.visible = true;
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void EndGame()
    {
        PlayerPrefs.SetFloat("MinCleared", timeMinutes);
        PlayerPrefs.SetFloat("SecondsCleared", timeSeconds);
        PlayerPrefs.SetInt("Laps", laps);
        SceneManager.LoadScene(2);

        //canvas = gameObject.GetComponentsInChildren<Transform>();
        //foreach (Transform i in canvas)
        //{
        //    i.gameObject.SetActive(false);
        //}
        //transform.gameObject.SetActive(true);
    }
}
