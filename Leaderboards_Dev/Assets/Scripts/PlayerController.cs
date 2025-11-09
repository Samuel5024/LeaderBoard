using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody rig;

    private float startTime;
    private float timeTaken;

    private int collectablesPicked;
    public int maxCollectables = 10;

    private bool isPlaying;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        rig.linearVelocity = new Vector3(x, rig.angularVelocity.y, z);
    }

    // called when we collide with an object
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            // add one ot the collectablesPicked then destroy collectable
            collectablesPicked++;
            Destroy(other.gameObject);

            if(collectablesPicked == maxCollectables)
            {
                End();
            }
        }
    }

    // Begin starts the timer
    public void Begin ()
    {
        startTime = Time.time;
        isPlaying = true;
    }

    // End gets called when timer has ended
    void End ()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
    }
}
