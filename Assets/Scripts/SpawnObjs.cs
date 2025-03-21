using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjs : MonoBehaviour
{
    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private GameObject enemyPrefab, coinPrefab, goalPrefab;

    [SerializeField] private int basicBlock = 5;// sl block co ban start
    [SerializeField] private int initialBlock = 10;// sl block khoi tao start
    [SerializeField] private int maxBlock = 20;
    [SerializeField] private int trapBlockNum = 1;
    [SerializeField] private int spikeBlockNum = 2;
    [SerializeField] private int enemyNum = 3, coinNum = 4;

    [SerializeField] private float width = 3f;
    [SerializeField] private float distanceY = 3f;
    [SerializeField] private float speed = 2f;

    private List<Transform> blocks = new List<Transform>();
    private bool[] isTrapBlock, isSpikeBlock, isEnemyBlock, isCoinBlock;
    private int blockNum;

    private Player16 player => FindObjectOfType<Player16>();

    [SerializeField] private LevelData[] levelDatas;

    void Start()
    {
        int levelCurrent = PlayerPrefs.GetInt("CurrentLevel", 1);
        basicBlock = levelDatas[levelCurrent - 1].basicBlock;
        maxBlock = levelDatas[levelCurrent - 1].maxBlock;
        trapBlockNum = levelDatas[levelCurrent - 1].trapBlockNum;
        spikeBlockNum = levelDatas[levelCurrent - 1].spikeBlockNum;
        enemyNum = levelDatas[levelCurrent - 1].enemyNum;
        coinNum = levelDatas[levelCurrent - 1].coinNum;

        isTrapBlock = new bool[maxBlock];
        isSpikeBlock = new bool[maxBlock];
        isEnemyBlock = new bool[maxBlock];
        isCoinBlock = new bool[maxBlock];
        SetupBlocks();

        float startY = transform.position.y;
        for (int i = 0; i < initialBlock; i++)
        {
            int blockType = (i < basicBlock) ? Random.Range(0, 2) : GetBlockType(i);
            SpawnBlocks(startY, blockType, i);
            startY -= distanceY;
        }

        blockNum = initialBlock;
    }

    void Update()
    {
        if (blockNum == maxBlock && blocks[blocks.Count - 1].position.y > 0f || (player && player.done))
            return;

        for (int i = 0; i < blocks.Count; i++)
            blocks[i].position += Vector3.up * speed * Time.deltaTime;

        if (blockNum < maxBlock && !IsOnScreen(blocks[0].position))
        {
            Destroy(blocks[0].gameObject);
            blocks.RemoveAt(0);// remove b[0]

            float lastY = blocks[blocks.Count - 1].position.y;
            bool isGoal = (blockNum == maxBlock - 1);

            SpawnBlocks(lastY - distanceY, GetNewBlockType(isGoal), blockNum);
            blockNum++;
        }
    }

    private void SpawnBlocks(float y, int blockType, int blockIndex)
    {
        Vector2 spawnPos = new Vector2(Random.Range(-width, width), y);
        Transform block = Instantiate(blockPrefabs[blockType], spawnPos, Quaternion.identity, transform).transform;
        blocks.Add(block);

        if (blockIndex == maxBlock - 1)
            Instantiate(goalPrefab, block.position + Vector3.up * 0.75f, Quaternion.identity, block);
        else if (blockIndex >= basicBlock)
        {
            if (isEnemyBlock[blockIndex])
                Instantiate(enemyPrefab, block.position + Vector3.up * /*0.53f*/ 0.48f, Quaternion.identity, block);
            else if (isCoinBlock[blockIndex])
                Instantiate(coinPrefab, block.position + Vector3.up * 0.75f, Quaternion.identity, block);
        }
    }

    private void SetupBlocks()
    {
        List<int> availableBlocks = new List<int>();

        for (int i = basicBlock; i < maxBlock - 1; i++)
            availableBlocks.Add(i);

        AssignRandomBlocks(trapBlockNum, availableBlocks, isTrapBlock);
        AssignRandomBlocks(spikeBlockNum, availableBlocks, isSpikeBlock);
        AssignRandomBlocks(enemyNum, availableBlocks, isEnemyBlock);
        AssignRandomBlocks(coinNum, availableBlocks, isCoinBlock);
    }

    private void AssignRandomBlocks(int length, List<int> list, bool[] isBool)
    {
        for (int i = 0; i < length; i++)
        {
            int index = Random.Range(0, list.Count);
            isBool[list[index]] = true;
            list.RemoveAt(index);
        }
    }

    private int GetBlockType(int blockIndex)
    {
        if (isSpikeBlock[blockIndex]) return 2;
        else if (isTrapBlock[blockIndex]) return 3;
        else return Random.Range(0, 2);
    }

    private int GetNewBlockType(bool isGoal)
    {
        if (isGoal) return 1;
        else return GetBlockType(blockNum);
    }

    private bool IsOnScreen(Vector3 pos)
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(pos);
        return screenPos.y <= 1;
    }
}