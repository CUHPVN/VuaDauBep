using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObject;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }
    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == null) return;
        if(e.selectedCounter == baseCounter)
        {
            Show();
        }else Hide();
    }
    private void Show()
    {
        foreach (var item in visualGameObject)
        {
            item.SetActive(true);
        }
    }
    private void Hide()
    {
        foreach (var item in visualGameObject)
        {
            item.SetActive(false);
        }
    }
}
