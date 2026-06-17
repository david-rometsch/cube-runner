using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class GameController: MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject finalUI;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private TextMeshProUGUI rightSolutionText; 
    [SerializeField] private TextMeshProUGUI finalUIText;
    [SerializeField] private CollectPiece collectPiece; 
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private PlaceObjects placeObjects;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject forest; 

    private bool gameStarted = false;
	private bool firstStart = true;
	private int frameCount = 0;
    public float gameTimer = 0f;
    public float finalTime;

    void Start()
    {
        Time.timeScale = 0f;   // freeze all
		placeObjects.placeObjects();
		randomizeForest(); 
    }
    

    void Update()
    {
		frameCount++;
        if (!gameStarted && frameCount > 100) 
        {
            gameTimer = 0.00f;
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                StartGame();
            }
        }
        
        gameTimer += Time.deltaTime;
        if (transform.position.y < -10f)
        {
            GameOver();
        }
    }

    public void StartGame()
    {
		// reset vor restart
        gameStarted = true;
		Time.timeScale = 0f;
        Time.timeScale = 1f;
		playerMovement.playerSpeed = 12f;
        startButton.SetActive(false);
        quitButton.SetActive(false);
        finalUI.SetActive(false);
		gameOverText.SetActive(false);
		rightSolutionText.gameObject.SetActive(false);

		if (firstStart) {
            firstStart = false;	
		}
		else {
			placeObjects.placeObjects();
		}

        gameTimer = 0f;
		collectPiece.solution = "";

        transform.parent.position = new Vector3(10f, 3f, -10f); // reset player position
		// start
        startButton.SetActive(false);
        gameStarted = true;
        Time.timeScale = 1f;
    }

  	public void EndGame()
    {
        Time.timeScale = 0f;
        finalUI.SetActive(true);
        finalTime = gameTimer;
        inputField.Select();
        inputField.ActivateInputField();
    }  

    public void GameOver()
    {
        Time.timeScale = 0f;
	
        startButton.SetActive(true);
		quitButton.SetActive(true);
		gameOverText.SetActive(true);

    }
    
	public void Quit()
    {
		Application.Quit();	
	}

	public void ShowStartQuitPanel()
    {
        finalUI.SetActive(false);
        startButton.SetActive(true);
        quitButton.SetActive(true);
    }

    public void CheckSolution(string input)
    {	
		string myInput = inputField.text.Trim();
		string mySolution = collectPiece.solution.Trim();
        Debug.Log("input: " + inputField.text + "solution: " + collectPiece.solution);
        if (string.Equals(
                myInput,
				mySolution,
                System.StringComparison.OrdinalIgnoreCase) && myInput.Length == 7)
        {
			rightSolutionText.gameObject.SetActive(true);
            rightSolutionText.text = "Your Solution is correct!\nYour Time was: " + finalTime.ToString("F2");
			inputField.gameObject.SetActive(false);
			ShowStartQuitPanel();
        }
        else
        {
            finalUI.SetActive(false); 
			rightSolutionText.gameObject.SetActive(true);
			if (myInput.Length < 7) {
                rightSolutionText.text = "DNF! - You have to collect all the pieces!";
			}
			else {
                rightSolutionText.text = "Your Solution is wrong!\nThe right solution ist: " + collectPiece.solution.ToString(); 
			}
            startButton.SetActive(true);
            quitButton.SetActive(true);
            gameOverText.SetActive(true);
        }
    }

    public void randomizeForest()
    {
        foreach (Transform child in forest.transform)
        {
            float randomY = Random.Range(0f, 360f);
            child.rotation = Quaternion.Euler(0, randomY, 0);
            
            float randomScale = Random.Range(0.6f, 1.4f);
            child.localScale = Vector3.one * randomScale;
        }
    }
}
