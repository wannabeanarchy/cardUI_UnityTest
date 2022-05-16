using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoroutineAnimations : MonoBehaviour 
{ 
     
    public void SetPositionCoroutine(Vector3 target, EasingFunction.Ease easing, float speed, float delay = 0, Action callback = null)
    {
        StartCoroutine(Move(target, easing, speed, delay, callback));
    }

    public void SetScaleCoroutine(Vector3 scale, EasingFunction.Ease easing, float speed, float delay = 0, Action callback = null)
    {
        StartCoroutine(Scale(scale, easing, speed, delay, callback));
    }

    public void SetRotationCoroutine(Vector3 target, EasingFunction.Ease easing, float speed, float delay = 0, Action callback = null)
    { 
        StartCoroutine(Rotation(target, easing, speed, delay, callback));
    }

    public void SetCountText(TextMeshPro goTarget, int newValue, int oldValue, float duration, float delay = 0, Action callback = null)
    {
        StartCoroutine(CountText(goTarget, newValue, oldValue, duration, delay, callback));
    }

    private IEnumerator Move(Vector3 target, EasingFunction.Ease easing, float speed, float delay = 0, Action callback = null)
    {
        yield return new WaitForSeconds(delay);

        float time = 0.0f;

        var tr = this.transform;
        var p = tr.position;

        var e = EasingFunction.easings[easing];

        Vector3 start = this.transform.position;
        Vector3 end = target; 

        while (time < speed)
        {
            float t = e(0.0f, 1.0f, time / speed);
            p.x = Mathf.Lerp(start.x, end.x, t);
            p.y = Mathf.Lerp(start.y, end.y, t);
            p.z = Mathf.Lerp(start.z, end.z, t);
            tr.position = p;

            yield return null;
            time += Time.deltaTime;
        }
 
        tr.position = p;

        if (callback != null)
            callback?.Invoke();

        yield return null;
    }
     
    private IEnumerator Rotation( Vector3 rotation, EasingFunction.Ease easing, float speed, float delay = 0, Action callback = null)
    {
       yield return new WaitForSeconds(delay);

       float time = 0.0f;

        var tr = this.transform;
        var r = tr.localEulerAngles;

        var e = EasingFunction.easings[easing];

        Vector3 startRotation = new Vector3(r.x, r.y, ((r.z > 180) ? -(360 - r.z) : r.z));
        Vector3 progressRotation = startRotation;
        Vector3 endRotation = rotation;
         
        while (time < speed)
        {
            float t = e(0.0f, 1.0f, time / speed);
            progressRotation.x =  Mathf.Lerp(startRotation.x, rotation.x, t);
            progressRotation.y =  Mathf.Lerp(startRotation.y, rotation.y, t);
            progressRotation.z =  Mathf.Lerp(startRotation.z, rotation.z, t);

            tr.localEulerAngles = progressRotation;
           

                yield return null;
            time += Time.deltaTime;
        }

        tr.localEulerAngles = progressRotation;

        if (callback != null)
            callback?.Invoke();

        yield return null;
    }

    private IEnumerator Scale(Vector3 scale, EasingFunction.Ease easing, float speed, float delay = 0, Action callback = null)
    {
        yield return new WaitForSeconds(delay);

        float time = 0.0f;

        var tr = this.transform;
        var s = tr.localScale;

        var e = EasingFunction.easings[easing];

        Vector3 start = this.transform.localScale;
        Vector3 end = scale;

        while (time < speed)
        {
            float t = e(0.0f, 1.0f, time / speed);
            s.x = Mathf.Lerp(start.x, end.x, t);
            s.y = Mathf.Lerp(start.y, end.y, t);
            s.z = Mathf.Lerp(start.z, end.z, t);
            tr.localScale = s;

            yield return null;
            time += Time.deltaTime;
        }
 
        tr.localScale = end;

        if (callback != null)
            callback?.Invoke();


        yield return null;
    }

    private IEnumerator CountText(TextMeshPro goTarget, int newValue, int oldValue, float duration, float delay = 0, Action callback = null)
    {
        yield return new WaitForSeconds(delay);

        int previousValue = oldValue;
        int stepAmount;

        if (newValue - previousValue < 0)
        {
            stepAmount = Mathf.FloorToInt((newValue - previousValue) / duration);  
        }
        else
        {
            stepAmount = Mathf.CeilToInt((newValue - previousValue) / duration);  
        }

        if (previousValue < newValue)
        {
            while (previousValue < newValue)
            {
                previousValue += stepAmount;
                if (previousValue > newValue)
                {
                    previousValue = newValue;
                } 
                goTarget.text = previousValue.ToString();
                
                yield return new WaitForSeconds(1 / duration);
            }
        }
        else
        {
            while (previousValue > newValue)
            {
                previousValue += stepAmount; 
                if (previousValue < newValue)
                {
                    previousValue = newValue;
                }

                goTarget.text = previousValue.ToString();

                yield return new WaitForSeconds(1 / duration);
            }
        }

        goTarget.text = newValue.ToString();

        if (callback != null)
            callback?.Invoke();

    }
}
 