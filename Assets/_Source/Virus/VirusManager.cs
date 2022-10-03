using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusManager : MonoBehaviour
{
    [SerializeField] private List<VirusSite> _virusSites;
    [SerializeField] private List<VirusArea> _virusAreas;

    [SerializeField] private GameObject _virusSitePrefab;

    [SerializeField] private float _inlargeSpeed;
    [SerializeField] private float _actualVirusSiteScale;

    [SerializeField] private TaskManager _taskManager;

    public void CreateVirusSites()
    {
        CreateNewRandomVirusSite(_virusAreas[0], 10);
        StartCoroutine(EnlargeVirusSites());
    }

    private void CreateNewRandomVirusSite(VirusArea area, int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject newVirusSite = Instantiate(_virusSitePrefab, new Vector2(
                Random.Range(area.CenterX - area.Width / 2, area.CenterX + area.Width / 2),
                Random.Range(area.CenterY - area.Height / 2, area.CenterY + area.Height / 2)),
                new Quaternion());
            _virusSites.Add(newVirusSite.GetComponent<VirusSite>());
        }
    }

    private IEnumerator EnlargeVirusSites()
    {
        yield return new WaitForSeconds(10); 
        Debug.Log(_virusSites[0].transform.localScale.x);
        _actualVirusSiteScale += _inlargeSpeed;
        foreach (VirusSite virusSite in _virusSites)
        {
            virusSite.transform.localScale = new Vector2(_actualVirusSiteScale, _actualVirusSiteScale);
        }
        yield return StartCoroutine(EnlargeVirusSites());
    }

}

[System.Serializable]
public class VirusArea
{

    [SerializeField] private float _centerOfAreaX;
    [SerializeField] private float _centerOfAreaY;
    [SerializeField] private float _width;
    [SerializeField] private float _height;

    public float CenterX { get => _centerOfAreaX; }
    public float CenterY { get => _centerOfAreaY; }
    public float Width { get => _width; }
    public float Height { get => _height; }
}