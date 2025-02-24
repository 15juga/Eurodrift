using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject playButton;
    public GameObject quitButton;

    public GameObject Car1;
    public GameObject Car2;
    public GameObject Back;
    public GameObject gameStart;

    public GameObject Dorifto;
    public GameObject Fafnir;

    public GameObject logo;

    private Vector3 Loc;
    private Vector3 startButtonPos;
    private Vector3 carPos;



    void Start()
    {
        //begin = new Vector3(760, -200, 0);
        Loc = new Vector3(-300, 0, 0);
        carPos = Fafnir.transform.position;

        Dorifto.transform.position = Loc;

        startButtonPos = new Vector3(gameStart.transform.position.x, gameStart.transform.position.y, gameStart.transform.position.z);
        gameStart.transform.position = Loc;
        PlayerPrefs.SetInt("carModel", 0);
    }

    private void Update() {
        logo.GetComponent<Image>().color = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.3f, 1), 0.7f, 1f);
    }

    // Start is called before the first frame update
    public void selectCar()
    {
        Car1.transform.position = playButton.transform.position;// + new Vector3(playButton.transform.position.x + );
        Car2.transform.position = quitButton.transform.position;
        Back.transform.position = new Vector3(quitButton.transform.position.x, quitButton.transform.position.y - 100, 0);

        playButton.transform.position = Loc;
        quitButton.transform.position = Loc;

        gameStart.transform.position = startButtonPos;
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
        SceneManager.LoadScene(1);
    }

    public void Return()
    {
        playButton.transform.position = Car1.transform.position;
        quitButton.transform.position = Car2.transform.position;

        Car1.transform.position = Loc;
        Car2.transform.position = Loc;
        Back.transform.position = Loc;
        gameStart.transform.position = Loc;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
