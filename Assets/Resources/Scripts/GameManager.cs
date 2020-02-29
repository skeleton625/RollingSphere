using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] BreakBlocks;
    [SerializeField]
    private GameObject[] UnBreakBlocks;
    [SerializeField]
    private GameObject[] EnemyObjects;
    [SerializeField]
    private GameObject PlayerBall;

    [SerializeField]
    private int BlockCount;
    [SerializeField]
    private int LimitBlockCount;
    [SerializeField]
    private float BreakableBlockTime;
    [SerializeField]
    private float UnBreakableBlockTime;
    [SerializeField]
    private float EnemyMoveTime;

    private int preBlockCount;
    private bool isGameOver;
    public bool IsGameOver
    { set => isGameOver = value; }
    private bool IsGamePause;
    private Transform BTransform;
    private BallController BControl;

    private GameObject[] Blocks;
    private Queue<GameObject> BlockQueue;

    private UIManager UManager;
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        preBlockCount = 0;
        IsGamePause = false;

        Blocks = new GameObject[BlockCount];
        BlockQueue = new Queue<GameObject>();
        BTransform = PlayerBall.transform;
        BControl = PlayerBall.GetComponent<BallController>();
        StartCoroutine(GenerateBreakableBlock());
        StartCoroutine(GenerateUnBreakableBlock());
        StartCoroutine(MoveEnemyObjects());
        StartCoroutine(UpdateCoroutine());
    }

    private void Start()
    {
        UManager = UIManager.instance;
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

    private IEnumerator UpdateCoroutine()
    {
        while(true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                IsGamePause = !IsGamePause;
                UManager.SetActivePausePanel();
            }

            if (!IsGamePause)
                BControl.MoveBallByMouse();

            if (!PlayerBall.activeSelf)
            {
                StopAllCoroutines();
                break;
            }
            yield return null;
        }
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

    private IEnumerator MoveEnemyObjects()
    {
        WaitForSeconds _time = new WaitForSeconds(EnemyMoveTime);
        int _num = 0;
        NavMeshAgent[] _ai = new NavMeshAgent[EnemyObjects.Length];

        for (int i = 0; i < EnemyObjects.Length; i++)
            _ai[i] = EnemyObjects[i].GetComponent<NavMeshAgent>();

        while(true)
        {
            yield return _time;
            _ai[_num].SetDestination(BTransform.position);

            ++_num;
            if (_num.Equals(EnemyObjects.Length))
                _num = 0;
                
        }
    }

    public void ReGenerateBlock(GameObject _block)
    {
        _block.SetActive(false);
        Vector3 _pos = RandomizeBlockPosition(36, 21);
        _block.transform.localPosition = _pos;
        _block.SetActive(true);
    }

    public void PushPreBlock(GameObject _block)
    {
        BlockQueue.Enqueue(_block);
        --preBlockCount;
    }
}
