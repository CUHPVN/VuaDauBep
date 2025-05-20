using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimingManager : MonoBehaviour
{
    public static TimingManager Instance { get; private set; }
    [SerializeField] private RectTransform Red;
    [SerializeField] private RectTransform Yellow;
    [SerializeField] private RectTransform Green;
    [SerializeField] private float redSizeX;
    [SerializeField] private float yPer= 0.75f;
    [SerializeField] private Vector2 ySize=Vector2.zero;
    [SerializeField] private float gPer= 0.25f;
    [SerializeField] private Vector2 gSize = Vector2.zero;
    [SerializeField] private const float constSpeed= 0.1f;
    [SerializeField] private float speed=0;
    [SerializeField] private bool Inc = true;
    [SerializeField] private bool isInteract=false;
    [SerializeField] private Slider Anchor;
    [SerializeField] private Image Notifi;
    private const string TimingCollectionAssetAddress = "TimingCollection";
    private static Dictionary<int, Vector2> _timingDictionary = new();
    //DebugZone
    [SerializeField] private bool IsDebug=false;
    [SerializeField] private Transform DebugPanel;
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
        LoadComponent();
    }
    void Start()
    {
        Red = transform.Find("RedBar").GetComponent<RectTransform>();
        redSizeX = Red.sizeDelta.x;
        Yellow = transform.Find("YellowBar").GetComponent<RectTransform>();
        Green = transform.Find("GreenBar").GetComponent<RectTransform>();
        Anchor = transform.Find("SliderBar").GetComponent<Slider>();
        Notifi = transform.Find("Notifi").GetComponent<Image>();
        DebugPanel = transform.Find("Debug");
        yellowSlider = DebugPanel.Find("Y").GetComponent<Slider>();
        greenSlider = DebugPanel.Find("G").GetComponent<Slider>();
        speedSlider = DebugPanel.Find("Speed").GetComponent<Slider>();
        yellowField = yellowSlider.GetComponentInChildren<TMP_InputField>();
        greenField = greenSlider.GetComponentInChildren<TMP_InputField>();
        speedField = speedSlider.GetComponentInChildren<TMP_InputField>();
        InitEvent();
        InitData();
    }

    void Update()
    {
        RunDebug();
        UpdateTiming();
        CheckSubmit();
    }
    private void FixedUpdate()
    {
        UpdateSlider();
    }
    public void StartDebug()
    {
        IsDebug = !IsDebug;
    }
    void RunDebug()
    {
        if (!IsDebug)
        {
            if (DebugPanel.gameObject.activeSelf) { DebugPanel.gameObject.SetActive(false); }
            return;
        }
        if (!DebugPanel.gameObject.activeSelf) {
            DebugPanel.gameObject.SetActive(true);
            speedSlider.value = speed;
            yellowSlider.value = yPer;
            greenSlider.value = gPer;
            yellowField.text = "" + yPer;
            greenField.text = "" + gPer;
            speedField.text = "" + speed;
        }
    }
    void InitData()
    {
        var timingData = Resources.Load<TimingCollection>(TimingCollectionAssetAddress);
        var dataTable = timingData.DataTable;
        foreach (var data in dataTable)
        {
            _timingDictionary[data.Level]=(new Vector2(data.Green,data.Yellow));
        }
        //foreach(var data in _timingDictionary)
        //{
        //    Debug.Log(data.ToString());
        //}
    }
    void InitEvent()
    {
        yellowSlider.onValueChanged.AddListener(ChangedYBySlider);
        greenSlider.onValueChanged.AddListener(ChangedGBySlider);
        speedSlider.onValueChanged.AddListener(ChangedSBySlider);
        yellowField.onSubmit.AddListener(ChangedYByInput);
        greenField.onSubmit.AddListener(ChangedGByInput);
        speedField.onSubmit.AddListener(ChangedSByInput);
    }
    void ChangedYByInput(string value)
    {
        yPer = float.Parse(value);
        yellowSlider.value = yPer;
    }
    void ChangedGByInput(string value)
    {
        gPer = float.Parse(value);
        greenSlider.value = gPer;
    }
    void ChangedSByInput(string value)
    {
        speed = float.Parse(value);
        speedSlider.value = speed;
    }
    void ChangedYBySlider(float value)
    {
        if (yPer == value) return;
        yPer = value;
        yellowField.text = "" + yPer;
    }
    void ChangedGBySlider(float value)
    {
        if (gPer == value) return;
        gPer = value;
        greenField.text = "" + gPer;
    }
    void ChangedSBySlider(float value)
    {
        if (speed == value) return;
        if (speed != 0)
        {
            speed = value;
        }
        speedField.text = "" + value;
    }
    void UpdateTiming()
    {
        ySize = new Vector2(yPer*redSizeX,Yellow.sizeDelta.y);
        gSize = new Vector2(gPer*redSizeX, Green.sizeDelta.y);
        Yellow.sizeDelta = ySize;
        Green.sizeDelta = gSize;
        if (Anchor.value == 0)  Inc = true;
        if (Anchor.value == 1)  Inc = false;
    }
    void UpdateSlider()
    {
        if (Inc)
        {
            Anchor.value += speed*constSpeed;
        }
        else
        {
            Anchor.value -= speed*constSpeed;
        }
    }
    void CheckSubmit()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isInteract)
            {
                isInteract = true;
                speed = 0;
                if (Mathf.Abs(Anchor.value - 0.5f) <= gPer / 2)
                {
                    Notifi.color = Color.green;
                }
                else if (Mathf.Abs(Anchor.value - 0.5f) <= yPer / 2)
                {
                    Notifi.color = Color.yellow;
                }
                else
                {
                    Notifi.color = Color.red;
                }
                Notifi.gameObject.SetActive(true);
                Invoke(nameof(TurnOff), 1f);
            }
        }
    }
    void TurnOff()
    {
        Notifi.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    void LoadComponent()
    {
        if(_timingDictionary == null) InitData();
        isInteract = false;
        if (IsDebug) speed = speedSlider.value;else
            speed = 0.2f;
        if (Anchor != null) 
            Anchor.value = 0;
    }

    public void TurnOn(int level)
    {
        if (_timingDictionary == null) InitData();
        if (level > _timingDictionary.Count) return;
        gPer = _timingDictionary[level].x/100f;
        yPer = _timingDictionary[level].y/100f;
    }
}
