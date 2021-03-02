using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class CollisionHandler : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision)
        {
            switch(collision.gameObject.tag)
            {
                case ("Friendly"): Debug.Log("Friendly");
                    break;
                case ("Finish"): Debug.Log("Finish");
                    break;
                case ("Obstacle"): ReloadScene();
                    break;
            }
        }

        void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}