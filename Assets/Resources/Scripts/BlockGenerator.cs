using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] BreakBlocks;
    [SerializeField]
    private GameObject[] UnBreakBlocks;

    [SerializeField]
    private int BlockCount;
    [SerializeField]
    private int LimitBlockCount;
    [SerializeField]
    private float BreakableBlockTime;
    [SerializeField]
    private float UnBreakableBlockTime;

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
        StartCoroutine(GenerateBreakableBlock());
        StartCoroutine(GenerateUnBreakableBlock());
    }

    private void GenerateBlocks()
    {
        for(int i = 0; i < BlockCount;)
        {
            for(int j = 0; j < BreakBlocks.Length; j++)
            {
                Blocks[i] = Instantiate(BreakBlocks[j], Vector3.zero, Quaternion.identity);
                Blocks[i].SetActive(false);
                BlockQueue.Enqueue(Blocks[i]);
                i++;
            }
        }
    }

    private Vector3 RandomizeBlockPosition(int _hor, int _ver)
    {
        float _x = Random.Range(-_hor, _hor);
        float _z = Random.Range(-_ver, _ver);
        return new Vector3(_x, -3, _z);
    }

    private IEnumerator GenerateBreakableBlock()
    {
        WaitForSeconds _time = new WaitForSeconds(BreakableBlockTime);

        GenerateBlocks();

        while(true)
        {
            if(preBlockCount < LimitBlockCount)
            {
                GameObject _mod = BlockQueue.Dequeue();
                _mod.transform.position = RandomizeBlockPosition(36, 21);
                _mod.SetActive(true);
                ++preBlockCount;
            }

            yield return _time;
        }
    }

    private IEnumerator GenerateUnBreakableBlock()
    {
        WaitForSeconds _time = new WaitForSeconds(UnBreakableBlockTime);
        int _num = 0;
        Vector3 _pos;

        while (true)
        {
            while (UnBreakBlocks[_num].transform.position.y > -3f)
            {
                UnBreakBlocks[_num].transform.Translate(0, -0.05f, 0);
                yield return null;
            }
            _pos = UnBreakBlocks[_num].transform.position;
            _pos.y = -3f;
            UnBreakBlocks[_num].transform.position = _pos;

            UnBreakBlocks[_num].SetActive(false);
            yield return _time;

            UnBreakBlocks[_num].transform.position = RandomizeBlockPosition(35, 20);
            UnBreakBlocks[_num].SetActive(true);

            while (UnBreakBlocks[_num].transform.position.y < 1.5f)
            {
                UnBreakBlocks[_num].transform.Translate(0, 0.05f, 0);
                yield return null;
            }
            _pos = UnBreakBlocks[_num].transform.position;
            _pos.y = 1.5f;
            UnBreakBlocks[_num].transform.position = _pos;

            ++_num;
            if (_num.Equals(UnBreakBlocks.Length))
                _num = 0;
        }
    }

    public void ReGenerateBlock(GameObject _block, int _type)
    {
        _block.SetActive(false);
        switch(_type)
        {
            case 0:
                _block.transform.position = RandomizeBlockPosition(36, 21);
                break;
            case 1:
                _block.transform.position = RandomizeBlockPosition(35, 20);
                break;
        }
        _block.SetActive(true);
    }

    public void PushPreBlock(GameObject _block)
    {
        BlockQueue.Enqueue(_block);
        --preBlockCount;
    }
}
