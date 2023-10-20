using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PrefabPlacement : MonoBehaviour
{
    [SerializeField] private GameObject[] rooms;
	[SerializeField] private GameObject[] hallways;
	[SerializeField] private GameObject spawnRoom;
	[SerializeField] private GameObject bossRoom;
	[SerializeField] private float roomSpacing;
	[SerializeField] private int mainBranchLength;
	[SerializeField] private int offshootBranchCap;

	private List<GameObject> dungeon = new();

    private readonly Dictionary<string, string> reverseDirection = new()
	{
		{"North", "South"},
		{"West", "East"},
		{"South", "North"},
		{"East", "West"}
	};

    private readonly List<string> directionNumber = new() { "North", "West", "South", "East" };

	void Start () {
		// Creates the starting room and starts the generation
		GameObject startRoom = CreateRoom(spawnRoom, null, false, true);
		CreateDungeon(startRoom, mainBranchLength, offshootBranchCap);
	}

	private void CreateDungeon(GameObject startRoom, int mainBranchLen, int branchCap)
    {
		// Loops between all available doors of the current room
		for (int i = 0; i < GetNumAvailable(startRoom.GetComponent<RoomInfo>()); i++)
        {
			if (mainBranchLen > 0)
            {
				// Determines if the room being created is a main branch(a branch that leads to the end)
				if (mainBranchLen == 1){
					GameObject hallway = CreateRoom(hallways[0], startRoom, true);
					if (hallway == null)
					{
						for (int j = dungeon.Count - 1; j >= 0; j--)
						{
							GameObject room = dungeon[j];
							if (GetNumAvailable(room.GetComponent<RoomInfo>()) > 0)
							{
								CreateDungeon(room, mainBranchLen, branchCap);
								break;
							}
						}
						break;
					}
					GameObject created = CreateRoom(bossRoom, hallway, true);
					if (created == null)
					{
						dungeon.Remove(hallway);
						Destroy(hallway);
						for (int j = dungeon.Count - 1; j >= 0; j--)
						{
							GameObject room = dungeon[j];
							if (GetNumAvailable(room.GetComponent<RoomInfo>()) > 0)
							{
								CreateDungeon(room, mainBranchLen, branchCap);
								break;
							}
							if (j == 0)
							{
								dungeon = new();
								GameObject start = CreateRoom(spawnRoom, null, true);
								CreateDungeon(start, mainBranchLength, offshootBranchCap);
							}
						}
						break;
					}
					CreateDungeon(created, mainBranchLen - 1, branchCap - 1);
				} else {
					if (Random.Range(0, 8) != 0) {
						GameObject hallway = CreateRoom(hallways[0], startRoom, true);
						if (hallway == null)
                        {
							for (int j = dungeon.Count - 1; j >= 0; j--)
                            {
								GameObject room = dungeon[j];
								if (GetNumAvailable(room.GetComponent<RoomInfo>()) > 0)
                                {
									CreateDungeon(room, mainBranchLen, branchCap);
									break;
                                }
                            }
							break;
                        }
						GameObject created = CreateRoom(rooms[Random.Range(0, rooms.Length)], hallway, true);
						if (created == null)
						{
							dungeon.Remove(hallway);
							Destroy(hallway);
							for (int j = dungeon.Count - 1; j >= 0; j--)
							{
								GameObject room = dungeon[j];
								if (GetNumAvailable(room.GetComponent<RoomInfo>()) > 0)
								{
									CreateDungeon(room, mainBranchLen, branchCap);
									break;
								}
								if (j == 0)
                                {
									dungeon = new();
									GameObject start = CreateRoom(spawnRoom, null, true);
									CreateDungeon(start, mainBranchLength, offshootBranchCap);
								}
							}
							break;
						}
						CreateDungeon(created, mainBranchLen - 1, branchCap - 1);
					} else {
						GameObject created = CreateRoom(rooms[Random.Range(0, rooms.Length)], startRoom, true);
						if (created == null)
						{
							for (int j = dungeon.Count - 1; j >= 0; j--)
							{
								GameObject room = dungeon[j];
								if (GetNumAvailable(room.GetComponent<RoomInfo>()) > 0)
								{
									CreateDungeon(room, mainBranchLen, branchCap);
									break;
								}
							}
							break;
						}
						CreateDungeon(created, mainBranchLen - 1, branchCap - 1);
					}
				}
				mainBranchLen = 0;
            }
			else if (branchCap > 0 && Random.Range(0, 5) != 0)
            {
				// Determines if any more branches should be added and rolls a dice to see if the room will be created
				if (Random.Range(0, 8) != 0) {
					GameObject hallway = CreateRoom(hallways[0], startRoom, false);
					if (hallway == null)
                    {
						break;
                    }
					GameObject created = CreateRoom(rooms[Random.Range(0, rooms.Length)], hallway, false);
					if (created == null)
					{
						dungeon.Remove(hallway);
						Destroy(hallway);
						break;
					}
					CreateDungeon(created, 0, branchCap - 1);
				} else {
					GameObject created = CreateRoom(rooms[Random.Range(0, rooms.Length)], startRoom, false);
					if (created == null)
					{
						break;
					}
					CreateDungeon(created, 0, branchCap - 1);
				}
			}
        }
    }

	private GameObject CreateRoom(GameObject room, GameObject toAlign, bool force, bool start=false)
    {
		// Initializes a room
		float randomDir = start ? Random.Range(0, 4) : 0;
		GameObject createdRoom = Instantiate(room, new Vector2(0, 0), Quaternion.Euler(0, 0, randomDir * 90));
		createdRoom.transform.SetParent(transform, false);
		if (!start)
        {
			// Aligns created room to previous room
			if (!AlignRooms(toAlign.transform, createdRoom.transform, roomSpacing, force))
            {
				return null;
            }
		}
		dungeon.Add(createdRoom);
		return createdRoom;
	}

	private bool AlignRooms(Transform origin, Transform created, float spacing, bool force)
	{
		// Gets all necessary information about the origin room
		RoomInfo origin_data = origin.GetComponent<RoomInfo>();
		string direction_origin = origin_data.doorDirection[FindAvailableDoor(origin_data)];
		Transform origin_join_point = origin.Find("Join Point " + direction_origin).Find("Join Point");
		origin_data.doorOccupation[origin_data.doorDirection.IndexOf(direction_origin)] = true;

		// Finds a door on created room that will connect with origin
		string true_dir = directionNumber[(directionNumber.IndexOf(direction_origin) + (int)(origin.rotation.eulerAngles.z / 90)) % 4];
		string direction_created;
		RoomInfo created_data = created.GetComponent<RoomInfo>();
		string true_created_dir;
		if (created_data.doorDirection.Count == 4)
		{
			// If the room has four doors do not rotate
			direction_created = reverseDirection[true_dir];
			true_created_dir = direction_created;
			created_data.doorOccupation[created_data.doorDirection.IndexOf(direction_created)] = true;
		}
		else
		{
			// Randomly rotates rooms with less than 4 doors	
			created.transform.rotation = Quaternion.Euler(0, 0, 0);
			string selected_door = created_data.doorDirection[FindAvailableDoor(created_data)];
			int selected_door_dir = directionNumber.IndexOf(selected_door);
			int dir_diff = directionNumber.IndexOf(reverseDirection[true_dir]) - selected_door_dir;
			created.transform.Rotate(0.0f, 0.0f, dir_diff * 90, Space.World);
			direction_created = selected_door;
			true_created_dir = directionNumber[(directionNumber.IndexOf(selected_door) + (int)(created.rotation.eulerAngles.z / 90)) % 4];
			created_data.doorOccupation[created_data.doorDirection.IndexOf(selected_door)] = true;
		}
		Transform created_join_point = created.Find("Join Point " + direction_created).Find("Join Point");

		// Aligns the created room to the origin room
		Vector2 shift;
		if (true_created_dir == "North" || true_created_dir == "South")
		{
			shift = new Vector2(origin_join_point.position.x - created_join_point.position.x, origin_join_point.position.y + (true_created_dir == "South" ? spacing : -spacing) - created_join_point.position.y);
		}
		else
		{
			shift = new Vector2(origin_join_point.position.x + (true_created_dir == "West" ? spacing : -spacing) - created_join_point.position.x, origin_join_point.position.y - created_join_point.position.y);
		}
		created.Translate(shift, Space.World);
		foreach (GameObject gb in dungeon) {
			if (created.gameObject.GetComponent<CompositeCollider2D>().bounds.Intersects(gb.GetComponent<CompositeCollider2D>().bounds))
			{
				Debug.Log(gb.name);
				if (force && GetNumAvailable(origin_data) > 0)
                {
					if (!AlignRooms(origin, created, spacing, force))
                    {
						//dungeon.Remove(created.gameObject);
						//Destroy(created.gameObject);
						return false;
					}
					return true;
				}
				return false;
			}
		}
		return true;
	}

	private int FindAvailableDoor(RoomInfo data)
    {
		// Finds a random available door for a given room
		List<int> available = new();
		for (int i = 0; i < data.doorOccupation.Count; i++)
        {
			if (!data.doorOccupation[i])
            {
				available.Add(i);
            }
        }
		return available.Count == 0 ? -1 : available[Random.Range(0, available.Count)];
    }

	private int GetNumAvailable(RoomInfo data)
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
