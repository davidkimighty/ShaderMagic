using System.Collections;
using UnityEngine;

public class ColorRipple : MonoBehaviour
{
    private static readonly int BaseColorId = Shader.PropertyToID("_base_color");
    private static readonly int RippleColorId = Shader.PropertyToID("_ripple_color");
    private static readonly int CenterId = Shader.PropertyToID("_center");
    private static readonly int StartTimeId = Shader.PropertyToID("_start_time");
    private static readonly int SpeedId = Shader.PropertyToID("_speed");
    private static readonly int SmoothId = Shader.PropertyToID("_smooth");
    
    [SerializeField] private float _randomPointRadius = 0.5f;
    [SerializeField] private float _rippleSpeed = 1f;
    [SerializeField] private float _rippleSmooth = 0f;
    [SerializeField] private float _timeOffset = 3f;
    
    private Material _material;
    private Color _prevColor;

    private void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        _material = Instantiate(rend.sharedMaterial);
        rend.material = _material;

        _prevColor = _material.GetColor(BaseColorId);
        _material.SetColor(RippleColorId, _prevColor);
        _material.SetFloat(SpeedId, _rippleSpeed);
        _material.SetFloat(SmoothId, _rippleSmooth);

        StartCoroutine(StartRipple());
    }

    private IEnumerator StartRipple()
    {
        float elapsedTime = Mathf.Infinity;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime < _timeOffset)
            {
                yield return null;
                continue;
            }
            elapsedTime = 0f;
            Vector3 randomPoint = transform.position + Random.onUnitSphere * _randomPointRadius;
            Color randomColor = Color.HSVToRGB(Random.value, 1, 1);
            
            _material.SetVector(CenterId, randomPoint);
            _material.SetFloat(StartTimeId, Time.time);
            _material.SetColor(BaseColorId, _prevColor);
            _material.SetColor(RippleColorId, randomColor);
            _prevColor = randomColor;
            yield return null;
        }
    } 

    private void OnDestroy()
    {
        if (_material != null)
            Destroy(_material);
    }
}
