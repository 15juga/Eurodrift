using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject playButton;
    public GameObject conButton;
    public GameObject quitButton;
    public GameObject logo;

    public GameObject Car1;
    public GameObject Car2;
    public GameObject Back;
    public GameObject gameStart;

    public GameObject Dorifto;
    public GameObject Fafnir;

    public Light lamp;
    public Material mat;

    private Vector3 Loc;
    private Vector3 carPos;

    private GameObject Keys;
    private GameObject Mechanics;

    private GameObject keyBoard;
    private GameObject Flip;
    private GameObject Spin;
    private GameObject DriftMode;
    private GameObject url;
    private float sound;

    void Start()
    {
        Keys = GameObject.Find("KeyButton");
        Mechanics = GameObject.Find("Mechanics");
        url = GameObject.Find("Link");
        sound = 0.01f;

        keyBoard = GameObject.Find("Keyboard");
        Flip = GameObject.Find("Flip");
        Spin = GameObject.Find("Spin");
        DriftMode = GameObject.Find("DriftMode");

        Loc = new Vector3(-300, 0, 0);
        carPos = Fafnir.transform.position;

        Dorifto.transform.position = Loc;
        PlayerPrefs.SetInt("carModel", 0);

        Car1.SetActive(false);
        Car2.SetActive(false);
        Back.SetActive(false);
        gameStart.SetActive(false);

        mat.EnableKeyword("_EMISSION");

        keyBoard.SetActive(false);
        Flip.SetActive(false);
        Spin.SetActive(false);
        DriftMode.SetActive(false);
        Keys.SetActive(false);
        Mechanics.SetActive(false);
    }

    private void Update() {
        logo.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);
        playButton.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);
        conButton.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);
        quitButton.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);

        Car1.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);
        Car2.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);
        gameStart.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);
        Back.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);
        Keys.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);
        Mechanics.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);
        url.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.1f, 1), 0.7f, 0.9f);


        if (PlayerPrefs.GetInt("carModel") == 0) {
            Color col = Fafnir.transform.GetChild(0).GetComponent<Renderer>().materials[1].GetColor("_EmissionColor");
            mat.SetColor("_EmissionColor", Color.Lerp(mat.GetColor("_EmissionColor"), col, 0.05f));
            lamp.color = Color.Lerp(lamp.color, col, 0.05f);

            Fafnir.transform.position = new Vector3(carPos.x, carPos.y + Mathf.Sin(Time.time * 0.7f) * 0.03f, carPos.z);
        }

        if(PlayerPrefs.GetInt("carModel") == 1) {
            Color col = Dorifto.transform.GetComponent<Renderer>().materials[0].GetColor("_EmissionColor");
            mat.SetColor("_EmissionColor", Color.Lerp(mat.GetColor("_EmissionColor"), col, 0.05f));
            lamp.color = Color.Lerp(lamp.color, col, 0.05f);

            Dorifto.transform.position = new Vector3(carPos.x, carPos.y + Mathf.Sin(Time.time * 0.7f) * 0.03f, carPos.z);
        }
    }

    // Start is called before the first frame update
    public void selectCar()
    {
        Car1.SetActive(true);
        Car2.SetActive(true);
        Back.SetActive(true);
        Back.SetActive(true);
        gameStart.SetActive(true);

        playButton.SetActive(false);
        conButton.SetActive(false);
        quitButton.SetActive(false);
        url.SetActive(false);
        
    }

    public void Car01()
    {
        Dorifto.transform.position = Loc;
        Fafnir.transform.position = carPos;
        PlayerPrefs.SetInt("carModel", 0);
    }

    public void Car02()
    {
        Dorifto.transform.position = carPos;

        Dorifto.transform.rotation = Fafnir.transform.rotation;
        Fafnir.transform.position = Loc;
        PlayerPrefs.SetInt("carModel", 1);
    }

    public void StartGame()
    {
        PlayerPrefs.SetFloat("Volume", sound);
        SceneManager.LoadScene(1);
    }

    public void Return()
    {
        Car1.SetActive(false);
        Car2.SetActive(false);
        Back.SetActive(false);
        gameStart.SetActive(false);
        keyBoard.SetActive(false);
        Keys.SetActive(false);
        Mechanics.SetActive(false);
        DriftMode.SetActive(false);
        Spin.SetActive(false);
        Flip.SetActive(false);
        keyBoard.SetActive(false);

        playButton.SetActive(true);
        conButton.SetActive(true);
        quitButton.SetActive(true);
        logo.SetActive(true);
        url.SetActive(true);
    }

    public void Controls()
    {
        playButton.SetActive(false);
        quitButton.SetActive(false);
        conButton.SetActive(false);
        logo.SetActive(false);

        Back.SetActive(true);
        keyBoard.SetActive(true);
        Keys.SetActive(true);
        Mechanics.SetActive(true);
        url.SetActive(false);
    }

    public void KeyB()
    {
        Flip.SetActive(false);
        DriftMode.SetActive(false);
        Spin.SetActive(false);
        keyBoard.SetActive(true);
    }

    public void Mechas()
    {
        keyBoard.SetActive(false);

        Flip.SetActive(true);
        Spin.SetActive(true);
        DriftMode.SetActive(true);
        Back.SetActive(true);
    }

    public void openURL()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdyhXZqusfVmUYm22iZjbdv9PHovN1hsh9Oa33nJ97_7yYUCw/viewform");
    }

    public void setVolume()
    {
        GameObject slider = GameObject.Find("Slider");
        GameObject cam = GameObject.Find("Main Camera");
        sound = slider.GetComponent<Slider>().value;
        cam.GetComponent<AudioSource>().volume = sound;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
