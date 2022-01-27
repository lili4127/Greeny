using UnityEngine;
using TMPro;

public class LearnMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject learnPanel;
    [SerializeField] private int buttonPressed;
    public static event System.Action<int> toggleBarChange;

    private void OnEnable()
    {
        ProgressBar.buttonPressed += SetActiveButton;
    }

    private void Awake()
    {
        buttonPanel.SetActive(true);
        learnPanel.SetActive(false);
}

    public void ToggleButtonPanel()
    {
        buttonPanel.SetActive(true);
        learnPanel.transform.GetChild(buttonPressed).gameObject.SetActive(false);
        learnPanel.SetActive(false);
        toggleBarChange?.Invoke(buttonPressed);
    }

    public void ToggleLearnPanel()
    {
        buttonPanel.SetActive(false);
        learnPanel.SetActive(true);
        learnPanel.transform.GetChild(buttonPressed).gameObject.SetActive(true);
    }

    private void SetActiveButton(int i)
    {
        buttonPressed = i;
    }

    private void OnDisable()
    {
        ProgressBar.buttonPressed -= SetActiveButton;
    }
}
