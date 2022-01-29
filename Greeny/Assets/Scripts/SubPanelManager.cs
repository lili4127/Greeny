using UnityEngine;
using TMPro;

public class SubPanelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    private int panelActive = 0;
    private GameObject firstPanel;
    private GameObject secondPanel;

    private void Awake()
    {
        firstPanel = transform.GetChild(0).gameObject;
        secondPanel = transform.GetChild(1).gameObject;
    }

    private void OnEnable()
    {
        panelActive = 0;
        firstPanel.SetActive(true);
        secondPanel.SetActive(false);
        buttonText.text = "Next";
    }

    public void ChangeFrame()
    {
        if (panelActive == 0)
        {
            panelActive = 1;
            firstPanel.SetActive(false);
            secondPanel.SetActive(true);
            buttonText.text = "Back";
        }

        else
        {
            panelActive = 0;
            firstPanel.SetActive(true);
            secondPanel.SetActive(false);
            buttonText.text = "Next";
        }
    }
}
