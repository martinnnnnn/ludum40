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

    bool yesSelected;
    float currentCooldown;

    private void Start()
    {
        yesSelected = true;
        Yes.gameObject.SetActive(false);
        No.gameObject.SetActive(false);
        Background.gameObject.SetActive(false);
        Title.gameObject.SetActive(false);
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


    public void OnNoButton()
    {
        Application.Quit();
    }
}
