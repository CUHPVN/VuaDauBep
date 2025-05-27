using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class ProgressClockUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private IHasProgress hasProgress;
    [SerializeField] private UnityEngine.UI.Image BGImage;
    [SerializeField] private UnityEngine.UI.Image clockImage;
    void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if(hasProgress == null )
        {
            Debug.Log("No 'IHasProgress' Object");
            return;
        }
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        clockImage.fillAmount = 0;
        clockImage.color = Color.white;
        Hide();
    }
    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        clockImage.fillAmount = e.progressNormalized;
        clockImage.color = e.color;
        if(e.progressNormalized == 0)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
    void Hide()
    {
        clockImage.gameObject.SetActive(false);
        BGImage.gameObject.SetActive(false);
    }
    void Show()
    {
        clockImage.gameObject.SetActive(true);
        BGImage.gameObject.SetActive(true);
    }
}
