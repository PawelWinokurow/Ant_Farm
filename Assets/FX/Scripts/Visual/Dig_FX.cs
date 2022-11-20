using UnityEngine;

public class Dig_FX : MonoBehaviour
{
    public ParticleSystemRenderer debrisRend;
    public ParticleSystemRenderer cloundsRend;
    public ParticleSystem ps;
    public void StartFx(FloorHexagon floorHexagon)
    {
        Texture texture = floorHexagon.GetComponent<MeshRenderer>().sharedMaterial.GetTexture("_MainTex");
        debrisRend.material.SetTexture("_MainTex", texture);
        cloundsRend.material.SetTexture("_MainTex", texture);
        ps.Clear();
        ps.Play();
    }

    public void StopFx()
    {
        ps.Stop();
    }
}

