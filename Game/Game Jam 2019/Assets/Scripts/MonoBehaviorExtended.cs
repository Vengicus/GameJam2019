using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviorExtended : MonoBehaviour
{
    private Rigidbody2D _rigBody;
    public Rigidbody2D RigBody
    {
        get
        {
            if (_rigBody == null)
            {
                _rigBody = gameObject.GetComponent<Rigidbody2D>();
            }
            return _rigBody;
        }

    }

    protected void MoveObjectToNewPosition(Vector2 newPosition)
    {
        Vector2 position = RigBody.position;
        RigBody.MovePosition(position + newPosition * Time.fixedDeltaTime);
    }
    

}
