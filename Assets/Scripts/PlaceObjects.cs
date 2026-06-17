using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaceObjects : MonoBehaviour
{
    private int Rows = 20;
    private int Lanes = 3;

    private int NrOfCactus = 8;
    private int NrOfRocks = 8;

    private float cellSize = 10f; // important to get the map dimensions 

    private int[,] levelMap;
    [SerializeField] private GameObject spawnParent; // needed for replay-remove-all
    [SerializeField] private GameObject Cactus;
    [SerializeField] private GameObject Rock;

	
    private GameObject[] corners;

	private List<int> GetShuffledRows()
    {
        List<int> rows = Enumerable.Range(0, Rows).ToList();

        for (int i = rows.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            (rows[i], rows[j]) = (rows[j], rows[i]);
        }

        return rows;
    } 

    public void placeObjects()
    {
		foreach (Transform child in spawnParent.transform)
            Destroy(child.gameObject);


        levelMap = new int[Rows, Lanes];
		// creat new list corners out of resourcses
		corners = Resources.LoadAll<GameObject>("CubePieces/Corners");

        // ==== create Map ==== //
        // set corners randomly 
        List<int> shuffledPieceIdizes = GetShuffledRows(); // shuffle pieces
        for (int i = 0; i < corners.Length && i < Rows; i++)
        {
            int row = shuffledPieceIdizes[i];
            int l = Random.Range(0, 3);
            levelMap[row, l] = i + 1; // 1..n = corners
        }
        
        // set Cactus (code = 20)
        List<int> shuffledCactusIdizes = GetShuffledRows(); // shuffled cactus
        
        for (int i = 0; i < NrOfCactus; i++)
        {
            int row = shuffledCactusIdizes[i];
            int l = Random.Range(0, 3);
            bool pieceSet = false;
            while (!pieceSet)
            {
                if (levelMap[row, l] == 0)
                {
                    levelMap[row, l] = 20;  
                    pieceSet = true;
                }
                else
                {
                    l = (l +1) % 3;
                } 
            }
        }
        

        // set Rock (code = 21)
        List<int> shuffledRockIdizes = GetShuffledRows(); // shuffled Rocks 
        
        for (int i = 0; i < NrOfRocks; i++)
        {
            int row = shuffledRockIdizes[i];
            int l = Random.Range(0, 3);
            bool pieceSet = false;
            while (!pieceSet)
            {
                if (levelMap[row, l] == 0)
                {
                    levelMap[row, l] = 21;  
                    pieceSet = true;
                }
                else
                {
                    l = (l +1) % 3;
                } 
            }
        }
        
        // spawn
        for (int row = 0; row < Rows; row++)
        {
            for (int lane = 0; lane < Lanes; lane++)
            {
                int tile = levelMap[row, lane];

                if (tile == 0)
                {
                    continue;
                }
                else if (tile > 0 && tile <= 7) // instanciate piece
                {
                    Debug.Log("spawn corner");
                    int randI = Random.Range(0, 3);

                    Quaternion randRot = PieceRotations.Rotations[randI];

                    Vector3 pos = new Vector3(lane * cellSize, 3, row * cellSize);

                    GameObject obj = Instantiate(corners[tile - 1], pos, randRot);
                    obj.transform.parent = spawnParent.transform;
                    
                }
                else if (tile == 20)  // instsanciante Cactus 
                {
                    int randY = Random.Range(0, 361);

                    Quaternion randRot = Quaternion.Euler(0, randY, 0);
                    
                    Vector3 pos = new Vector3(lane * cellSize, 0, row * cellSize);
                    
                    GameObject obj = Instantiate(Cactus, pos, randRot);
                    obj.transform.parent = spawnParent.transform;
               }
                else if (tile == 21)  // instsanciante Rock 
                {
                    int randY = Random.Range(0, 361);

                    Quaternion randRot = Quaternion.Euler(0, randY, 0);
                    
                    Vector3 pos = new Vector3(lane * cellSize, -1, row * cellSize);
                    
                    GameObject obj = Instantiate(Rock, pos, randRot);
                    obj.transform.parent = spawnParent.transform;
                }
            }
        }
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Forrest");

        foreach (GameObject tree in trees)
        {
            // Rotation nur Y (natürlich)
            float yRot = Random.Range(0f, 360f);
            tree.transform.Rotate(0, yRot, 0);

            // leichte Skalierung
            float scale = Random.Range(0.85f, 1.2f);
            tree.transform.localScale *= scale;
        }
    }
}