using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string Name;

    public virtual bool UseItem()
    {
        return true;
    }
}
