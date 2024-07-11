using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ParticleObject : MonoBehaviour
{
    public Rigidbody2D[]    particleObjects = new Rigidbody2D[5];
    public SpriteRenderer[] particleSprites = new SpriteRenderer[5];
    public string           PrefabId => "particle_object";
    
    public async void StartParticle(Color32 color,Vector3 pos)
    {
        transform.position = pos;
        for (int i = 0; i < particleObjects.Length; i++)
        {
            particleSprites[i].color = color;
            particleObjects[i].bodyType = RigidbodyType2D.Dynamic;
            particleObjects[i].AddForce(new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(0, 4f), 0),ForceMode2D.Impulse);
        }
        await UniTask.Delay(TimeSpan.FromSeconds(.5f));
        StopParticle();
    }
    public void StopParticle()
    {
        foreach (var item in particleObjects)
        {
            item.bodyType = RigidbodyType2D.Static;
            item.transform.localPosition = Vector3.zero;
        }

        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
