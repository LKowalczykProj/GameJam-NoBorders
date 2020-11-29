using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionControl : MonoBehaviour
{

    public Rigidbody2D myRB;

    public void SetKinematic()
    {
        myRB.isKinematic = true;
    }
    public void SetDynamic()
    {
        myRB.isKinematic = false;
    }

}
