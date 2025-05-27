using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimingManager : MonoBehaviour
{
    public static TimingManager Instance { get; private set; }

    [SerializeField] private Transform timingRoot;

    [SerializeField] private RectTransform redBar;
    [SerializeField] private RectTransform yellowBar;
    [SerializeField] private RectTransform greenBar;
    [SerializeField] private Slider anchorSlider;
    [SerializeField] private Image notification;

    [SerializeField] private float redSizeX;
    [SerializeField] private float yellowPercent = 0.75f;
    [SerializeField] private Vector2 yellowSize = Vector2.zero;
    [SerializeField] private float greenPercent = 0.25f;
    [SerializeField] private Vector2 greenSize = Vector2.zero;

    [SerializeField] private const float constSpeed = 0.1f;
    [SerializeField] private float speed = 0;
    [SerializeField] private bool increasing = true;
    [SerializeField] private bool isInteract = false;

    private const string TimingCollectionAssetAddress = "TimingCollection";
    private static Dictionary<int, Vector2> _timingDictionary = new();

    [SerializeField] private bool isDebug = false;
    [SerializeField] private Transform debugPanel;
    [SerializeField] private Slider yellowSlider;
    [SerializeField] private Slider greenSlider;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private TMP_InputField yellowField;
    [SerializeField] private TMP_InputField greenField;
    [SerializeField] private TMP_InputField speedField;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
            ActivateTimingUI();
    }

    private void Start()
    {
        redBar = timingRoot.Find("RedBar").GetComponent<RectTransform>();
        redSizeX = redBar.sizeDelta.x;

        yellowBar = timingRoot.Find("YellowBar").GetComponent<RectTransform>();
        greenBar = timingRoot.Find("GreenBar").GetComponent<RectTransform>();
        anchorSlider = timingRoot.Find("SliderBar").GetComponent<Slider>();
        notification = timingRoot.Find("Notifi").GetComponent<Image>();

        debugPanel = timingRoot.Find("Debug");
        yellowSlider = debugPanel.Find("Y").GetComponent<Slider>();
        greenSlider = debugPanel.Find("G").GetComponent<Slider>();
        speedSlider = debugPanel.Find("Speed").GetComponent<Slider>();

        yellowField = yellowSlider.GetComponentInChildren<TMP_InputField>();
        greenField = greenSlider.GetComponentInChildren<TMP_InputField>();
        speedField = speedSlider.GetComponentInChildren<TMP_InputField>();

        InitEvent();
        InitData();
    }

    private void Update()
    {
        RunDebug();
        UpdateTiming();
        CheckSubmit();
    }

    private void FixedUpdate()
    {
        UpdateSlider();
    }

    public void ActivateTimingUI()
    {
        if (_timingDictionary == null || _timingDictionary.Count == 0)
            InitData();

        isInteract = false;
        speed = isDebug ? speedSlider.value : 0.25f;
        if (anchorSlider != null)
            anchorSlider.value = 0;
    }

    public void ToggleDebug()
    {
        isDebug = !isDebug;
    }

    public void TurnOn(int level)
    {
        timingRoot.gameObject.SetActive(true);
        if (_timingDictionary == null || !_timingDictionary.ContainsKey(level)) return;
        ActivateTimingUI();
        var values = _timingDictionary[level];
        greenPercent = values.x / 100f;
        yellowPercent = values.y / 100f;
    }

    private void InitData()
    {
        var timingData = Resources.Load<TimingCollection>(TimingCollectionAssetAddress);
        foreach (var data in timingData.DataTable)
        {
            _timingDictionary[data.Level] = new Vector2(data.Green, data.Yellow);
        }
    }

    private void InitEvent()
    {
        yellowSlider.onValueChanged.AddListener(ChangedYBySlider);
        greenSlider.onValueChanged.AddListener(ChangedGBySlider);
        speedSlider.onValueChanged.AddListener(ChangedSBySlider);

        yellowField.onSubmit.AddListener(ChangedYByInput);
        greenField.onSubmit.AddListener(ChangedGByInput);
        speedField.onSubmit.AddListener(ChangedSByInput);
    }

    private void RunDebug()
    {
        if (!isDebug)
        {
            if (debugPanel.gameObject.activeSelf)
                debugPanel.gameObject.SetActive(false);
            return;
        }

        if (!debugPanel.gameObject.activeSelf)
        {
            debugPanel.gameObject.SetActive(true);
            speedSlider.value = speed;
            yellowSlider.value = yellowPercent;
            greenSlider.value = greenPercent;
            yellowField.text = yellowPercent.ToString();
            greenField.text = greenPercent.ToString();
            speedField.text = speed.ToString();
        }
    }

    private void ChangedYByInput(string value)
    {
        if (float.TryParse(value, out var result))
        {
            yellowPercent = result;
            yellowSlider.value = yellowPercent;
        }
    }

    private void ChangedGByInput(string value)
    {
        if (float.TryParse(value, out var result))
        {
            greenPercent = result;
            greenSlider.value = greenPercent;
        }
    }

    private void ChangedSByInput(string value)
    {
        if (float.TryParse(value, out var result))
        {
            speed = result;
            speedSlider.value = speed;
        }
    }

    private void ChangedYBySlider(float value)
    {
        if (Mathf.Approximately(yellowPercent, value)) return;
        yellowPercent = value;
        yellowField.text = value.ToString();
    }

    private void ChangedGBySlider(float value)
    {
        if (Mathf.Approximately(greenPercent, value)) return;
        greenPercent = value;
        greenField.text = value.ToString();
    }

    private void ChangedSBySlider(float value)
    {
        if (Mathf.Approximately(speed, value)) return;
        speed = value;
        speedField.text = value.ToString();
    }

    private void UpdateTiming()
    {
        yellowSize = new Vector2(yellowPercent * redSizeX, yellowBar.sizeDelta.y);
        greenSize = new Vector2(greenPercent * redSizeX, greenBar.sizeDelta.y);

        yellowBar.sizeDelta = yellowSize;
        greenBar.sizeDelta = greenSize;

        if (anchorSlider.value == 0) increasing = true;
        if (anchorSlider.value == 1) increasing = false;
    }

    private void UpdateSlider()
    {
        anchorSlider.value += (increasing ? 1 : -1) * speed * constSpeed;
    }

    private void CheckSubmit()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || isInteract) return;

        isInteract = true;
        speed = 0;

        float offset = Mathf.Abs(anchorSlider.value - 0.5f);
        notification.color = offset <= greenPercent / 2f ? Color.green :
                             offset <= yellowPercent / 2f ? Color.yellow : Color.red;

        notification.gameObject.SetActive(true);
        Invoke(nameof(TurnOff), 1f);
    }

    private void TurnOff()
    {
        notification.gameObject.SetActive(false);
        timingRoot.gameObject.SetActive(false);
    }
}
