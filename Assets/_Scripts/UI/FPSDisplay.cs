using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsText;
    private float deltaTime;
    [SerializeField] private float targetFPS=60;

    private void Awake()
    {
    }
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = $"FPS: {Mathf.FloorToInt(fps)}";
        float scale = Mathf.Clamp(fps / targetFPS, 0.5f, 1f);
        ScalableBufferManager.ResizeBuffers(scale, scale);
    }
}
