using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpawnMap : MonoBehaviour
{
    [SerializeField] private string mapKey = "Assets/Prefabs/Environment/Kitchen.prefab";
    private AsyncOperationHandle<GameObject> handle;
    private GameObject map;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _ = Spawn();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Destroy();
        }
    }

    private async UniTask Spawn()
    {
        handle = Addressables.LoadAssetAsync<GameObject>(mapKey);
        await handle.ToUniTask();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            map = Instantiate(handle.Result);
        }
        else
        {
            Debug.LogError($"Failed to load Addressable: {mapKey}");
        }
    }
    private void Destroy()
    {
        GameObject.Destroy(map);
        handle.Release();
    }

}
