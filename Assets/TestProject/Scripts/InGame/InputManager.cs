using UnityEngine;

namespace HikanyanLaboratory.InGame
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] GameManager _gameManager;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _gameManager.GridManager.MoveTile(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _gameManager.GridManager.MoveTile(Vector2.down);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _gameManager.GridManager.MoveTile(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _gameManager.GridManager.MoveTile(Vector2.right);
            }
        }
    }

}