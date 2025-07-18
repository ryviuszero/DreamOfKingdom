using UnityEngine;

public class SceneLoadManager : MonoBehaviour
{
    public void OnLoadRoomEvent(object data)
    {
        if (data is RoomDataSO)
        {
            var currentData = (RoomDataSO)data;
            Debug.Log("Loading room: " + currentData.roomType);
        }
    }
  

}
