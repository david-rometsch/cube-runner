using UnityEngine;

public class CollectPiece : MonoBehaviour
{
    public string solution = "";
	[SerializeField] private GameController gameController;
	[SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip audioClip;


    void OnTriggerEnter(Collider other)
    {
        IdentifyPiece piece = other.GetComponent<IdentifyPiece>();
		if  (piece == null) return; 
		if (piece.pieceType == PieceType.EndGame) {
			gameController.EndGame();
		} 
        else if (piece.pieceType == PieceType.Cactus) {
            gameController.GameOver();
        }
        else  
        {
            sfxSource.PlayOneShot(audioClip, 5f);

            // if collision with collectible 
            string pieceName = piece.pieceType.ToString();
            
            //check rotation to now which side is up
            Quaternion current = other.transform.rotation;
            if (Quaternion.Angle(current, PieceRotations.Rotations[0]) < 0.01f)
            {
                solution += pieceName[0];
            }
            else if (Quaternion.Angle(current, PieceRotations.Rotations[1]) < 0.01f)
            {
                solution += pieceName[1];
            }
            else if (Quaternion.Angle(current, PieceRotations.Rotations[2]) < 0.01f)
            {
                Debug.Log("acctual pieces letters: " + string.Join(", ", pieceName));

                Debug.Log(pieceName[2]);
                solution += pieceName[2];
            }

            other.gameObject.SetActive(false);
            //Destroy(other.gameObject);
            Debug.Log("New solution-string: " + solution);
        }
    }
}