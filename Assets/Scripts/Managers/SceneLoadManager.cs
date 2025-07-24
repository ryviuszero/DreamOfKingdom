using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class SceneLoadManager : MonoBehaviour
{

    private AssetReference currentScene;

    public AssetReference map;


    public async void OnLoadRoomEvent(object data)
    {
        if (data is RoomDataSO)
        {
            var currentData = (RoomDataSO)data;
            Debug.Log("Loading room: " + currentData.roomType);

            currentScene = currentData.sceneToLoad;
        }
        // 卸载当前场景
        await UnLoadSceneTask();

        // 加载房间
        await LoadSceneTask();

    }

    // 异步操作加载场景
    private async Awaitable LoadSceneTask()
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await s.Task;
        if (s.Status == AsyncOperationStatus.Succeeded)
        {
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    private async Awaitable UnLoadSceneTask()
    {
        await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    // 监听返回房间的事件函数
    public async void LoadMap()
    {
        await UnLoadSceneTask();

        currentScene = map;
        await LoadSceneTask();

    }
  

}
