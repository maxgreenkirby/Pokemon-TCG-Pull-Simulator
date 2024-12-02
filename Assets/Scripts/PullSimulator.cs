using UnityEngine;
using PrimeTween;
using UniRx;
using System.Collections.Generic;

public enum EState
{
    Menu,
    PackSelect,
    PackOpen,
    CardReveal,
}

public class PullSimulator : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Database _database;
    [SerializeField] private EState _state;
    [SerializeField] private Transform _packOrigin;
    [SerializeField] private Pack _packPrefab;
    private int _packCount = 10;
    private float _packRadius = 2.2f;
    private Vector3 _originalCameraPosition;

    [Header("Controls")]
    private float _sensitivity = 10f;
    private Vector3 _initialMousePosition;
    private bool _isDragging = false;
    private float _scroll;
    private Tween _alignTween;

    [Header("Pull")] 
    [SerializeField] private WorldCard _cardPrefab;
    private List<Pack> _packs = new List<Pack>();
    [SerializeField] private List<Card> _cards = new List<Card>();
    private int _cardCount = 5;
    private Pack _closestPack;
    private Pack _chosenPack;

    private void Awake()
    {
        MainEventHandler.ListenForEventStream<PackSelectEvent>().Subscribe(OnPackChooseEvent).AddTo(this);
        MainEventHandler.ListenForEventStream<PackOpenEvent>().Subscribe(OnPackOpenEvent).AddTo(this);
        MainEventHandler.ListenForEventStream<CardSelectEvent>().Subscribe(OnCardSelectEvent).AddTo(this);
    }

    private void Start()
    {
        _originalCameraPosition = Camera.main.transform.position;
        SwitchState(EState.Menu);
    }

    private void Update()
    {
        if (_state == EState.PackSelect)
        {
            RotatePacks();
        }

        // Temp logic for pack opening
        if (_state == EState.PackOpen && Input.GetKeyDown(KeyCode.Space))
        {
            SwitchState(EState.CardReveal);
        }
    }

    private void SwitchState(EState state)
    {
        _state = state;

        MainEventHandler.AddToEventStream(new StateChangeEvent(state));
    }

    private void OnPackChooseEvent(PackSelectEvent packOpenEvent)
    {
        _chosenPack = packOpenEvent.Pack;
        
        // Return if the chosen pack is not centered on screen
        if (_chosenPack != _closestPack) return;

        SwitchState(EState.PackOpen);

        foreach (Pack pack in _packs)
        {
            if (pack != _chosenPack)
            {
                Destroy(pack.gameObject);
            }
        }

        _packs.Clear();

        _chosenPack.SetTearMaterial();

        Tween.Position(_chosenPack.transform, new Vector3(0, 0, -2.25f), 0.25f, Ease.OutQuint);
        Tween.Position(Camera.main.transform, new Vector3(0, 0.5f, -3.25f), 1f, Ease.OutQuint);
    }

    private void OnPackOpenEvent(PackOpenEvent packOpenEvent)
    {
        // Open pack and show cards
        for (int i = 0; i < _cards.Count; i++)
        {
            float zOffset = i * 0.01f;
            WorldCard worldCard = Instantiate(_cardPrefab, new Vector3(0, -0.5f, -2.2f + zOffset), _cardPrefab.transform.rotation);
            worldCard.Initialize(_cards[i]);
            packOpenEvent.Pack.AddCard(worldCard);
        }
    }

    private void OnCardSelectEvent(CardSelectEvent cardSelectEvent)
    {
        WorldCard selectedCard = cardSelectEvent.Card;
        Tween.LocalPositionY(selectedCard.transform, 2f, 0.5f, Ease.OutQuart);

        _cardCount--;

        if (_cardCount == 0)
        {
            SwitchState(EState.Menu);
            _cardCount = 5;
        }
    }

    public void SpawnPacks()
    {
        SwitchState(EState.PackSelect);

        _packOrigin.rotation = Quaternion.identity;
        Camera.main.transform.position = _originalCameraPosition;

        _cards = DrawCards();

        for (int i = 0; i < _packCount; i++)
        {
            // Spawn packs in a circle around the origin
            float angle = i * Mathf.PI * 2 / _packCount;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * _packRadius;
            Quaternion rot = Quaternion.LookRotation(pos);
            Pack pack = Instantiate(_packPrefab, pos, rot, _packOrigin);

            // Simulate float animation with offset
            pack.Float(i * 0.15f);

            // Keep reference 
            _packs.Add(pack);
        }

        AlignPacks();
    }

    private void RotatePacks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _initialMousePosition = Input.mousePosition;
            _scroll = _packOrigin.eulerAngles.y;
            _isDragging = true;

            // Stop the alignment tween if it's active
            if (_alignTween.isAlive)
            {
                _alignTween.Stop();
            }
        }

        if (_isDragging)
        {
            Vector3 scrollDirection = Input.mousePosition - _initialMousePosition;

            // Only rotate if mouse is moving enough
            if (scrollDirection.magnitude > 1)
            {
                _scroll += -scrollDirection.x * _sensitivity * Time.deltaTime;
                _packOrigin.rotation = Quaternion.Euler(0, _scroll, 0);

                _initialMousePosition = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;

            AlignPacks();
        }
    }

    private void AlignPacks()
    {
        // Align the packs to the nearest angle
        float angle = 360 / _packCount;
        float eulerAngleY = _packOrigin.eulerAngles.y;
        float remainder = Mathf.Abs(eulerAngleY % angle);
        float offset = angle / 2;

        // Decide which direction to round to
        if (remainder > offset)
        {
            eulerAngleY = Mathf.Ceil(eulerAngleY / angle) * angle - offset;
        }
        else
        {
            eulerAngleY = Mathf.Floor(eulerAngleY / angle) * angle + offset;
        }

        float transitionTime = 0.75f;
        _alignTween = Tween.Rotation(_packOrigin, Quaternion.Euler(0, eulerAngleY, 0), transitionTime, Ease.OutCirc);

        FindClosestPack();
    }

    private void FindClosestPack()
    {
        Pack closestPack = _packs[0];
        
        // Find the closest pack to the camera
        foreach (Pack pack in _packs)
        {
            if (Vector3.Distance(pack.transform.position, Camera.main.transform.position) < Vector3.Distance(closestPack.transform.position, Camera.main.transform.position))
            {
                closestPack = pack;
            }
        }

        _closestPack = closestPack;
    }

    private List<Card> DrawCards()
    {
        List<Card> cards = new List<Card>();
        float randomChance;
        ERarity rarity;

        for (int i = 0; i < 5; i++)
        {
            randomChance = Random.Range(0f, 1f);

            if (randomChance < 0.005f)
            {
                rarity = ERarity.CrownRare;
            }
            else if (randomChance < 0.05f) // 0.01f + (i * 0.005)) // 3% on last pull
            {
                rarity = ERarity.UltraRare;
            }
            else if (randomChance < 0.2f + (i * 0.04)) 
            {
                rarity = ERarity.Rare;
            }
            else
            {
                rarity = ERarity.Common;
            }

            Card card = _database.GetCard(rarity);

            // Duplicate check
            while (cards.Contains(card))
            {
                card = _database.GetCard(rarity);
            }

            cards.Add(card);
        }

        return cards;
    }
}
