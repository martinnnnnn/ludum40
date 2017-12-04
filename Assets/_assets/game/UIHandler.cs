using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{

    public Button Yes;
    public Button No;
    public Image Background;
    public Text Title;

    public Color DefaultColor;
    public Color SelectedColor;
    public float ButtonCooldown;

    public Image Shield1;
    public Image Shield2;
    public Image Shield3;
    public Image Shield4;

    public Text GoldValue;

    bool yesSelected;
    float currentCooldown;
    Hero _hero;

    private void Start()
    {
        yesSelected = true;
        Yes.gameObject.SetActive(false);
        No.gameObject.SetActive(false);
        Background.gameObject.SetActive(false);
        Title.gameObject.SetActive(false);

        Shield1.gameObject.SetActive(false);
        Shield2.gameObject.SetActive(false);
        Shield3.gameObject.SetActive(false);
        Shield4.gameObject.SetActive(false);

        _hero = FindObjectOfType<Hero>();
        _hero.OnLifeChange += SetArmors;
        _hero.OnLootChange += UpdateGoalValue;
    }

    private void Update()
    {
        if (!Background.gameObject.activeSelf)
            return;

        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.8f && currentCooldown <= 0)
        {
            currentCooldown = ButtonCooldown;
            if (yesSelected)
            {
                yesSelected = false;
                No.transform.GetChild(0).GetComponent<Text>().color = SelectedColor;
                Yes.transform.GetChild(0).GetComponent<Text>().color = DefaultColor;
            }
            else
            {
                yesSelected = true;
                Yes.transform.GetChild(0).GetComponent<Text>().color = SelectedColor;
                No.transform.GetChild(0).GetComponent<Text>().color = DefaultColor;
            }
        }

        if (Input.GetButtonDown("ThrowArmor"))
        {
            if (yesSelected)
            {
                OnYesButton();
            }
            else
            {
                OnNoButton();
            }
        }
        
    }


    public void ShowUI()
    {
        Yes.gameObject.SetActive(true);
        No.gameObject.SetActive(true);
        Background.gameObject.SetActive(true);
        Title.gameObject.SetActive(true);

        Yes.transform.GetChild(0).GetComponent<Text>().color = SelectedColor;
        No.transform.GetChild(0).GetComponent<Text>().color = DefaultColor;
    }



    public void OnYesButton()
    {
        Yes.gameObject.SetActive(false);
        No.gameObject.SetActive(false);
        Background.gameObject.SetActive(false);
        Title.gameObject.SetActive(false);
        FindObjectOfType<LevelHandler>().Reset();
    }


    public void SetArmors()
    {
        int life = FindObjectOfType<Hero>().Life;
        Shield1.gameObject.SetActive(false);
        Shield2.gameObject.SetActive(false);
        Shield3.gameObject.SetActive(false);
        Shield4.gameObject.SetActive(false);
        if (life > 0)
        {
            Shield1.gameObject.SetActive(true);
        }
        if (life > 1)
        {
            Shield2.gameObject.SetActive(true);
        }
        if (life > 2)
        {
            Shield3.gameObject.SetActive(true);
        }
        if (life > 3)
        {
            Shield4.gameObject.SetActive(true);
        }
    }


    public void OnNoButton()
    {
        Application.Quit();
    }

    public void UpdateGoalValue()
    {
        GoldValue.text = _hero.Gold.ToString();
    }
}
