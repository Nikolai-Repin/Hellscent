using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDungeon : MonoBehaviour
{
    [SerializeField] private GameObject[] rooms;
	[SerializeField] private GameObject[] hallways;
	[SerializeField] private GameObject spawnRoom;
	[SerializeField] private GameObject bossRoom;
	[SerializeField] private float roomSpacing;
	[SerializeField] private int mainBranchLength;
	[SerializeField] private int offshootBranchCap;
	[SerializeField] private int waitingFrames;

	private bool go = true;

    private List<GameObject> dungeon = new();

    private readonly Dictionary<string, string> reverseDirection = new()
	{
		{"North", "South"},
		{"West", "East"},
		{"South", "North"},
		{"East", "West"}
	};

    private readonly List<string> directionNumber = new() { "North", "West", "South", "East" };

    void Start() {
		//GameObject startRoom = CreateRoom(spawnRoom, true);
		//dungeon.Add(startRoom);
		//StartCoroutine(CreateDungeon(startRoom, mainBranchLength, offshootBranchCap));
    }

	void Update() {
		if (go) {
			go = false;
			GameObject startRoom = CreateRoom(spawnRoom, true);
			dungeon.Add(startRoom);
			StartCoroutine(CreateDungeon(startRoom, mainBranchLength, offshootBranchCap));
		}
	}

	IEnumerator CreateDungeon(GameObject origin, int mainBranch, int branchCap) {
		int nextMainBranch = mainBranch;
		int nextBranchCap = branchCap;
		bool usedHallway = false;
		bool continueDungeon = false;
		GameObject nextOrigin = origin;
		if (mainBranch < 1 && branchCap < 1) 
		{
			yield break;
		}
		if (mainBranch > 0)
        {
			continueDungeon = true;
			nextMainBranch--;
        }
		else if (branchCap > 0 && Random.Range(0, 5) != 0)
        {
			continueDungeon = true;
			nextBranchCap--;
        }
		if (Random.Range(0, 8) != 0)
        {
			GameObject nextHallway = CreateRoom(hallways[0], false);
			AlignRooms(nextOrigin.transform, nextHallway.transform, roomSpacing);
			yield return StartCoroutine(waitFrames(waitingFrames));
			foreach (GameObject dr in dungeon)
			{
				if (nextHallway.GetComponent<CompositeCollider2D>().bounds.Intersects(dr.GetComponent<CompositeCollider2D>().bounds))
				{
					Destroy(nextHallway);
					yield break;
				}
			}
			nextOrigin = nextHallway;
			usedHallway = true;
		}
		GameObject nextRoom = CreateRoom(rooms[Random.Range(0, rooms.Length)], false);
		AlignRooms(nextOrigin.transform, nextRoom.transform, roomSpacing);
		yield return StartCoroutine(waitFrames(waitingFrames));
		foreach (GameObject dr in dungeon) {
			if (nextRoom.GetComponent<CompositeCollider2D>().bounds.Intersects(dr.GetComponent<CompositeCollider2D>().bounds))
			{
				if (usedHallway) {
					Destroy(nextOrigin);
				}
				Destroy(nextRoom);
				yield break;
			}
		}
		if (usedHallway) {
			dungeon.Add(nextOrigin);
		}
		dungeon.Add(nextRoom);
		if (GetNumAvailable(origin.GetComponent<RoomInfo>()) > 0) 
		{
			StartCoroutine(CreateDungeon(origin, nextMainBranch, branchCap));
		}
		else if (continueDungeon) 
		{
			StartCoroutine(CreateDungeon(nextRoom, nextMainBranch, nextBranchCap));
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

	private void AlignRooms(Transform origin, Transform created, float spacing)
	{
		// Gets all necessary information about the origin room
		RoomInfo origin_data = origin.GetComponent<RoomInfo>();
		string direction_origin = origin_data.doorDirection[FindAvailableDoor(origin_data)];
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
}
