using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SynergyInfoPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title;

    [SerializeField]
    private TextMeshProUGUI _description;

    [SerializeField]
    private TextMeshProUGUI _status;

    private void Awake()
    {
        Hide();
    }

    public void Show(string synergyName, string synergyDescription, int currCount, int requirementCount)
    {
        _title.SetText(synergyName);
        _description.SetText(synergyDescription);
        _status.SetText(string.Format("{0}/{1}", currCount, requirementCount));
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

}
