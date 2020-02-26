using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    [SerializeField]
    private GameObject SplitBlockModel;
    [SerializeField]
    private Animator BlockAnim;
    private GameObject PreSplitBlock;

    private UIManager UManager;
    private BlockGenerator BG;

    private void OnEnable()
    {
        BlockAnim.Play("MoveUp", 0, 0.25f);
    }

    private void Start()
    {
        BG = BlockGenerator.instance;
        UManager = UIManager.instance;
        BlockAnim = GetComponent<Animator>();
        PreSplitBlock = Instantiate(SplitBlockModel, Vector3.zero, Quaternion.identity);
        PreSplitBlock.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PreSplitBlock.transform.position = transform.position;
            PreSplitBlock.SetActive(true);
            BG.PushPreBlock(gameObject);
            UManager.PlusScore();
            gameObject.SetActive(false);
        }
    }
}
