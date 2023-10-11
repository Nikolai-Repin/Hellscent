using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class prefabPlacement : MonoBehaviour
{
    [SerializeField] private GameObject[] rooms;
	[SerializeField] private GameObject[] hallways;
	[SerializeField] private GameObject spawnRoom;
	[SerializeField] private float roomSpacing;

	private List<GameObject> dungeon = new List<GameObject>();

	Dictionary<string, string> reverseDirection = new Dictionary<string, string>()
	{
		{"North", "South"},
		{"West", "East"},
		{"South", "North"},
		{"East", "West"}
	};

	List<string> directionNumber = new List<string>(){"North", "West", "South", "East"};

	void Start () {
		GameObject startRoom = createRoom(spawnRoom, null, true);

		createRoom(spawnRoom, startRoom);
	}

	private GameObject createRoom(GameObject room, GameObject toAlign, bool start=false)
    {
		RoomInfo data = room.GetComponent<RoomInfo>();
		float randomDir = start ? Random.Range(0, 4) : 0;
		GameObject createdRoom = Instantiate(room, new Vector2(0, 0), Quaternion.Euler(0, 0, randomDir * 90));
		createdRoom.transform.SetParent(transform, false);
		dungeon.Add(createdRoom);
		if (!start)
        {
			alignRooms(toAlign.transform, createdRoom.transform);
		}
		return createdRoom;
	}

	private void alignRooms(Transform origin, Transform created) {
		RoomInfo origin_data = origin.GetComponent<RoomInfo>();
		string direction_origin = origin_data.doorDirection[findAvailableDoor(origin_data)];
		Transform origin_join_point = origin.Find("Join Point " + direction_origin).Find("Join Point");
		origin_data.doorOccupation[origin_data.doorDirection.IndexOf(direction_origin)] = true;

		string true_dir = directionNumber[directionNumber.IndexOf(direction_origin) + (int) (origin.rotation.eulerAngles.z / 90)];
		string direction_created;
		RoomInfo created_data = created.GetComponent<RoomInfo>();
		string true_created_dir = "";
		if (created_data.doorDirection.Count == 4) {
			direction_created = reverseDirection[true_dir];
			true_created_dir = direction_created;
		} else {
			created.transform.rotation = Quaternion.Euler(0, 0, 0);
			string selected_door = created_data.doorDirection[Random.Range(0, created_data.doorDirection.Count)];
			int selected_door_dir = directionNumber.IndexOf(selected_door);
			int dir_diff = directionNumber.IndexOf(reverseDirection[true_dir]) - selected_door_dir;
			created.transform.Rotate(0.0f, 0.0f, dir_diff * 90, Space.World);
			direction_created = selected_door;
			true_created_dir = directionNumber[directionNumber.IndexOf(selected_door) + (int)(created.rotation.eulerAngles.z / 90)];
			created_data.doorOccupation[created_data.doorDirection.IndexOf(selected_door)] = true;
		}
		Transform created_join_point = created.Find("Join Point " + direction_created).Find("Join Point");

		Vector2 shift;
		if (true_created_dir == "North" || true_created_dir == "South") {
			shift = new Vector2(origin_join_point.position.x - created_join_point.position.x, origin_join_point.position.y + ((true_created_dir == "North" ? -roomSpacing : roomSpacing)) - created_join_point.position.y);
		} else {
			shift = new Vector2(origin_join_point.position.x + (true_created_dir == "West" ? roomSpacing : -roomSpacing) - created_join_point.position.x, origin_join_point.position.y - created_join_point.position.y);
		}
		created.Translate(shift, Space.World);
	}

	private int findAvailableDoor(RoomInfo data)
    {
		List<int> available = new List<int>();
		for (int i = 0; i < data.doorOccupation.Count; i++)
        {
			if (!data.doorOccupation[i])
            {
				available.Add(i);
            }
        }
		return available.Count == 0 ? -1 : available[Random.Range(0, available.Count)];
    }
}
