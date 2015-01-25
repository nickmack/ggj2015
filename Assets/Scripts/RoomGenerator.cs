﻿using UnityEngine;
using System.Collections;

public class RoomGenerator : MonoBehaviour
{
    public Transform wall;
    public Transform player;
    public Transform enemy;

    void Start()
    {
        createRoom(0, 0);
    }

    void createRoom(int startX, int startY)
    {
        string[] lines = System.IO.File.ReadAllLines(@"Assets\Maps\Map1.txt");
        Vector3 wallSize = wall.renderer.bounds.size;
        Vector3 playerSize = wall.renderer.bounds.size;
        Vector3 enemySize = wall.renderer.bounds.size;

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
                else if (currentChar == 'm')
                {
                    Instantiate(enemy, new Vector3(startX + (lines[i].Length - j - 1) * enemySize.x, 0, startY + (lines.Length - i - 1) * enemySize.y), Quaternion.identity);
                }
            }
        }
    }

}