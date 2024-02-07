using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
public class GameManager : MonoBehaviour
{
    bool coroutineInProgress = false;
    public BucketController bucket1;
    public BucketController bucket2;
    public float RotateSpeed = 2f;
    public bool winChek = false;
    
    private bool isFirstClick = true; // Переменная для отслеживания первого нажатия
    private BucketController selectedBucket; // Выбранное ведро после первого нажатия
    public float amount1 = 0;
    public float amount2 =0;
    public float amountToMeasure =0;
    void Start()
    {
        // Создаем два ведра с заданным объемом

        winChek = false;
        int scenario = UnityEngine.Random.Range(1, 11);
        bucket1.volumeNumber.text = amount1.ToString();
        bucket2.volumeNumber.text = amount2.ToString();
        // Запускаем соответствующий сценарий
        switch (scenario)
        {
             case 1:
                amount1 = 5;
                amount2 = 3;
                amountToMeasure = 4;
                break;
            case 2:
                amount1 = 5;
                amount2 = 4;
                amountToMeasure = 3;
                break;
            case 3:
                amount1 = 7;
                amount2 = 5;
                amountToMeasure = 4;
                break;
            case 4:
                amount1 = 7;
                amount2 = 4;
                amountToMeasure = 5;
                break;
            case 5:
                amount1 = 8;
                amount2 = 5;
                amountToMeasure = 7;
                break;
            case 6:
                amount1 = 7;
                amount2 = 6;
                amountToMeasure = 3;
                break;
            case 7:
                amount1 = 6;
                amount2 = 5;
                amountToMeasure = 3;
                break;
            case 8:
                amount1 = 4;
                amount2 = 3;
                amountToMeasure = 2;
                break;
            case 9:
                amount1 = 5;
                amount2 = 4;
                amountToMeasure = 2;
                break;
            case 10:
                amount1 = 7;
                amount2 = 5;
                amountToMeasure = 3;
                break;
        }
        bucket1.bucketVolume = amount1;
        bucket2.bucketVolume = amount2;
        bucket1.currentvolume = bucket1.BucketMask.material.GetFloat("_FillAmount")*amount1;
        bucket2.currentvolume = bucket2.BucketMask.material.GetFloat("_FillAmount")*amount2;
    }

public void FillSelectedBucket()
{
    if (selectedBucket == bucket1)
    {
        StartCoroutine(bucket1.Fill(1));
    }
    else if (selectedBucket == bucket2)
    {
        StartCoroutine(bucket2.Fill(1));
    }
}



void Update()
{
    AmountCheck();
   if (!coroutineInProgress && Input.GetMouseButtonDown(0))
    {
        StartCoroutine(HandleBucketInteraction());
    }
    

}
public void AmountCheck()   {  
    if (bucket1.currentvolume == amountToMeasure || bucket1.currentvolume == amountToMeasure) {
        Debug.Log("True");
        winChek =true;
    }
}
IEnumerator ChangeNumber(BucketController bucket, float initialV, float finalV) {
    float t = 0;
    float RotateValue;
    while (t < RotateSpeed) {
       
        float lerpValue = t / RotateSpeed;
        RotateValue = Mathf.Lerp(90, 0, lerpValue);
        float fillValue = Mathf.Lerp(initialV, finalV, lerpValue);
        bucket.volumeNumber.text = fillValue.ToString("F2");
        
        // Ждем следующего кадра перед продолжением цикла
        yield return null;

        t += Time.deltaTime*bucket.RotateMultiplier.Evaluate(RotateValue);
    }
    bucket.currentvolume = finalV;
    // Убеждаемся, что мы установили окончательное значение
    bucket.volumeNumber.text = finalV.ToString("F2");
}

    IEnumerator HandleBucketInteraction()
    {
        coroutineInProgress = true;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        
        // Проверяем, попал ли курсор на кнопку паузы
        if (hit.collider != null && hit.collider.CompareTag("PauseButton"))
        {
            // Если курсор попал на кнопку паузы, ничего не делаем
            coroutineInProgress = false;
            yield break;
        }

        // Проверяем, было ли произведено нажатие на объект
        if (hit.collider != null)
        {
            BucketController hitBucket = hit.collider.GetComponent<BucketController>();

            if (isFirstClick)
            {
                // Если это первое нажатие, выбираем ведро
                selectedBucket = hitBucket;

                if (selectedBucket != null)
                {
                    // Поднимаем выбранное ведро
                    coroutineInProgress = true;
                    yield return StartCoroutine(RaiseBucketSmoothly(selectedBucket, 0.1f));
                    isFirstClick = false;
                }
            }
            else
            {
                if (selectedBucket != null && selectedBucket == hitBucket)
                {
                    // Если второе нажатие на том же ведре, наполняем его и опускаем
                    if (selectedBucket == bucket1 || selectedBucket == bucket2)
                    {
                        StartCoroutine(ChangeNumber(selectedBucket, selectedBucket.currentvolume, selectedBucket.bucketVolume));
                        yield return StartCoroutine(selectedBucket.Fill(1));
                        yield return StartCoroutine(RaiseBucketSmoothly(selectedBucket, -0.1f));
                        isFirstClick = true;
                        
                    }
                }
                else if (selectedBucket != null && hitBucket != null && hitBucket.GetComponent<BucketController>() != null &&
                    selectedBucket.BucketMask.material.GetFloat("_FillAmount") != 0.00f && hitBucket.BucketMask.material.GetFloat("_FillAmount") != 1f)
                {
                    // Если второе нажатие на другом ведре, переливаем жидкость
                    float hitBucketFinalVolume = 0;
                    float selectedFinalVolume = 0;

                    if (selectedBucket.currentvolume + hitBucket.currentvolume > hitBucket.bucketVolume)
                    {
                        hitBucketFinalVolume = hitBucket.bucketVolume;
                        selectedFinalVolume = selectedBucket.currentvolume - (hitBucket.bucketVolume - hitBucket.currentvolume);
                    }
                    else if (selectedBucket.currentvolume + hitBucket.currentvolume <= hitBucket.bucketVolume)
                    {
                        hitBucketFinalVolume = hitBucket.currentvolume + selectedBucket.currentvolume;
                        selectedFinalVolume = 0;
                    }

                    yield return StartCoroutine(MoveBucketToSelectedAndReturn(selectedBucket, hitBucket, selectedFinalVolume, hitBucketFinalVolume));
                    isFirstClick = true;
                }
            }
        }
        else
        {
            // Если нажатие было на пустое место, опускаем ведро, если оно не пустое
            if (!isFirstClick && selectedBucket != null && selectedBucket.BucketMask.material.GetFloat("_FillAmount") > 0)
            {
                StartCoroutine(ChangeNumber(selectedBucket, selectedBucket.currentvolume, 0));
                yield return StartCoroutine(unfill(selectedBucket));
                isFirstClick = true;
            }
        }

        coroutineInProgress = false;
    }


IEnumerator MoveBucketToSelected(BucketController sourceBucket, BucketController targetBucket)
{
    float speed = 2f;
    float offsetX=0;
    float offsetY=0;
    if (sourceBucket==bucket1)
    {
        offsetX = sourceBucket.GetComponent<SpriteRenderer>().bounds.size.x/-2f; // половина ширины ведра
        offsetY = 0.7f;
    }
    else {
        offsetX = sourceBucket.GetComponent<SpriteRenderer>().bounds.size.x/2f; // половина ширины ведра
        offsetY = 0.9f;
    }

    Vector2 targetPosition = new Vector2(targetBucket.transform.position.x - offsetX, targetBucket.transform.position.y + offsetY);

    while (Vector2.Distance(sourceBucket.transform.position, targetPosition) > 0.2f)
    {
        sourceBucket.transform.position = Vector2.MoveTowards(sourceBucket.transform.position, targetPosition, Time.deltaTime * speed);
        yield return null;
    }
}


IEnumerator ReturnBucketToOriginalPosition(BucketController bucket, Vector2 originalPosition)
{
    float returnSpeed = 2f;

    while (Vector2.Distance(bucket.transform.position, originalPosition) > 0.1f)
    {
        bucket.transform.position = Vector2.MoveTowards(bucket.transform.position, originalPosition, Time.deltaTime * returnSpeed);
        yield return null;
    }
}

IEnumerator MoveBucketToSelectedAndReturn(BucketController sourceBucket, BucketController targetBucket, float sourceBucketFV, float targetBucketFV)
{
    yield return StartCoroutine(RaiseBucketSmoothly(sourceBucket, -0.1f));
    Vector2 originalPosition = sourceBucket.transform.position; // сохраняем начальную позицию
    float originalHeight = sourceBucket.transform.position.y; // сохраняем исходную высоту

    

    yield return StartCoroutine(MoveBucketToSelected(sourceBucket, targetBucket)); // вызываем первую корутину
    if (sourceBucket == bucket1)
    {
        sourceBucket.StartCoroutine(sourceBucket.Rotate(-90*(sourceBucket.bucketVolume-sourceBucketFV)/sourceBucket.bucketVolume));
        
        StartCoroutine(ChangeNumber(sourceBucket, sourceBucket.currentvolume, sourceBucketFV));
    }
    else{
        sourceBucket.StartCoroutine(sourceBucket.Rotate(90*(sourceBucket.bucketVolume-sourceBucketFV)/sourceBucket.bucketVolume));
        StartCoroutine(ChangeNumber(sourceBucket, sourceBucket.currentvolume, sourceBucketFV));
        
    }
    StartCoroutine(ChangeNumber(targetBucket, targetBucket.currentvolume, targetBucketFV));
    yield return targetBucket.StartCoroutine(targetBucket.Fill(targetBucketFV/targetBucket.bucketVolume));
    yield return StartCoroutine(ReturnBucketToOriginalPosition(sourceBucket, originalPosition)); // вызываем вторую корутину

    // Возвращаем ведро на исходную высоту после выполнения второй корутины
    sourceBucket.transform.position = new Vector2(originalPosition.x, originalHeight);
    sourceBucket.currentvolume = sourceBucketFV;
    targetBucket.currentvolume = targetBucketFV;
}

IEnumerator unfill(BucketController sourceBucket) {
    
    yield return StartCoroutine(RaiseBucketSmoothly(sourceBucket, -0.1f));
    if (sourceBucket == bucket1) {
        sourceBucket.StartCoroutine(sourceBucket.Rotate(-90));
    } else {
        sourceBucket.StartCoroutine(sourceBucket.Rotate(90));
    }
    
}

    IEnumerator RaiseBucketSmoothly(BucketController currentBucket, float height)
    {
    float targetY = currentBucket.transform.position.y + height;
    float elapsedTime = 0;
    Vector3 initialPosition = currentBucket.transform.position;

    while (elapsedTime < 0.5f) 
    {
        float newY = Mathf.Lerp(initialPosition.y, targetY, elapsedTime / 0.5f); // 2.0f - примерное время поднятия в секундах
        currentBucket.transform.position = new Vector3(currentBucket.transform.position.x, newY, currentBucket.transform.position.z);

        elapsedTime += Time.deltaTime;
        yield return null;
    }
    }
}