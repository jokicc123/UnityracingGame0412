using UnityEngine;

namespace CHANG
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance; // 單例模式

        public AudioSource aud;
        public AudioClip screamClip;
        public AudioClip ConsumeitemClip;

        private void Awake()
        {
            // 建立單例
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            // 確保 AudioSource 有綁定
            if (aud == null)
            {
                aud = GetComponent<AudioSource>();
            }
        }

        public void PlayScream()
        {
            if (screamClip != null)
            {
                aud.PlayOneShot(screamClip);
            }
        }

        public void PlayConsumeitemClip()
        {
            if (ConsumeitemClip != null)
            {
                aud.PlayOneShot(ConsumeitemClip);
            }
        }
    }
}
