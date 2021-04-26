using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool hasKey;
    bool haveSkill;
    public GameObject keySprite;
    public GameObject swordSkill;
    // Start is called before the first frame update
    void Start()
    {
        hasKey = false;
        haveSkill = false;
        keySprite.SetActive(false);
        swordSkill.SetActive(false);
        Debug.Log("Inventory started properly");
    }

    // Update is called once per frame
    public void playerGetKey()
    {
        hasKey = true;
        keySprite.SetActive(true);
    }

    public void playerGetSkill()
    {
        haveSkill = true;
        swordSkill.SetActive(true);
    }
}
