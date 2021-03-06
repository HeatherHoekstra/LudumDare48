using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public GameObject alert;
    public Player player;
    public GameObject endGameMenu;

    [SerializeField] TextMeshProUGUI speedNr;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI strengthNr;
    [SerializeField] TextMeshProUGUI result;

    // Update is called once per frame
    void Update()
    {
        float speed = Mathf.Round(player.fallSpeed *10);
        speedNr.text = speed.ToString();

        strengthNr.text = player.strength.ToString();

        if (player.dangerZone && speedNr.color != Color.red)
        {
            speedNr.color = Color.red;
            speedText.color = Color.red;
        }
        else if(!player.dangerZone && speedNr.color !=Color.green)
        {
            speedNr.color = Color.green;
            speedText.color = Color.green;
        }
    }

    public void InstanciateAlert(GameObject collision)
    {
        GameObject newAlert = Instantiate(alert);
        newAlert.transform.SetParent(gameObject.transform, false);
        Vector2 alertPos = new Vector2(collision.gameObject.transform.position.x, collision.transform.position.y + 12.5f);
        RectTransform alertRect = newAlert.GetComponent<RectTransform>();
        alertRect.transform.position = Camera.main.WorldToScreenPoint(alertPos);
        
        if(collision.transform.parent.tag ==  "Weight" ^ collision.transform.parent.tag == "WeightBig")
        {
            newAlert.GetComponent<Image>().color = Color.red;
        }
    }

    public void EndGameMenu(bool alive)
    {
        if (alive)
        {
            result.text = "You survived the fall!";
        }
        else
        {
            result.text = "You are dead";
        }
        
        endGameMenu.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        endGameMenu.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
