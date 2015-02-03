using UnityEngine;
using System.Collections;

public class RoomGenerator : MonoBehaviour
{
    public Transform wall;
    public Transform player;
    public Transform blockedSpace;
	public Transform door;
    public bool createWall;

    void Start()
    {
        createRoom(0, 0);
    }

    void createRoom(int startX, int startY)
    {

		if (!createWall)
		{
			return;
		}
        string[] lines = System.IO.File.ReadAllLines(@"Assets\Maps\Map1.txt");
        Vector3 wallSize = wall.renderer.bounds.size;
        Vector3 playerSize = wall.renderer.bounds.size;
		Vector3 doorSize = wall.renderer.bounds.size;
		Vector3 blockedSpaceSize = wall.renderer.bounds.size;

        //GameObject player = null;
        //GameObject enemy = null;

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                char currentChar = lines[i][j];

                if (currentChar == 'w')
                {
                    Instantiate(wall, new Vector3(startX + (lines[i].Length - j - 1) * wallSize.x, 0, startY + (lines.Length - i - 1) * wallSize.y), Quaternion.identity);
                }
                else if (currentChar == 'p')
                {
                    Transform playerObj = (Transform)Instantiate(player, new Vector3(startX + (lines[i].Length - j - 1) * playerSize.x, 0, startY + (lines.Length - i - 1) * playerSize.y), Quaternion.identity);
                    playerObj.tag = "Player";
                }
				else if (currentChar == 'd')
				{
					Transform playerObj = (Transform)Instantiate(door, new Vector3(startX + (lines[i].Length - j - 1) * doorSize.x, 0, startY + (lines.Length - i - 1) * doorSize.y), Quaternion.identity);
									}
                else if (currentChar == 'r')
                {
					Instantiate(blockedSpace, new Vector3(startX + (lines[i].Length - j - 1) * blockedSpaceSize.x, 0, startY + (lines.Length - i - 1) * blockedSpaceSize.y), Quaternion.identity);
                }
            }
        }
    }

}