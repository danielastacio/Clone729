using System.Collections;
using UnityEngine;

public class GameData
{
    public long lastUpdated;
    public float playerCurrentHp;
    public Vector3 playerSpawnPoint;

    public GameData ()
    {
        playerCurrentHp = 100;
        playerSpawnPoint = Vector3.zero;
    }
}