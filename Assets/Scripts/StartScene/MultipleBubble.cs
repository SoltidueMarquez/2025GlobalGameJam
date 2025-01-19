using UnityEngine;

namespace StartScene
{
    public class MultipleBubble : MonoBehaviour, IInit
    {
        public int addNum;
        public void Init()
        {
            this.tag = "Tool";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Utils.CheckIfPlayer(other))
            {
                var snake = other.GetComponent<Snake>();
                for (int i = 0; i < addNum; i++)
                {
                    snake.AddBodyPart();
                    if (AudioManager.Instance != null) AudioManager.Instance.PlayRandomSound("Eat");
                }
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }
}