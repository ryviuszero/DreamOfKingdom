using Mono.Cecil;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;

    // 更新房间的事件监听
    public void UpdateMapLayoutData(object value)
    {
        var roomVector = (Vector2Int)value;
        var currentRoom = mapLayout.mapRoomDataList.Find(r => r.column == roomVector.x && r.line == roomVector.y);
        currentRoom.roomState = RoomState.Visited;
        // 更新相邻房间的数据
        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(r => r.column == currentRoom.column && r.line != currentRoom.line);

        foreach (var room in sameColumnRooms)
        {
            room.roomState = RoomState.Locked;
        }

        foreach (var link in currentRoom.linkTo)
        {
            var linkedRoom = mapLayout.mapRoomDataList.Find(r => r.column == link.x && r.line == link.y);
            if (linkedRoom != null)
            {
                linkedRoom.roomState = RoomState.Attainable;
            }
        }


    }

}
