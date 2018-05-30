using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Torus TorusPrefab;

    [Space] 
    public Tower TowerFrom;
    public Tower TowerTo;
    public Tower TowerHelp;

    [Space]
    public GameObject Ui;
    public Slider Slider;
    public Text MinCount;
    public Text MaxCount;
    public Text CurrentCount;

    private readonly List<Torus> _toruses = new List<Torus>();
    
    
    // change from slider
    public void OnTorusCountChange(float count)
    {
        CurrentCount.text = count.ToString(CultureInfo.CurrentCulture);
    }

    // click on ui button
    public void Play()
    {
        Ui.SetActive(false);
        Clear();
        StartCoroutine(PlayInternal());
    }


    private void Start()
    {
        MinCount.text = Slider.minValue.ToString(CultureInfo.CurrentCulture);
        MaxCount.text = Slider.maxValue.ToString(CultureInfo.CurrentCulture);
        CurrentCount.text = Slider.value.ToString(CultureInfo.CurrentCulture);
    }


    private void Clear()
    {
        foreach (var toruse in _toruses)
            Destroy(toruse.gameObject);
        
        _toruses.Clear();
    }

    private IEnumerator PlayInternal()
    {
        yield return InstantiateToruses();
        yield return new WaitForSeconds(1f);
        yield return Visualize(0, TowerFrom, TowerTo, TowerHelp);
        
        yield return new WaitForSeconds(1f);
        Ui.SetActive(true);
    }

    private IEnumerator InstantiateToruses()
    {
        const float sizeMultiplyer = 0.05f;
        const float colorMultiplyer = 0.15f;

        for (var i = 0; i < (int)Slider.value; i++)
        {
            var torus = Instantiate(TorusPrefab);
            torus.gameObject.transform.localScale -= Vector3.one * sizeMultiplyer * i;
            torus.MeshRenderer.material.color += Color.white * colorMultiplyer * i;
            torus.transform.position = TowerFrom.TopPoint.position;
            torus.CurrentTower = TowerFrom;
            
            _toruses.Add(torus);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator Visualize(int torusIndex, Tower from, Tower to, Tower help)
    {
        if (torusIndex >= _toruses.Count)
            yield break;
        
        yield return Visualize(torusIndex + 1, from, help, to);
        yield return MoveToTower(_toruses[torusIndex], to);
        yield return Visualize(torusIndex + 1, help, to, from);
    }

    private IEnumerator MoveToTower(Torus torus, Tower tower)
    {
        torus.Rigidbody.isKinematic = true;
        yield return MoveTo(torus.transform, torus.CurrentTower.TopPoint.position);
        yield return MoveTo(torus.transform, tower.TopPoint.position);
        torus.CurrentTower = tower;
        torus.Rigidbody.isKinematic = false;
        
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator MoveTo(Transform torus, Vector3 destination)
    {
        const float time = 0.5f;
        var startPosition = torus.position;
        var startTime = Time.time;

        while (Time.time - startTime < time)
        {
            var newPosition = Vector3.Lerp(startPosition, destination, (Time.time - startTime) / time);
            torus.position = newPosition;
            yield return null;
        }
    }
}