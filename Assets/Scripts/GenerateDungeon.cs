using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;

public class GenerateDungeon : MonoBehaviour
{
	[SerializeField] private GameObject dungeonLimits;
    [SerializeField] private GameObject[] rooms;
	[SerializeField] private GameObject[] hallways;
	[SerializeField] private GameObject spawnRoom;
	[SerializeField] private GameObject bossRoom;
	[SerializeField] private GameObject endCap;
	[SerializeField] private GameObject closedDoor;
	[SerializeField] private float roomSpacing;
	[SerializeField] private int mainBranchLength;
	[SerializeField] private int offshootBranchCap;
	[SerializeField] private int waitingFrames;
	[SerializeField] private int onlyBranchRooms;
	[SerializeField, Range(0, 100)] private int tryRoomChance;
	[SerializeField, Range(0, 100)] private int roomChance;
	[SerializeField, Range(0, 100)] private int hallwayChance;
	public bool dungeonOver = false;

	private bool go = true;
	private bool success = true;

    private List<GameObject> dungeon = new();
	private List<GameObject> closedDoors = new();

    private readonly Dictionary<string, string> reverseDirection = new()
	{
		{"North", "South"},
		{"West", "East"},
		{"South", "North"},
		{"East", "West"}
	};

    private readonly List<string> directionNumber = new() { "North", "West", "South", "East" };

	void Update() {
		if (go) {
			go = false;
			GameObject limits = Instantiate(dungeonLimits, new Vector2(), new Quaternion());
			limits.transform.SetParent(transform);
			foreach (string lim in directionNumber) {
				dungeon.Add(limits.transform.Find("Limit " + lim).gameObject);
			}
			GameObject startRoom = CreateRoom(spawnRoom, true);
			dungeon.Add(startRoom);
			StartCoroutine(CreateDungeon(startRoom, mainBranchLength, offshootBranchCap));
			StartCoroutine(detectEnd());
		}
	}

	IEnumerator detectEnd() {
		bool end = false;
		while (!end) {
			int start = dungeon.Count;
			yield return StartCoroutine(waitFrames(waitingFrames * 10));
			if (start == dungeon.Count) {
				end = true;
			}
		}
		if (success) {
			foreach (GameObject dr in dungeon) {
				if (dr.name == bossRoom.name + "(Clone)") {
					CapDoors();
					yield return null;
					AstarPath.active.Scan();
					yield break;
				}
			}
		}
		foreach (GameObject dr in dungeon) {
			Destroy(dr);
		}
		dungeon = new();
		GameObject limits = Instantiate(dungeonLimits, new Vector2(), new Quaternion());
		limits.transform.SetParent(transform);
		foreach (string lim in directionNumber) {
			dungeon.Add(limits.transform.Find("Limit " + lim).gameObject);
		}
		GameObject startRoom = CreateRoom(spawnRoom, true);
		success = true;
		dungeon.Add(startRoom);
		StartCoroutine(CreateDungeon(startRoom, mainBranchLength, offshootBranchCap));
		StartCoroutine(detectEnd());
	}

	IEnumerator CreateDungeon(GameObject origin, int mainBranch, int branchCap) {
		int nextMainBranch = mainBranch;
		int nextBranchCap = branchCap;
		bool usedHallway = false;
		bool continueDungeon = false;
		string door = "";
		bool boss = false;
		GameObject nextOrigin = origin;
		if (GetNumAvailable(origin.GetComponent<RoomInfo>()) == 0) {
			if (mainBranch > 1) {
				int nextIndex = dungeon.IndexOf(origin) - 1;
				for (int i = nextIndex; i >= 0; i--) {
					if (GetNumAvailable(dungeon[i].GetComponent<RoomInfo>()) > 0) {
						yield return StartCoroutine(CreateDungeon(origin, mainBranch, branchCap));
						yield break;
					}
				}
				success = false;
				yield break;
			}
			yield break;
		}
		if (mainBranch < 1 && branchCap < 1) 
		{
			yield break;
		}
		if (mainBranch > 0)
        {
			if (mainBranch == 1) {
				boss = true;
			}
			continueDungeon = true;
			nextMainBranch--;
        }
		else if (branchCap > 0 && Random.Range(0, 101) <= roomChance)
        {
			nextMainBranch = 0;
			continueDungeon = true;
			nextBranchCap--;
		}
		if (boss || Random.Range(0, 101) <= hallwayChance)
        {
			GameObject nextHallway = CreateRoom(hallways[0], false);
			door = AlignRooms(nextOrigin.transform, nextHallway.transform, roomSpacing);

			yield return StartCoroutine(waitFrames(waitingFrames));
			foreach (GameObject dr in dungeon)
			{
				if (nextHallway.GetComponent<CompositeCollider2D>().bounds.Intersects(dr.GetComponent<CompositeCollider2D>().bounds))
				{
					Destroy(nextHallway);
					if (GetNumAvailable(origin.GetComponent<RoomInfo>()) > 0) {
						yield return StartCoroutine(CreateDungeon(origin, mainBranch, branchCap));
					} else if (mainBranch > 0) {
						int nextIndex = dungeon.IndexOf(origin) - 1;
						for (int i = nextIndex; i >= 0; i--) {
							if (GetNumAvailable(dungeon[i].GetComponent<RoomInfo>()) > 0) {
								yield return StartCoroutine(CreateDungeon(origin, mainBranch, branchCap));
								yield break;
							}
						}
						success = false;
					}
					yield break;
				}
			}
			nextOrigin = nextHallway;
			usedHallway = true;
			nextOrigin.GetComponent<RoomInfo>().trueOccupancy[0] = true;
			nextOrigin.GetComponent<RoomInfo>().trueOccupancy[1] = true;
		}
		GameObject nextRoom = CreateRoom(boss ? bossRoom : rooms[Random.Range(0, rooms.Length - (mainBranch > 0 ? onlyBranchRooms : 0))], false);
		string hallDoor = AlignRooms(nextOrigin.transform, nextRoom.transform, roomSpacing);
		if (!usedHallway) {
			door = hallDoor;
		}
		yield return StartCoroutine(waitFrames(waitingFrames));
		foreach (GameObject dr in dungeon) {
			if (nextRoom.GetComponent<CompositeCollider2D>().bounds.Intersects(dr.GetComponent<CompositeCollider2D>().bounds))
			{
				if (usedHallway) {
					Destroy(nextOrigin);
				}
				Destroy(nextRoom);
				if (GetNumAvailable(origin.GetComponent<RoomInfo>()) > 0) {
						yield return StartCoroutine(CreateDungeon(origin, mainBranch, branchCap));
					} else if (mainBranch > 0) {
						int nextIndex = dungeon.IndexOf(origin) - 1;
						for (int i = nextIndex; i >= 0; i--) {
							if (GetNumAvailable(dungeon[i].GetComponent<RoomInfo>()) > 0) {
								yield return StartCoroutine(CreateDungeon(origin, mainBranch, branchCap));
								yield break;
							}
						}
						success = false;
					}
				yield break;
			}
		}
		RoomInfo originData = origin.GetComponent<RoomInfo>();
		originData.trueOccupancy[originData.doorDirection.IndexOf(door)] = true;
		if (usedHallway) {
			dungeon.Add(nextOrigin);
		}
		dungeon.Add(nextRoom);
		if (continueDungeon) 
		{
			yield return StartCoroutine(CreateDungeon(nextRoom, nextMainBranch, nextBranchCap));
		}
		if (GetNumAvailable(origin.GetComponent<RoomInfo>()) > 0 && Random.Range(0, 101) <= tryRoomChance) 
		{
			yield return StartCoroutine(CreateDungeon(origin, 0, branchCap));
		} 
    }

	private void CapDoors() {
		foreach (GameObject dr in dungeon) {
			RoomInfo data = dr.GetComponent<RoomInfo>();
			for (int i = 0; i < data.trueOccupancy.Count; i++) {
				bool o = data.trueOccupancy[i];
				if (!o) {
					GameObject cap = Instantiate(endCap, new Vector2(0, 0), Quaternion.Euler(0, 0, 0));
					cap.transform.SetParent(transform, false);
					AlignRooms(dr.transform, cap.transform, 0, data.doorDirection[i]);
				}	
			}
		}
	}

    private GameObject CreateRoom(GameObject room, bool start)
    {
		// Initializes a room
		float randomDir = start ? Random.Range(0, 4) : 0;
		GameObject createdRoom = Instantiate(room, new Vector2(0, 0), Quaternion.Euler(0, 0, randomDir * 90));
		createdRoom.transform.SetParent(transform, false);
		return createdRoom;
	}

	private string AlignRooms(Transform origin, Transform created, float spacing, string force = null)
	{
		// Gets all necessary information about the origin room
		RoomInfo origin_data = origin.GetComponent<RoomInfo>();
		string direction_origin = force == null ? origin_data.doorDirection[FindAvailableDoor(origin_data)] : force;
		Transform origin_join_point = origin.Find("Join Point " + direction_origin);
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
		Transform created_join_point = created.Find("Join Point " + direction_created);

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
		return direction_origin;
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

	IEnumerator waitFrames(int frames) {
		for (int i = 0; i < frames; i++) {
			yield return null;
		}
	}

	public void LockRoom(GameObject room) {
		RoomInfo data = room.GetComponent<RoomInfo>();
		for (int i = 0; i < data.trueOccupancy.Count; i++) {
			bool o = data.trueOccupancy[i];
			if (o) {
				GameObject cap = Instantiate(closedDoor, new Vector2(0, 0), Quaternion.Euler(0, 0, 0));
				cap.transform.SetParent(transform, false);
				closedDoors.Add(cap);
				AlignRooms(room.transform, cap.transform, 0, data.doorDirection[i]);
			}
		}
	}

	public void UnlockRooms() {
		foreach (GameObject part in closedDoors) {
			if (part.name == closedDoor.name + "(Clone)") {
				Destroy(part);
			}
		}
		closedDoors.Clear();
	}
}
