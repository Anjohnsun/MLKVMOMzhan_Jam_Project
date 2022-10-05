using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class mapCreator : MonoBehaviour
{
    public Vector2Int MapSize = new Vector2Int(30, 30);
    public GameObject CellGen;
    public GameObject[] Borders;

    public GameObject[] Prefabs;
    public int ButtonMax;
    public int AntidoteMax;
    public int ScientistMax;

    [SerializeField] ControlSystem _controlSystem;
    [SerializeField] ButtonManager _buttonManager;
    [SerializeField] TaskManager _taskManager;
    [SerializeField] VirusManager _virusManager;

    private void Start()
    {
        BeginGenerat();
        StartCoroutine(SpawnObjects());
    }

    void BeginGenerat()
    {
        gameObject.transform.position = new Vector2(MapSize.x / -2 + (2 ^ 3) + 2, -MapSize.y + MapSize.y / 3 / 2 + (2 ^ 3) + 4);

        int row = 0;
        int col = 0;
        Vector3 pos;
        for (int i = 0; i < 8; i++)
        {
            if (i == 3 || i == 5) { row++; col = 0; }
            else if (i == 4) col++;

            pos = new Vector2(MapSize.x / 3 * col - 2 * col, MapSize.y / 3 * row - 2 * row);

            Instantiate(CellGen, gameObject.transform.position + pos, gameObject.transform.rotation, gameObject.transform);
            col++;
        }

        Vector3 posB1;
        Vector3 posB2;
        // Borders outside
        for (int i = 0; i < MapSize.y - 4; i++)
        {
            posB1 = new Vector2(-.5f, i - .5f);
            posB2 = new Vector2(MapSize.x - 5.5f, i - .5f);
            Instantiate(Borders[0], gameObject.transform.position + posB1, gameObject.transform.rotation, gameObject.transform);
            Instantiate(Borders[0], gameObject.transform.position + posB2, gameObject.transform.rotation, gameObject.transform);
        }
        for (int i = 0; i < MapSize.x - 6; i++)
        {
            posB1 = new Vector2(i + .5f, -.5f);
            posB2 = new Vector2(i + .5f, MapSize.y - 5.5f);
            Instantiate(Borders[2], gameObject.transform.position + posB1, gameObject.transform.rotation, gameObject.transform);
            Instantiate(Borders[1], gameObject.transform.position + posB2, gameObject.transform.rotation, gameObject.transform);
        }

        // Borders inside
        int fixedPosX = MapSize.x / 3 - 2;
        int fixedPosY = MapSize.y / 3 - 2;
        for (int i = 0; i < MapSize.y / 3 - 4; i++)
        {
            posB1 = new Vector2(fixedPosX + .5f, fixedPosY + 1.5f + i);
            posB2 = new Vector2(fixedPosX * 2 - .5f, fixedPosY + 1.5f + i);
            if ((MapSize.y / 3 - 2) / 2 == i)
            {
                Instantiate(Borders[5], gameObject.transform.position + posB1, gameObject.transform.rotation, gameObject.transform);
                Instantiate(Borders[5], gameObject.transform.position + posB2, gameObject.transform.rotation, gameObject.transform);
            }
            Instantiate(Borders[0], gameObject.transform.position + posB1, gameObject.transform.rotation, gameObject.transform);
            Instantiate(Borders[0], gameObject.transform.position + posB2, gameObject.transform.rotation, gameObject.transform);
        }
        for (int i = 0; i < MapSize.x / 3 - 2; i++)
        {
            posB1 = new Vector2(fixedPosX + .5f + i, fixedPosY + .5f);
            posB2 = new Vector2(fixedPosX + .5f + i, fixedPosY * 2 - .5f);
            if ((MapSize.x / 3 - 2) / 2 == i)
            {
                Instantiate(Borders[3], gameObject.transform.position + posB1, gameObject.transform.rotation, gameObject.transform);
                Instantiate(Borders[4], gameObject.transform.position + posB2, gameObject.transform.rotation, gameObject.transform);
                continue;
            }
            Instantiate(Borders[1], gameObject.transform.position + posB1, gameObject.transform.rotation, gameObject.transform);
            Instantiate(Borders[2], gameObject.transform.position + posB2, gameObject.transform.rotation, gameObject.transform);
        }
    }

    IEnumerator SpawnObjects()
    {
        List<Button> generatedButtons = new List<Button>();
        List<Scientist> generatedScientists = new List<Scientist>();

        yield return new WaitForEndOfFrame();

        List<GameObject> floorsW = GameObject.FindGameObjectsWithTag("floorW").ToList();
        List<GameObject> floorsB = GameObject.FindGameObjectsWithTag("floorB").ToList();

        int selectedNum;
        for (int i = 0; i < ButtonMax; i++)
        {
            selectedNum = Random.Range(0, floorsB.Count - 1);
            generatedButtons.Add(Instantiate(Prefabs[0], floorsB[selectedNum].transform.position, new Quaternion()).GetComponent<Button>());
            floorsB.RemoveAt(selectedNum);
        }
        for (int i = 0; i < AntidoteMax; i++)
        {
            selectedNum = Random.Range(0, floorsB.Count - 1);
            Instantiate(Prefabs[1], floorsB[selectedNum].transform.position, new Quaternion()); floorsB.RemoveAt(selectedNum);
        }
        for (int i = 0; i < ScientistMax; i++)
        {
            selectedNum = Random.Range(0, floorsW.Count - 1);
            generatedScientists.Add(Instantiate(Prefabs[2], floorsW[selectedNum].transform.position, new Quaternion()).GetComponent<Scientist>());
            floorsW.RemoveAt(selectedNum);
        }

        _controlSystem.RegistrateScientists(generatedScientists);
        _buttonManager.RegistrateButtons(generatedButtons);
        _taskManager.RefreshPresentation();
        _virusManager.CreateVirusSites();
    }
}
