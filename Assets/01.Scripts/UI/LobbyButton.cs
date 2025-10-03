using UnityEngine;
using UnityEngine.SceneManagement;

namespace MIE.UI
{
    public class LobbyButton : MonoBehaviour
    {
        public void Lobby()
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}