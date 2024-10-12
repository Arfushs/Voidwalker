using UnityEngine;

public class ProximityAudio : MonoBehaviour
{
    public float maxDistance = 5f; // Sesin tamamen duyulabileceği maksimum mesafe
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float distance = Vector2.Distance(LevelManager.Instance.GetPlayerPosition(), transform.position);

        if (distance <= maxDistance)
        {
            // Mesafeye bağlı olarak sesi ayarla
            _audioSource.volume = 1 - (distance / maxDistance);
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            _audioSource.Stop(); // Uzaklaştığında sesi durdur
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Daireyi yeşil olarak çiz
        Gizmos.DrawWireSphere(transform.position, maxDistance); // Sesin yayılma alanını temsil eden daire
    }
}