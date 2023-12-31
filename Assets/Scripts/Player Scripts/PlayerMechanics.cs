using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerMechanics : MonoBehaviour
{
    public Tilemap tilemap;
    public Rigidbody2D lightBoy;
    
    [SerializeField]
    private CountDown countDown;

    public float speed = 10.5f;

    [SerializeField]
    private GameEngine gameEngine;

    [SerializeField]
    private GameObject globalLight;

    [SerializeField]
    private GameObject playerBaby;

    private bool LevelOver = false;

    [SerializeField] private GameObject popLightPrefab;

    private Vector3 pos;


    private Queue<float> queue; // TODO: initialize
    private int smoothing = 5;
    
    private Color green = new Color(0.3224012f, 0.990566f, 0.3548364f);

    private bool hasMagicEdible;

   




    // Start is called before the first frame update

    private void Start()
    {
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 1f;
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius = 0.69f;
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightInnerRadius = 0.15f;
        playerBaby.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            SceneManager.LoadScene(0);
        }
        
        pos = transform.position;

        if (Input.GetKey("w") && !LevelOver || Input.GetKey(KeyCode.UpArrow) && !LevelOver)
        {
            
            pos.y += speed * Time.deltaTime;
            countDown.moveDrain();
            DrainLight();
        }
        if (Input.GetKey("s") && !LevelOver || Input.GetKey(KeyCode.DownArrow) && !LevelOver)
        {
            
            pos.y -= speed * Time.deltaTime;
            countDown.moveDrain();
            DrainLight();
        }
        if (Input.GetKey("d") && !LevelOver || Input.GetKey(KeyCode.RightArrow) && !LevelOver)
        {
            
            pos.x += speed * Time.deltaTime;
            countDown.moveDrain();
            DrainLight();

        }
        if (Input.GetKey("a") && !LevelOver || Input.GetKey(KeyCode.LeftArrow) && !LevelOver)
        {
            
            pos.x -= speed * Time.deltaTime;
            countDown.moveDrain();
            DrainLight();
        }
        
        lightBoy.transform.position = pos;

        if (lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius < 0.3f && !LevelOver)
        {
            Debug.Log("Noooooooooooo");
            gameEngine.ActivateLoseScreen();
            SetLevelOver(true);

        }

        if (Input.GetKeyDown("z"))
        {
            DropGreenPopLight();
            
        }

        if (Input.GetKeyDown("c"))
        {
            DropRedPopLight();

        }

        if (Input.GetKeyDown("x") && hasMagicEdible)
        {
            globalLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 20f;
            Invoke("turnGlobalLightOff", 1f);
            hasMagicEdible = false;
            playerBaby.SetActive(false);

        }

        if (lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius < 0.42)
        {
            StartCoroutine(Flicker());
        }

    }

 
    public void IncreaseLight()
    {
        Debug.Log("Before "+"Intensity "+ lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity + "Outer " + lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius + "Inner " + lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightInnerRadius);
        if (lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity <= 0.8) {
            lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity += 0.2f;
        } else
        {
            lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 1f;
        }
        if (lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius <= 0.552)
        {
            lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius += 0.138f;
        } else
        {
            lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius = 0.69f;
        }
        if (lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightInnerRadius <= 0.12)
        {
            lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightInnerRadius += 0.03f;
        }
        else
        {
            lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightInnerRadius = 0.15f;
        }
        Debug.Log("After "+"Intensity " + lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity + "Outer " + lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius + "Inner " + lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightInnerRadius);
    }

    public void DropGreenPopLight()
    {
        
        GameObject newPopLight = Instantiate(popLightPrefab, pos, Quaternion.identity);
        newPopLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color = green;
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity -= 0.1f;
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius -= 0.0138f;
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightInnerRadius -= 0.03f;
        

    }
    public void DropRedPopLight()
    {

        GameObject newPopLight = Instantiate(popLightPrefab, pos, Quaternion.identity);
        newPopLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color = Color.red;
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity -= 0.1f;
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius -= 0.0138f;
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightInnerRadius -= 0.03f;


    }
    public void DrainLight()
    {
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity -= 0.0012f * Time.deltaTime;
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius -= 0.00828f * Time.deltaTime;
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightInnerRadius -= 0.0018f * Time.deltaTime;
    }

    public void SetLevelOver(bool status)
    {
        LevelOver = status;
    }


    public void CrystalReducePlayerLight()
    {
        Debug.Log("light before " + lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity);
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity -= 0.2f;
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius -= 0.138f;
        lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightInnerRadius -= 0.03f;
        Debug.Log("I have reduced the light to "+lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity);
    }

    IEnumerator Flicker()
    {
            lightBoy.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = Random.Range(0.1f, 0.3f);
            var randomTime = Random.Range(0, 0.5f);
            yield return new WaitForSeconds(randomTime);
    }

    public void pickUpMagicEdible()
    {
        hasMagicEdible = true;
        Debug.Log("I'm pregnant!");
        playerBaby.SetActive(true);
    }

    public void turnGlobalLightOff()
    {
        globalLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0f;
    }
}
