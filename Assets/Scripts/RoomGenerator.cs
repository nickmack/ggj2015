﻿using UnityEngine;
using System.Collections;

public class RoomGenerator : MonoBehaviour
{
    public Transform wall;

    void Start()
    {
        createRoom(0, 0, 8, 8);
    }

    void createRoom(int x, int y, int xAmount, int yAmount)
    {
        Vector3 size = wall.renderer.bounds.size;
        for (int j = 0; j < yAmount; j++)
        {
            for (int i = 0; i < xAmount; i++)
            {
                if (j == 0 || j == xAmount - 1)
                {
                    Instantiate(wall, new Vector2(x + i * size.x, y + j * size.y), Quaternion.identity);
                }
                else
                {
                    if (i == 0 || i < xAmount - 1)
                    {
                        Instantiate(wall, new Vector2(x + i * size.x, y + j * size.y), Quaternion.identity);
                    }
                }
            }
        }
    }

}