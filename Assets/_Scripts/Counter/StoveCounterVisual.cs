using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;
    private void Awake()
    {
    }
    private void Start()
    {
       stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e.cookingState== StoveCounter.CookingState.Cooking|| e.cookingState == StoveCounter.CookingState.Cooked;
        stoveOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}
