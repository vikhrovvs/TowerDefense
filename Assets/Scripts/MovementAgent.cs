using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAgent : MonoBehaviour
{
    //[NonSerializeField] 
    public int a; //автоматически SerializeField
    [SerializeField]
    private int A; //Получили 2 одинаковых имени в Unity :)

    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private Vector3 m_Target;

    private const float TOLERANCE = 0.1f;
    
    /*
    // Start is called before the first frame update
    void Start()
    {
        m_Speed = 5f; //units per second
        m_Target = new Vector3(10f, 0f, 10f);
    }
    */
    
    // Update is called once per frame
    void Update()
    {
        float distance = (m_Target - transform.position).magnitude;
        if (distance < TOLERANCE)
        {
            return; //при очень маленьком fps может никогда не дойти, но на практике это невозможно
        }

        Vector3 dir = (m_Target - transform.position).normalized; //свойство Vector3
        Vector3 delta = dir * (m_Speed * Time.deltaTime);
        transform.Translate(delta);
    }
}
