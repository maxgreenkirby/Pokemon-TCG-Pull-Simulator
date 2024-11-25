using UnityEngine;
using PrimeTween;
using UniRx;
using System.Collections.Generic;

public class PullSimulator : MonoBehaviour
{
    [Header("Setup"), SerializeField] private Transform _packOrigin;
    [SerializeField] private Pack _packPrefab;
    private int _packCount = 10;
    private float _packRadius = 2.2f;

    [Header("Controls"), SerializeField] private float _sensitivity = 150f;
    private float _yaw;
    private Tween _alignTween;

    [Header("Pull"), SerializeField] private List<Pack> _packs = new List<Pack>();

    private void Awake()
    {
        MainEventHandler.ListenForEventStream<PackChooseEvent>().Subscribe(OnPackChooseEvent).AddTo(this);
    }

    void Start()
    {
        SpawnPacks();  
    }

    private void Update()
    {
        if (_packs.Count > 0)
        {
            RotatePacks();
        }
    }

    private void OnPackChooseEvent(PackChooseEvent packOpenEvent)
    {
        // Destroy all packs except the selected pack
        foreach (Pack pack in _packs)
        {
            if (pack != packOpenEvent.Pack)
            {
                Destroy(pack.gameObject);
            }
        }
        
        _packs.Clear();
    }

    private void SpawnPacks()
    {
        for (int i = 0; i < _packCount; i++)
        {
            // Spawn packs in a circle around the origin
            float angle = i * Mathf.PI * 2 / _packCount;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * _packRadius;
            Quaternion rot = Quaternion.LookRotation(-pos);
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
        // TODO: Convert all logic to use InputActions
        if (Input.GetMouseButton(0))
        {
            _yaw += Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
            _packOrigin.rotation = Quaternion.Euler(0, -_yaw, 0);

            if (_alignTween.isAlive)
            {
                _alignTween.Stop();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
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
    }
}
