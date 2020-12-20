using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShrinkingPlatformController : MonoBehaviour
{
    public bool isActive;
    public float platformTimer;
    public float threshold;
    Transform goTransform;
    public PlayerBehaviour player;
    public AudioClip ShrinkingSfx;  //Shrinking sfx
    public AudioClip UnshrinkingSfx;    //Unshrinking sfx
    public AudioSource sfx; //To play audio sfx
    bool isFloating = false;    //Bool to check if the platform is floating
    bool isShrinking = false;   //Bool to check if the platform is already shrinking

    private Vector3 distance;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>();

        platformTimer = 0.1f;
        platformTimer = 0;
        isActive = false;
        //Starting the floating effect after this it is in a continuous loop
       
    }

    void Update()
    {
        if (!isFloating)
        {
            StartCoroutine(floaty(2.0f, transform.position, transform.position.y + 1.25f));
        }
        if (isActive)
        {   
            
            //When the player is touching the latform it will start the coroutine that shrinks the platform to make the player fall off
            StartCoroutine(Shrinker(2.0f, 1.0f, 0.0f));
            isShrinking = false;
            
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
                //Lerps the position so that the animation isn't instant it gradually moves
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

    IEnumerator Shrinker(float period, float startScaleX, float endScaleX)
    {
        if (!isShrinking)
        {
            isShrinking = true;
            var time = 0.0f;
            //Assigning the correct sound effect
            sfx.clip = ShrinkingSfx; 
            //Playing the sound effect clip 
            sfx.Play();
            while (time < 1.0f)
            {
                //Lerping the x scale of the platform so that it appears to be shrinking
                transform.localScale = Vector3.Lerp(new Vector3(startScaleX, 1, 1), new Vector3(endScaleX, 1, 1), time);
                time = time + (Time.deltaTime / period);
                yield return null;
            }
            sfx.clip = UnshrinkingSfx;
            Debug.Log("playing sound");
            sfx.Play();
            time = 0.0f;
            while (time < 1.0f)
            {
                transform.localScale = Vector3.Lerp(new Vector3(endScaleX, 1, 1), new Vector3(startScaleX, 1, 1), time);
                time = time + (Time.deltaTime / period);
                yield return null;
            }
            sfx.Stop();
            yield return null;
            
        }
    }
}
