using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvesTester : MonoBehaviour
{
    [SerializeField] 
    private AnimationCurve curve;
    private float curveTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestCurve()
    {
        Debug.Log(Mathf.Round(curve.Evaluate(curveTime) * 0.3f * 1000.0f) / 1000.0f);
        curveTime += 0.02f;
    }
}
