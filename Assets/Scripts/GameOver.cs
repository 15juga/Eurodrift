using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject Again;
    public GameObject Quit;
    public Text GameIsDone;
    public Text timeText;
    public Text LapsText;

    private int done;
    private float Minutes;
    private float Seconds;
    private float Laps;
    private GameObject url;
    private float sound;

    // Start is called before the first frame update
    void Start()
    {

        done = PlayerPrefs.GetInt("Done");
        Laps = PlayerPrefs.GetInt("Laps");
        Minutes = PlayerPrefs.GetFloat("MinCleared");
        Seconds = PlayerPrefs.GetFloat("SecondsCleared");
        url = GameObject.Find("Link");

        if (done == 0)
        {
            GameIsDone.text = ("Cleared!");
        }
        else if (done != 0)
        {
            GameIsDone.text = ("Game Over!");
        }

        if (Minutes == 0)
        {
            timeText.text = ("Time: " + Seconds.ToString("0.00") + " sec");
        }
        else
        {
            timeText.text = "Time: " + Minutes.ToString("0") + "m " + Seconds.ToString("0.00") + " s";
        }

        LapsText.text = ("Laps: " + Laps);

        Cursor.visible = true;

        GameObject Cam = GameObject.Find("Main Camera");
        sound = PlayerPrefs.GetFloat("Volume");
        Cam.GetComponent<AudioSource>().volume = sound;
    }

    void Update()
    {
        Again.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);
        Quit.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);
        url.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void openURL()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdyhXZqusfVmUYm22iZjbdv9PHovN1hsh9Oa33nJ97_7yYUCw/viewform");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
