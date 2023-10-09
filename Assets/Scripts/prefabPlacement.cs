using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class prefabPlacement : MonoBehaviour
{
    [SerializeField] private GameObject[] rooms;
	[SerializeField] private GameObject[] hallways;
	[SerializeField] private GameObject spawnRoom;
	[SerializeField] private float roomSpacing;



	void Start (){
		float randomDir = Random.Range(0, 4);
		GameObject startRoom = Instantiate(spawnRoom, new Vector2(0, 0), Quaternion.Euler(0, 0, randomDir * 90));
		startRoom.transform.parent = transform;

		GameObject nextRoom = Instantiate(rooms[0], new Vector2(0, 0), Quaternion.Euler(0, 0, 0));
		nextRoom.transform.parent = transform;

		alignRooms(startRoom.transform, randomDir, nextRoom.transform);
	}

	private void alignRooms(Transform origin, float dir, Transform created) {
		string direction;
		string[] directions = {"South", "East", "North", "West"};
		Transform origin_join_point = origin.Find("Join Point").Find("Join Point");
		direction = directions[(int) dir];

		Transform created_join_point = created.Find("Join Point " + direction).Find("Join Point");

		Vector2 shift = new Vector2();
		if (direction == "North" || direction == "South") {
			shift = new Vector2(origin_join_point.position.x - created_join_point.position.x, origin_join_point.position.y + ((direction == "North" ? -roomSpacing : roomSpacing)) - created_join_point.position.y);
		} else {
			shift = new Vector2(origin_join_point.position.x + (direction == "West" ? roomSpacing : -roomSpacing) - created_join_point.position.x, origin_join_point.position.y - created_join_point.position.y);
		}
		created.Translate(shift);
	}
}
