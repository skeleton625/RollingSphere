using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBlockController : MonoBehaviour
{
    [SerializeField]
    private int BlockCount;

    private float DeActiveTime = 2f;
    private Vector3[] DefaultPos;
    private Transform[] Blocks;

    private void Awake()
    {
        InitiateSplitBlockPos();
    }

    private void OnEnable()
    {
        StartCoroutine(SetActiveSplitBlock());
    }

    private IEnumerator SetActiveSplitBlock()
    {
        yield return new WaitForSeconds(DeActiveTime);
        ReInitiateBlockPos();
        gameObject.SetActive(false);
    }

    private void InitiateSplitBlockPos()
    {
        Blocks = new Transform[BlockCount];
        DefaultPos = new Vector3[BlockCount];

        for (int i = 0; i < BlockCount; i++)
        {
            Blocks[i] = transform.GetChild(i);
            DefaultPos[i] = Blocks[i].transform.localPosition;
        }
    }

    private void ReInitiateBlockPos()
    {
        for(int i = 0; i < BlockCount; i++)
            Blocks[i].localPosition = DefaultPos[i];
    }
}
