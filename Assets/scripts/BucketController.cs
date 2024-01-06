using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketController : MonoBehaviour
{
    
    public SpriteRenderer BucketMask;
    public AnimationCurve ScaleAndRotationMultiplier;
    public AnimationCurve FillAmountCurve;
    public AnimationCurve RotateMultiplier;
    [SerializeField] float RotateSpeed = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P)) {
            StartCoroutine(Rotate());
        
        }
    }
    IEnumerator Rotate() {
        float t = 0;
        float LerpValue;
        float AngleValue;
        while(t<RotateSpeed) {
             LerpValue = t / RotateSpeed;
            AngleValue = Mathf.Lerp(0.0f, 90.0f, LerpValue);
             transform.eulerAngles = new Vector3(0,0, AngleValue);
             BucketMask.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(AngleValue));
             BucketMask.material.SetFloat("_ScaleAndRotationProperty", ScaleAndRotationMultiplier.Evaluate(AngleValue));
             t += Time.deltaTime*RotateMultiplier.Evaluate(AngleValue);
             yield return new WaitForEndOfFrame();
        }
        AngleValue = 90.0f;
        transform.eulerAngles = new Vector3(0,0, AngleValue);
        BucketMask.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(AngleValue));
        BucketMask.material.SetFloat("_ScaleAndRotationProperty", ScaleAndRotationMultiplier.Evaluate(AngleValue));
        StartCoroutine(BackRotate());
    }
    IEnumerator BackRotate() {
        float t = 0;
        float LerpValue;
        float AngleValue;
        while(t<RotateSpeed) {
             LerpValue = t / RotateSpeed;
            AngleValue = Mathf.Lerp(90.0f, 0.0f, LerpValue);
             transform.eulerAngles = new Vector3(0,0, AngleValue);
             BucketMask.material.SetFloat("_ScaleAndRotationProperty", ScaleAndRotationMultiplier.Evaluate(AngleValue));
             t += Time.deltaTime;
             yield return new WaitForEndOfFrame();
        }
        AngleValue = 0;
        transform.eulerAngles = new Vector3(0,0, AngleValue);
        BucketMask.material.SetFloat("_ScaleAndRotationProperty", ScaleAndRotationMultiplier.Evaluate(AngleValue));
    }
}
