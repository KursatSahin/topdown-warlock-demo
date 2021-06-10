using System.Threading.Tasks;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Utils;
using WarlockBrawls.Utils;
using static Utils.ContainerFacade;

public class GameManager : MonoBehaviour
{
    [Header("Character Selection Dependecies")]
    [SerializeField] private GameObject _characterSelectionPanel;

    [Header("Player Controllers")] 
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private PlayerActionController _playerActionController;

    [Header("Other Dependencies")] [SerializeField]
    private GameObject _genericPlayerPrefab;

    [SerializeField] private ParticleSystem[] _fogSystems;
    [SerializeField] private GameObject _circleAreaIndicator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            ShrinkFogAndRedline();
        }
    }

    private void ShrinkFogAndRedline()
    {
        for (int i = 0; i < _fogSystems.Length; i++)
        {
            var shapeModule = _fogSystems[i].shape;
            shapeModule.radiusThickness += .2f;
        }

        var circleScale = _circleAreaIndicator.transform.localScale;
        _circleAreaIndicator.transform.DOScale((new Vector3(circleScale.x - 1f, circleScale.y - 1f, circleScale.z)),8f);

    }

    public void OnHeroSelectionButtonClick(int id)
    {
        var selectedhero = (HeroTypes) id;
        
        EventManager.GetInstance().Notify(Events.HeroIsSelected, selectedhero);
        
        _characterSelectionPanel.SetActive(false);

        BuildHero(selectedhero);
    }

    private void BuildHero(HeroTypes type)
    {
        HeroData selectedHeroData;
        
        switch (type)
        {
            case HeroTypes.Hades: 
                selectedHeroData = HeroesData.hades;
                break;
            case HeroTypes.Poseidon: 
                selectedHeroData = HeroesData.poseidon;
                break;
            default:
            case HeroTypes.Zeus:
                selectedHeroData = HeroesData.zeus;
                break;
        }

        
        var spawnedPlayerView = LeanPool.Spawn(_genericPlayerPrefab).GetComponent<PlayerView>();
        spawnedPlayerView.SetHero(selectedHeroData);
        
        Camera.main.transform.GetComponent<CameraFollow>().target = spawnedPlayerView.transform;
        
        spawnedPlayerView.transform.position = new Vector3(0, 0, -1);
        _playerActionController.playerView = _playerMovementController.playerView = spawnedPlayerView;
        _playerActionController.selectedHeroData = selectedHeroData;
        _playerActionController.spellProjectilePrefab = selectedHeroData.spellVfx;
        
        _playerActionController.gameObject.SetActive(true);
        _playerMovementController.gameObject.SetActive(true);
    }

    public enum HeroTypes
    {
        Zeus = 0,
        Hades = 1, 
        Poseidon = 2
    }
}
