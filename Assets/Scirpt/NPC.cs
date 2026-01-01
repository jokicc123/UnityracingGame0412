using UnityEngine;

namespace CHANG
{
    public class NPC : MonoBehaviour, IInteractable
    {
        public bool rescued = false;

        public void Interact()
        {
            if (!rescued)
            {
                rescued = true;
                Debug.Log($"{gameObject.name} 被救援！");

                gameObject.SetActive(false);
                GameManager.Instance.RescueNPC(); // 修正拼寫
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Interact();
            }
        }
    }
}
