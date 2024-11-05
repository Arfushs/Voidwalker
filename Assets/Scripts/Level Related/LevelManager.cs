using System.Collections;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Game Objects")]
    [Space]
    [SerializeField] private Player _player;
    [SerializeField] private CinemachineConfiner2D _confiner2D;

    private GameObject _currentLevel;
    private GameObject _nextLevel;
    private int _currentLevelIndex;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        Cursor.visible = false;

        // Last level index yükleme veya varsayılan olarak 0 atama
        _currentLevelIndex = PlayerPrefs.HasKey("last_level") ? PlayerPrefs.GetInt("last_level") : 0;
    }

    private void Start()
    {
        LoadLevel(_currentLevelIndex);
    }

    public void LoadLevel(int levelIndex)
    {
        // Level sayısını belirle
        int totalLevels = Resources.LoadAll<GameObject>("Levels").Length;

        // Eğer levelIndex, level sayısını aşıyorsa başa dön
        if (levelIndex >= totalLevels)
        {
            levelIndex = 0;
            PlayerPrefs.SetInt("all_levels_finished",1);
        }

        // Mevcut leveli kapat ve yok et
        if (_currentLevel != null)
        {
            _currentLevel.SetActive(false);
            Destroy(_currentLevel);
        }

        // Yeni leveli yükle
        if (_nextLevel == null)
        {
            _currentLevel = InstantiateLevel(levelIndex);
        }
        else
        {
            _currentLevel = _nextLevel;
            _currentLevel.gameObject.SetActive(true);
            _nextLevel = null;
        }

        // Geçerli level indeksini güncelle
        _currentLevelIndex = levelIndex;
        PlayerPrefs.SetInt("last_level", _currentLevelIndex);
        
        if(_currentLevelIndex > PlayerPrefs.GetInt("max_level"))
            PlayerPrefs.SetInt("max_level", _currentLevelIndex);

        // Level init işlemleri
        Level currentLevelComponent = _currentLevel.GetComponent<Level>();
        currentLevelComponent.Init(_player, _confiner2D);

        // Bir sonraki leveli arka planda hazırlayın
        StartCoroutine(PrepareNextLevelCoroutine(levelIndex + 1));
    }

    private GameObject InstantiateLevel(int levelIndex)
    {
        string levelName = $"Levels/Level_{levelIndex + 1}";
        GameObject levelPrefab = Resources.Load<GameObject>(levelName);

        if (levelPrefab != null)
        {
            GameObject levelInstance = Instantiate(levelPrefab, transform);
            levelInstance.SetActive(true);
            return levelInstance;
        }
        else
        {
            Debug.LogError($"Level {levelName} bulunamadı.");
            return null;
        }
    }

    private IEnumerator PrepareNextLevelCoroutine(int levelIndex)
    {
        int totalLevels = Resources.LoadAll<GameObject>("Levels").Length;

        // Eğer levelIndex, toplam level sayısını aşıyorsa başa dön
        if (levelIndex >= totalLevels)
        {
            levelIndex = 0;
        }

        // Mevcut bir sonraki level varsa temizle
        if (_nextLevel != null)
        {
            Destroy(_nextLevel);
        }

        string nextLevelName = $"Levels/Level_{levelIndex + 1}";
        ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(nextLevelName);

        yield return resourceRequest;

        if (resourceRequest.asset != null)
        {
            _nextLevel = Instantiate((GameObject)resourceRequest.asset, transform);
            _nextLevel.SetActive(false);
        }
        else
        {
            Debug.LogError($"Next Level {nextLevelName} bulunamadı.");
        }
    }

    public void LoadNextLevel()
    {
        _player.transform.parent = null;
        LoadLevel(_currentLevelIndex + 1);
    }

    public Vector3 GetPlayerPosition() => _player.transform.position;
    public Transform GetPlayerTransform() => _player.transform;
}
