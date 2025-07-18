using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("地图配置")]
    public MapConfigSO mapConfig;
    [Header("预制体")]
    public Room roomPrefab;
    public LineRenderer linePrefab;
    private float screenHeight;
    private float screenWidth;

    private float columnWidth;

    private Vector3 generatePoint;

    public float border;

    private List<Room> rooms = new();
    private List<LineRenderer> lines = new();

    public List<RoomDataSO> roomDataList = new();
    private Dictionary<RoomType, RoomDataSO> roomDataDict = new();

    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;

        columnWidth = screenWidth / mapConfig.roomBlueprints.Count;

        foreach (var roomData in roomDataList)
        {
            roomDataDict.Add(roomData.roomType, roomData);
        }

    }

    private void Start()
    {
        CreateMap();
    }

    public void CreateMap()
    {

        List<Room> previousColumnRooms = new();
        for (int column = 0; column < mapConfig.roomBlueprints.Count; column++)
        {
            var blueprint = mapConfig.roomBlueprints[column];

            var amount = Random.Range(blueprint.min, blueprint.max + 1);

            var startHeight = (screenHeight / 2) - (screenHeight / (amount + 1));

            generatePoint = new Vector3(-(screenWidth / 2) + border + columnWidth * column, startHeight, 0);

            var newPosition = generatePoint;

            List<Room> currentColumnRooms = new();

            var roomGapY = screenHeight / (amount + 1);

            for (int i = 0; i < amount; i++)
            {

                if (column == mapConfig.roomBlueprints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - border;
                }
                else if (column != 0)
                {
                    newPosition.x += Random.Range(-border / 4, border / 4);
                }

                newPosition.y = startHeight - roomGapY * i;
                // 生成房间
                var room = Instantiate(roomPrefab, newPosition, Quaternion.identity, transform);
                RoomType newType = GetRandomRoomType(mapConfig.roomBlueprints[column].roomType);
                room.SetupRoom(column, i, GetRoomData(newType));

                rooms.Add(room);
                currentColumnRooms.Add(room);
            }

            if (column != 0)
            {
                // 画线
                createConnectingLine(previousColumnRooms, currentColumnRooms);
            }

            previousColumnRooms = currentColumnRooms;
        }
    }

    private void createConnectingLine(List<Room> column1, List<Room> column2)
    {
        HashSet<Room> connectedColumn2Room = new();
        foreach (var room in column1)
        {
            var targetRoom = ConnectToRandomRoom(room, column2);
            connectedColumn2Room.Add(targetRoom);
        }

        foreach (var room in column2)
        {
            if (!connectedColumn2Room.Contains(room))
            {
                //如果没有连接到，就随机连接一个
                ConnectToRandomRoom(room, column1);
            }
        }
    }

    private Room ConnectToRandomRoom(Room room, List<Room> column2)
    {
        Room targetRoom;
        targetRoom = column2[Random.Range(0, column2.Count)];

        //创建房间之前的连线
        var line = Instantiate(linePrefab, transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);
        lines.Add(line);
        return targetRoom;
    }

    // 重新生成房间
    [ContextMenu("ReGenerateRoom")]
    public void ReGenerateRoom()
    {
        foreach (var room in rooms)
        {
            Destroy(room.gameObject);
        }

        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }
        rooms.Clear();
        lines.Clear();
        CreateMap();
    }

    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDict[roomType];
    }

    private RoomType GetRandomRoomType(RoomType flags)
    {
        string[] options = flags.ToString().Split(',');
        string randomOption = options[Random.Range(0, options.Length)];

        RoomType roomType = (RoomType)System.Enum.Parse(typeof(RoomType), randomOption);

        return roomType;

    }
}
 