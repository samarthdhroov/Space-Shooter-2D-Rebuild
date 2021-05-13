using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ast_Explosion : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, 3);
    }
}
