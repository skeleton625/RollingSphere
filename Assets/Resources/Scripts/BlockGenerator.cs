using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] BlockModel;

    [SerializeField]
    private int BlockCount;
    [SerializeField]
    private int LimitBlockCount;
    [SerializeField]
    private float GenerateTime;

    public static BlockGenerator instance;
    private int preBlockCount;
    private GameObject[] Blocks;
    private Queue<GameObject> BlockQueue;

    private void Awake()
    {
        instance = this;

        preBlockCount = 0;
        Blocks = new GameObject[BlockCount];
        BlockQueue = new Queue<GameObject>();
        StartCoroutine(SetActiveBlock());
    }

    private void GenerateBlocks()
    {
        for(int i = 0; i < BlockCount;)
        {
            for(int j = 0; j < BlockModel.Length; j++)
            {
                Blocks[i] = Instantiate(BlockModel[j], Vector3.zero, Quaternion.identity);
                Blocks[i].SetActive(false);
                BlockQueue.Enqueue(Blocks[i]);
                i++;
            }
        }
    }

    private Vector3 RandomizeBlockPosition()
    {
        float _x = Random.Range(-36, 36);
        float _z = Random.Range(-21, 21);
        return new Vector3(_x, 2, _z);
    }

    private IEnumerator SetActiveBlock()
    {
        WaitForSeconds _time = new WaitForSeconds(GenerateTime);

        GenerateBlocks();

        while(true)
        {
            if(preBlockCount < LimitBlockCount)
            {
                GameObject _mod = BlockQueue.Dequeue();
                _mod.transform.position = RandomizeBlockPosition();
                _mod.SetActive(true);
                ++preBlockCount;
            }

            yield return _time;
        }
    }

    public void PushPreBlock(GameObject _block)
    {
        BlockQueue.Enqueue(_block);
        --preBlockCount;
    }
}
