using UnityEngine;
using TMPro;

public class DisplayTimer : MonoBehaviour
{
   public GameController gameController;
   public TextMeshProUGUI timerText;

   void Update()
   {
      timerText.text = gameController.gameTimer.ToString("F2"); 
   }
}
