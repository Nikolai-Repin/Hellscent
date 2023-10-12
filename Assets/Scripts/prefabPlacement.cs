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
		// Creates the starting room and starts the generation
		GameObject startRoom = createRoom(spawnRoom, null, false);
		createDungeon(startRoom, 4, 5);
	}

	private void createDungeon(GameObject startRoom, int mainBranchLen, int branchCap)
    {
		// Loops between all available doors of the current room
		for (int i = 0; i < getNumAvailable(startRoom.GetComponent<RoomInfo>()); i++)
        {
			if (mainBranchLen > 0)
            {
				// Determines if the room being created is a main branch(a branch that leads to the end)
				createDungeon(createRoom(rooms[0], startRoom), mainBranchLen - 1, branchCap - 1);
				mainBranchLen = 0;
            }
			else if (branchCap > 0 && Random.Range(0, 3) == 0)
            {
				// Determines if any more branches should be added and rolls a dice to see if the room will be created
				createDungeon(createRoom(rooms[0], startRoom), 0, branchCap - 1);
			}
        }
    }

	private GameObject createRoom(GameObject room, GameObject toAlign, bool start=false)
    {
		// Initializes a room
		float randomDir = start ? Random.Range(0, 4) : 0;
		GameObject createdRoom = Instantiate(room, new Vector2(0, 0), Quaternion.Euler(0, 0, randomDir * 90));
		createdRoom.transform.SetParent(transform, false);
		dungeon.Add(createdRoom);
		if (!start)
        {
			// Aligns created room to previous room
			alignRooms(toAlign.transform, createdRoom.transform, roomSpacing);
		}
		return createdRoom;
	}

	private void alignRooms(Transform origin, Transform created, float spacing) {
		// Gets all necessary information about the origin room
		RoomInfo origin_data = origin.GetComponent<RoomInfo>();
		string direction_origin = origin_data.doorDirection[findAvailableDoor(origin_data)];
		Transform origin_join_point = origin.Find("Join Point " + direction_origin).Find("Join Point");
		origin_data.doorOccupation[origin_data.doorDirection.IndexOf(direction_origin)] = true;

		// Finds a door on created room that will connect with origin
		string true_dir = directionNumber[directionNumber.IndexOf(direction_origin) + (int) (origin.rotation.eulerAngles.z / 90)];
		string direction_created;
		RoomInfo created_data = created.GetComponent<RoomInfo>();
		string true_created_dir;
		if (created_data.doorDirection.Count == 4) {
			// If the room has four doors do not rotate
			direction_created = reverseDirection[true_dir];
			true_created_dir = direction_created;
		} else {
			// Randomly rotates rooms with less than 4 doors
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

		// Aligns the created room to the origin room
		Vector2 shift;
		if (true_created_dir == "North" || true_created_dir == "South") {
			shift = new Vector2(origin_join_point.position.x - created_join_point.position.x, origin_join_point.position.y + ((true_created_dir == "North" ? -spacing : roomSpacing)) - created_join_point.position.y);
		} else {
			shift = new Vector2(origin_join_point.position.x + (true_created_dir == "West" ? roomSpacing : -spacing) - created_join_point.position.x, origin_join_point.position.y - created_join_point.position.y);
		}
		created.Translate(shift, Space.World);
	}

	private int findAvailableDoor(RoomInfo data)
    {
		// Finds a random available door for a given room
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

	private int getNumAvailable(RoomInfo data)
    {
		// Gets the number of all available doors for a given room
		int count = 0;
		foreach (bool occupied in data.doorOccupation)
        {
			if (!occupied)
            {
				count++;
            }
        }
		return count;
    }
}
