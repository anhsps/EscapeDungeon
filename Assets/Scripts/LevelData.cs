using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    public int basicBlock;
    public int maxBlock;
    public int trapBlockNum;
    public int spikeBlockNum;
    public int enemyNum;
    public int coinNum;
}
