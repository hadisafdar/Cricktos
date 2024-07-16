using UnityEngine;

public class SpawnPointProvider : GenericSingleton<SpawnPointProvider>
{
    [SerializeField] private Transform batsmanSpawnPoint;
    [SerializeField] private Transform bowlerSpawnPoint;
    [SerializeField] private Transform bowlerRunEndPoint;
    [SerializeField] private Transform ballHitPoint;

    public Transform BowlerRunEndPoint => bowlerRunEndPoint;
    public Transform BallHitPoint => ballHitPoint;

    public Transform GetSpawnPoint(PlayerRole role)
    {
        return role == PlayerRole.Batsman ? batsmanSpawnPoint : bowlerSpawnPoint;
    }
}
