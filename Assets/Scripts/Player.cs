using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public string playerType;
    public float movementSpeed;
    public bool hasBall;
    private bool winFlag;
    public float lookSpeed;
    public float castDistance;
    public GameObject Marker;
    public GameObject Ball;
    private float score;
    public List<AudioSource> audioSources;
    RaycastHit2D hit;
    private Vector3 horizontalMovement;
    public GameObject winLoseUI;
    // Start is called before the first frame update
    void Start()
    {
        score= 0;
        winFlag= false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerType == "Passer")
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("x key was pressed");
                if(hasBall)
                    checkIfPassed();
            }
        }

        if (playerType == "Shooter")
        {
            //shoot ball
            if(Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("s key was pressed");
                if(hasBall)
                    checkIfScored();
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

    }

    private void FixedUpdate()
    {
        if (hasBall)
        {

            if (playerType.Equals("Passer"))
            {
                Debug.Log("has ball");
                transform.position = new Vector2(transform.position.x, transform.position.y + movementSpeed * Time.deltaTime);
                //horizontalMovement = new Vector3(0f, 0f, 10);
                horizontalMovement = new Vector3(0f, 0f, -Input.GetAxis("Horizontal"));
                Marker.transform.Rotate(horizontalMovement * Time.deltaTime * lookSpeed);
                //transform.Rotate(horizontalMovement * Time.deltaTime * lookSpeed);
                //Physics2D.Raycast(Marker.transform.position, Marker.transform.TransformDirection(Vector2.up), castDistance);
                Debug.DrawRay(Marker.transform.position, Marker.transform.TransformDirection(Vector2.up) * castDistance, Color.white);
            }
            else
            {
                horizontalMovement = new Vector3(0f, 0f, -Input.GetAxis("Horizontal"));
                //transform.Rotate(horizontalMovement * Time.deltaTime * lookSpeed);
                Marker.transform.Rotate(horizontalMovement * Time.deltaTime * lookSpeed);
                //Physics2D.Raycast(Marker.transform.position, Marker.transform.TransformDirection(Vector2.up), castDistance);
                Debug.DrawRay(Marker.transform.position, Marker.transform.TransformDirection(Vector2.up) * castDistance, Color.white);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // player runs into opponent
        if (col.gameObject.tag.Equals("Opponent") && hasBall)
        {
            Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
            Debug.Log("Game Over");
            hasBall = false;
            winFlag= false;
            winLoseUI.SetActive(true);
            Text WinLoseText = winLoseUI.GetComponentInChildren<Text>();
            WinLoseText.text = "You Lost!!";
            StartCoroutine(NextLevelAfterWait(winFlag));
        }
    }

    private void checkIfPassed()
    {
        hit = Physics2D.Raycast(Marker.transform.position, Marker.transform.TransformDirection(Vector2.up), castDistance);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag.Equals("Player"))
            {
                Debug.DrawRay(Marker.transform.position, Marker.transform.TransformDirection(Vector2.up) * castDistance, Color.red);
                Debug.Log("Passed");
                fire_ball(2);
                audioSources[0].Play();
                Debug.Log("Hit : " + hit.collider.gameObject.name);
                hasBall = false;
                Ball.SetActive(false);
                Player hitObject = hit.collider.gameObject.GetComponent<Player>();
                hitObject.hasBall = true;
                hitObject.Ball.SetActive(true);
                score += 1;
            }
            else
            {
                Debug.Log("Did not pass");
            }
        }
    }
    private void checkIfScored()
    {
        hit = Physics2D.Raycast(Marker.transform.position, Marker.transform.TransformDirection(Vector2.up), castDistance);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag.Equals("Opponent"))
            {
                Debug.Log("Did not score");
                fire_ball(1);
                audioSources[2].Play();
                Debug.Log("Game Over");
                hasBall = false;
                Ball.SetActive(false);
                winFlag= false;
                winLoseUI.SetActive(true);
                Text WinLoseText = winLoseUI.GetComponentInChildren<Text>();
                WinLoseText.text = "You Lost!!";
                StartCoroutine(NextLevelAfterWait(winFlag));

            }
            else
            {
                Debug.DrawRay(Marker.transform.position, Marker.transform.TransformDirection(Vector2.up) * castDistance, Color.red);
                fire_ball(2);
                Debug.Log("Scored");
                audioSources[1].Play();
                hasBall = false;
                Ball.SetActive(false);
                score += 10;
                Debug.Log(score);
                winFlag= true;
                winLoseUI.SetActive(true);
                Text WinLoseText = winLoseUI.GetComponentInChildren<Text>();
                WinLoseText.text = "You Win!!";
                StartCoroutine(NextLevelAfterWait(winFlag));
            }
        }
    }

    private void fire_ball(int duration)
    {
        GameObject bullet = Instantiate(Ball.gameObject, Ball.gameObject.transform.position, Ball.gameObject.transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(Ball.gameObject.transform.TransformDirection(Vector2.up) * 10, ForceMode2D.Impulse);
        Destroy(bullet, duration);
    }

    IEnumerator NextLevelAfterWait(bool flag)
    {
        yield return new WaitForSeconds(3.0f);

        if (flag == true)
        {
            Scene m_Scene = SceneManager.GetActiveScene();
            if (m_Scene.name.Equals("Level1"))
                SceneManager.LoadScene("Level2");
            else if (m_Scene.name.Equals("Level2"))
                SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Scene m_Scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(m_Scene.name);
        }
    }
}
