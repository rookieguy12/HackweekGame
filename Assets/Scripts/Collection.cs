using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    public Text count;
    public void Delete()
    {
        count.text = Convert.ToString(int.Parse(count.text) + 1);
        Destroy(this.gameObject);
    }
}
