using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BucketController : MonoBehaviour
{
    
    public SpriteRenderer BucketMask;
    public AnimationCurve ScaleAndRotationMultiplier;
    public AnimationCurve FillAmountCurve;
    public AnimationCurve RotateMultiplier;
    public float bucketVolume;
    public float currentvolume;
    public float RotateSpeed = 2f;
     [SerializeField] public TMP_Text volumeNumber;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 textPosition = transform.position;
        volumeNumber.transform.position = textPosition;
    }
    

    
    // Update is called once per frame
    void Update()
    {
        Vector3 textPosition = transform.position;
        volumeNumber.transform.position = textPosition;
        if (Input.GetKeyUp(KeyCode.P)) {
            StartCoroutine(Rotate(80));
        }
        if (Input.GetKeyUp(KeyCode.W)) {
            StartCoroutine(Rotate(-90));
        
        }
        if (Input.GetKeyUp(KeyCode.D)) {
            
        
        }

    }
    
    public IEnumerator Rotate(float RotateAngle) {
    float t = 0;
    float LerpValue;
    float AngleValue;

    // Получить текущее значение FillAmount
    float currentFill = BucketMask.material.GetFloat("_FillAmount");

    while (t < RotateSpeed) {
        LerpValue = t / RotateSpeed;
        float fillValue = Mathf.Lerp(currentFill, RotateAngle, LerpValue);
        AngleValue = Mathf.Lerp(0.0f, RotateAngle, LerpValue);
        transform.eulerAngles = new Vector3(0, 0, -AngleValue);
        if (currentFill > FillAmountCurve.Evaluate(fillValue))
        {
            BucketMask.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(fillValue));
            BucketMask.material.SetFloat("_ScaleAndRotationProperty", ScaleAndRotationMultiplier.Evaluate(AngleValue));
        }
        
        

        t += Time.deltaTime * RotateMultiplier.Evaluate(AngleValue);
        yield return new WaitForEndOfFrame();
    }

    AngleValue = RotateAngle;
    // Используйте окончательное значение RotateAngle как окончательное значение FillAmount
    float finalFillValue = RotateAngle;
    transform.eulerAngles = new Vector3(0, 0, -AngleValue);
    BucketMask.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(finalFillValue));
    BucketMask.material.SetFloat("_ScaleAndRotationProperty", ScaleAndRotationMultiplier.Evaluate(AngleValue));

    StartCoroutine(BackRotate(AngleValue));
}
    IEnumerator BackRotate(float RotateAngle) {
        float t = 0;
        float LerpValue;
        float AngleValue;
        while(t<RotateSpeed) {
            LerpValue = t / RotateSpeed;
            AngleValue = Mathf.Lerp(RotateAngle, 0.0f, LerpValue);
            transform.eulerAngles = new Vector3(0,0, -AngleValue);
            BucketMask.material.SetFloat("_ScaleAndRotationProperty", ScaleAndRotationMultiplier.Evaluate(AngleValue));
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        AngleValue = 0;
        transform.eulerAngles = new Vector3(0,0, -AngleValue);
        BucketMask.material.SetFloat("_ScaleAndRotationProperty", 1);
    }
    public IEnumerator Fill(float endAmount)
    {
        float t =0 ;
        float startFillAmount = BucketMask.material.GetFloat("_FillAmount");
        float AngleValue;
    while (t < RotateSpeed)
    {
        
        float lerpValue = t / RotateSpeed;
        AngleValue = Mathf.Lerp(90, 0,  lerpValue);
        float targetFillAmount = Mathf.Lerp(startFillAmount, endAmount, lerpValue); // Изменено на 1, чтобы наполнить ведро
        BucketMask.material.SetFloat("_FillAmount", targetFillAmount);
        t += Time.deltaTime* RotateMultiplier.Evaluate(AngleValue);
        yield return new WaitForEndOfFrame();
    }

        BucketMask.material.SetFloat("_FillAmount", endAmount); // Убедитесь, что _FillAmount становится 1 после анимации
        
    }

}
