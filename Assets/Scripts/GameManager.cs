using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


// this script sits on GameManager object in scene
public class GameManager: MonoBehaviour
{
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Button fishButton;
    [SerializeField] private GameObject fishPanel;
    [SerializeField] private Text fishText;
    [SerializeField] private Text scoreText;

    [SerializeField] private LayerMask clickMask;
    [SerializeField] private LayerMask bobberMask;
    [SerializeField] private GameObject bobberPrefab;
    [SerializeField] private float minBiteTime;
    [SerializeField] private float maxBiteTime;
    // list/contrainer for scriptable object fish
    [SerializeField] private List<Fish> fishes;

    private bool fishing = false;
    private bool fishBiting = false;
    private GameObject bobber;
    
    public void Start()
    {
        // creates and "caches" bobber gameobject from bobberPrefab
        bobber = Instantiate(bobberPrefab);
        bobber.SetActive(false);
        // sets totalCaught to all fishes from the list to 0
        for (int i = 0; i < fishes.Count; i++)
        {
            fishes[i].totalCaught = 0;
        }
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) == !mainCanvas.isActiveAndEnabled)
        {
            //Vector3 clickPosition = -Vector3.one;
            RaycastHit hit;
            // shoot a ray from your mouse pos on screen to world space object
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // if ray hit something in range of 100.0f and layer it hit is the one set on clickMask in editor on object (Water layer) and if not already fishing
            if (Physics.Raycast(ray, out hit, 100.0f, clickMask) && !fishing)
            {
                // set bobber active in scene
                bobber.SetActive(true);
                // set position of obber to position of what bobber hit
                bobber.transform.position = hit.point;
                // just setting bool that I am fishing true so that you cant enter this if unless it is set to false elsewhere
                fishing = true;
                // starts fishing routine (just another thread)
                StartCoroutine(FishingRoutine());
            }
            // logic could have been better similar to if above but layer it searches is bobberMask (Bobber in editor)
            else if (Physics.Raycast(ray, out hit, 100.0f, bobberMask) && fishBiting)
            {
                Debug.Log(bobber.name);
                bobber.gameObject.SetActive(false);
                EnableFishUI();
            }
        }
    }

    private IEnumerator FishingRoutine()
    {
        // this line will set waiting time between minBiteTime and maxBiteTime(set in editor) before it executes code undearneath this line
        yield return new WaitForSeconds(UnityEngine.Random.Range(minBiteTime, maxBiteTime));
        // gets animator from bobber object and plays animation called Shake
        bobber.GetComponent<Animator>().Play("Shake");
        fishBiting = true;
    }

    public void EnableFishUI()
    {
        float rnd = UnityEngine.Random.Range(0f, 100f);
        mainCanvas.gameObject.SetActive(true);
        // sets caught fish panel where mouse position is
        fishPanel.transform.position = Input.mousePosition;

        // if rnd is 5 or less set image and text of fishbutton and text to
        // image and text of fish from position 0 of the list 
        if (rnd <= 5f)
        {
            fishButton.image.sprite = fishes[0].image;
            fishText.text = fishes[0].fishName;
            // set total caught for fish at position 0 to its current value plus 1
            fishes[0].totalCaught = fishes[0].totalCaught+1;
        }
        else if (rnd > 5 && rnd <= 35f)
        {
            fishButton.image.sprite = fishes[1].image;
            fishText.text = fishes[1].fishName;
            fishes[1].totalCaught = fishes[1].totalCaught+1;
        }
        else
        {
            fishButton.image.sprite = fishes[2].image;
            fishText.text = fishes[2].fishName;
            fishes[2].totalCaught = fishes[2].totalCaught+1;
        }
    }


    // gets called on FishButton in FishingCanvas in editor - just resets bools ("flags"), disables FishingCanvas and runs UpdateUI
    public void GetFish()
    {
        mainCanvas.gameObject.SetActive(false);
        fishBiting = false;
        fishing = false;
        UpdateUI();
    }

    
    private void UpdateUI()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Fishes caught");
        // goes through all the fishes in the list and appends new line of text with fish name and current total caught
        for (int i = 0; i < fishes.Count; i++)
        {
            sb.AppendLine(fishes[i].fishName + ": " + fishes[i].totalCaught);
        }
        scoreText.text = sb.ToString();
    }
}




