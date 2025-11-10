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

    public GameObject playButton;
    public TextMeshProUGUI curTimeText;
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if(!isPlaying)
        {
            return;
        }
        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        rig.linearVelocity = new Vector3(x, rig.angularVelocity.y, z);
        curTimeText.text = (Time.time - startTime).ToString("F2");
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
        playButton.SetActive(false);
    }

    // End gets called when timer has ended
    void End ()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
        playButton.SetActive(true);
        Leaderboard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
    }
}
