using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShrinkingPlatformController : MonoBehaviour
{
    //public Transform start;
    //public Transform end;
    public bool isActive;
    public float platformTimer;
    public float threshold;
    Transform goTransform;
    public PlayerBehaviour player;
    public AudioClip ShrinkingSfx;
    public AudioClip UnshrinkingSfx;
    public AudioSource sfx;
    bool isFloating = false;
    bool isShrinking = false;
    bool hasShrunk = false;

    private Vector3 distance;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>();

        platformTimer = 0.1f;
        platformTimer = 0;
        isActive = false;
    }

    void Update()
    {
        if (isFloating == false)
        {
            sfx.clip = ShrinkingSfx;
            sfx.Play();
            StartCoroutine(floaty(2.0f, transform.position, transform.position.y + 2.0f));
            //sfx.Stop();
        }   
            if (isActive)
            {

                sfx.clip = UnshrinkingSfx;
                sfx.Play();
                StartCoroutine(Shrinker(2.0f, 1.0f, 0.0f));
                //sfx.Stop();
                hasShrunk = true;
            }
    }

    void floating()
    {
        var time = 0.0f;
        Vector3 jeez = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        while (time < 1.0f)
        {                    
             if (time < 0.5f)
             {
                transform.position = Vector3.Lerp(transform.position,jeez , time);
                 
             }
             else if (time >= 0.5f)
             {
                Debug.Log("hi");
                transform.position = Vector3.Lerp(jeez, transform.position, time);
            } 
            time = time + (Time.deltaTime / 2.0f);
        }
        
    }

    IEnumerator floaty(float duration, Vector3 startPos, float endY)
    {
        if (!isFloating)
        {
            isFloating = true;
            var time = 0.0f;

            while (time < 1.0f)
            {
                transform.position = Vector3.Lerp(startPos, new Vector3(transform.position.x, endY, 0), time); //Mathf.Lerp(startPos, endPos, time);
                time = time + (Time.deltaTime / duration);
                yield return null;
            }
            time = 0.0f;
            while (time < 1.0f)
            {
                transform.position = Vector3.Lerp(new Vector3 (startPos.x, endY, startPos.z), new Vector3(transform.position.x, startPos.y, 0), time); //Mathf.Lerp(startPos, endPos, time);
                time = time + (Time.deltaTime / duration);
                yield return null;
            }
            isFloating = false;
            Debug.Log("I'm working");
        }  
        
    }

    IEnumerator Shrinker(float duration, float startScaleX, float endScaleX)
    {
        if (!isShrinking)
        {
            var time = 0.0f;

            while (time < 1.0f)
            {
                transform.localScale = Vector3.Lerp(new Vector3(startScaleX, 1, 1), new Vector3(endScaleX, 1, 1), time);
                time = time + (Time.deltaTime / duration);
                yield return null;
            }
            time = 0.0f;
            while (time < 1.0f)
            {
                transform.localScale = Vector3.Lerp(new Vector3(endScaleX, 1, 1), new Vector3(startScaleX, 1, 1), time);
                time = time + (Time.deltaTime / duration);
                yield return null;
            }
            //isShrinking = false;
            yield return null;
        }
    }
}
