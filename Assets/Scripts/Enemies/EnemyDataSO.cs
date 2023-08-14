using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Data/EnemyData")]

public class EnemyDataSO : ScriptableObject {
    [SerializeField] public float Health { get; private set; } = 100;
    [SerializeField] public float Defense { get; private set; } = 10;
    [SerializeField] public float Attack { get; private set; } = 10;
    [SerializeField] public string Name { get; private set; } = "Unnamed";

}
