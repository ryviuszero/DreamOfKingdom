using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class SceneLoadManager : MonoBehaviour
{

    private AssetReference currentScene;

    public AssetReference map;

    private Vector2Int currentRoomVector;

    [Header("广播")]
    public ObjectEventSO afterRoomLoadEvent;

    public async void OnLoadRoomEvent(object data)
    {
        if (data is Room)
        {
            Room currentRoom = data as Room;
            var currentData = currentRoom.roomData;
            currentRoomVector = new Vector2Int(currentRoom.column, currentRoom.line);
            currentScene = currentData.sceneToLoad;
        }
        // 卸载当前场景
        await UnLoadSceneTask();

        // 加载房间
        await LoadSceneTask();

        afterRoomLoadEvent.RaiseEvent(currentRoomVector, this);

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
