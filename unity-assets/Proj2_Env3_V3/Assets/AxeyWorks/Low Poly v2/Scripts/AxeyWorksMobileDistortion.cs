using UnityEngine;

public class AxeyWorksMobileDistortion : MonoBehaviour
{
    private Material Material;

    void Start()
    {
        Material = GetComponent<Renderer>().material;
    }
    void Update()
    {
        var waving = Material.GetVector("_WaveAndDistance");
        var wavingPower = waving.w;
        var wavingMod = Time.deltaTime * 0.10f * wavingPower * Random.value;

        waving.x += wavingMod;
        waving.y -= wavingMod;
        Material.SetVector("_WaveAndDistance", new Vector4(waving.x, waving.y, waving.z, waving.w));
    }
}