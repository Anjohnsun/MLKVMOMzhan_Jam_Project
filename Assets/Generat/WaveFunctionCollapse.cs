using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveFunctionCollapse : MonoBehaviour
{
    public List<Tile> TilePrefabs;
    public Vector2Int MapSize = new Vector2Int(10, 10);

    private Tile[,] spawnedTiles;

    private Queue<Vector2Int> recalcPossibleTilesQueue = new Queue<Vector2Int>();
    private List<Tile>[,] possibleTiles;

    private void Start()
    {
        spawnedTiles = new Tile[MapSize.x, MapSize.y];

        /*
        foreach (Tile tilePrefab in TilePrefabs)
        {
            tilePrefab.CalculateSidesColors();
        }

        int countBeforeAdding = TilePrefabs.Count;
        for (int i = 0; i < countBeforeAdding; i++)
        {
            Tile clone;
            switch (TilePrefabs[i].Rotation)
            {
                case Tile.RotationType.OnlyRotation:
                    break;

                case Tile.RotationType.TwoRotations:
                    TilePrefabs[i].Weight /= 2;
                    if (TilePrefabs[i].Weight <= 0) TilePrefabs[i].Weight = 1;

                    clone = Instantiate(TilePrefabs[i], TilePrefabs[i].transform.position + Vector3.right,
                        Quaternion.identity);
                    clone.Rotate90();
                    TilePrefabs.Add(clone);
                    break;

                case Tile.RotationType.FourRotations:
                    TilePrefabs[i].Weight /= 4;
                    if (TilePrefabs[i].Weight <= 0) TilePrefabs[i].Weight = 1;

                    clone = Instantiate(TilePrefabs[i], TilePrefabs[i].transform.position + Vector3.right,
                        Quaternion.identity);
                    clone.Rotate90();
                    TilePrefabs.Add(clone);

                    clone = Instantiate(TilePrefabs[i], TilePrefabs[i].transform.position + Vector3.right * 2,
                        Quaternion.identity);
                    clone.Rotate90();
                    clone.Rotate90();
                    TilePrefabs.Add(clone);

                    clone = Instantiate(TilePrefabs[i], TilePrefabs[i].transform.position + Vector3.right * 3,
                        Quaternion.identity);
                    clone.Rotate90();
                    clone.Rotate90();
                    clone.Rotate90();
                    TilePrefabs.Add(clone);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        */

        Generate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            foreach (Tile spawnedTile in spawnedTiles)
            {
                if (spawnedTile != null) Destroy(spawnedTile.gameObject);
            }

            Generate();
        }
    }

    private void Generate()
    {
        possibleTiles = new List<Tile>[MapSize.x, MapSize.y];

        int maxAttempts = 10;
        int attempts = 0;
        while (attempts++ < maxAttempts)
        {
            for (int x = 0; x < MapSize.x; x++)
                for (int y = 0; y < MapSize.y; y++)
                {
                    possibleTiles[x, y] = new List<Tile>(TilePrefabs);
                }

            Tile tileInCenter = TilePrefabs[0];
            possibleTiles[MapSize.x / 2, MapSize.y / 2] = new List<Tile> { tileInCenter };

            recalcPossibleTilesQueue.Clear();
            EnqueueNeighboursToRecalc(new Vector2Int(MapSize.x / 2, MapSize.y / 2));

            bool success = GenerateAllPossibleTiles();

            if (success) break;
        }

        PlaceAllTiles();
    }

    private bool GenerateAllPossibleTiles()
    {
        int maxIterations = MapSize.x * MapSize.y;
        int iterations = 0;
        int backtracks = 0;

        while (iterations++ < maxIterations)
        {
            int maxInnerIterations = 500;
            int innerIterations = 0;

            while (recalcPossibleTilesQueue.Count > 0 && innerIterations++ < maxInnerIterations)
            {
                Vector2Int position = recalcPossibleTilesQueue.Dequeue();
                if (position.x == 0 || position.y == 0 ||
                    position.x == MapSize.x - 1 || position.y == MapSize.y - 1)
                {
                    continue;
                }

                List<Tile> possibleTilesHere = possibleTiles[position.x, position.y];

                int countRemoved = possibleTilesHere.RemoveAll(t => !IsTilePossible(t, position));

                if (countRemoved > 0) EnqueueNeighboursToRecalc(position);

                if (possibleTilesHere.Count == 0)
                {
                    // Зашли в тупик, в этих координатах невозможен ни один тайл. Попробуем ещё раз, разрешим все тайлы
                    // в этих и соседних координатах, и посмотрим устаканится ли всё
                    possibleTilesHere.AddRange(TilePrefabs);
                    possibleTiles[position.x + 1, position.y] = new List<Tile>(TilePrefabs);
                    possibleTiles[position.x - 1, position.y] = new List<Tile>(TilePrefabs);
                    possibleTiles[position.x, position.y + 1] = new List<Tile>(TilePrefabs);
                    possibleTiles[position.x, position.y - 1] = new List<Tile>(TilePrefabs);

                    EnqueueNeighboursToRecalc(position);

                    backtracks++;
                }
            }
            if (innerIterations == maxInnerIterations) break;

            List<Tile> maxCountTile = possibleTiles[1, 1];
            Vector2Int maxCountTilePosition = new Vector2Int(1, 1);

            for (int x = 1; x < MapSize.x - 1; x++)
                for (int y = 1; y < MapSize.y - 1; y++)
                {
                    if (possibleTiles[x, y].Count > maxCountTile.Count)
                    {
                        maxCountTile = possibleTiles[x, y];
                        maxCountTilePosition = new Vector2Int(x, y);
                    }
                }

            if (maxCountTile.Count == 1)
            {
                Debug.Log($"Generated for {iterations} iterations, with {backtracks} backtracks");
                return true;
            }

            Tile tileToCollapse = GetRandomTile(maxCountTile);
            possibleTiles[maxCountTilePosition.x, maxCountTilePosition.y] = new List<Tile> { tileToCollapse };
            EnqueueNeighboursToRecalc(maxCountTilePosition);
        }

        Debug.Log($"Failed, run out of iterations with {backtracks} backtracks");
        return false;
    }

    private bool IsTilePossible(Tile tile, Vector2Int position)
    {
        bool isAllRightImpossible = possibleTiles[position.x - 1, position.y]
            .All(rightTile => !CanAppendTile(tile, rightTile, Direction.Right));
        if (isAllRightImpossible) return false;

        bool isAllLeftImpossible = possibleTiles[position.x + 1, position.y]
            .All(leftTile => !CanAppendTile(tile, leftTile, Direction.Left));
        if (isAllLeftImpossible) return false;

        bool isAllForwardImpossible = possibleTiles[position.x, position.y - 1]
            .All(upTile => !CanAppendTile(tile, upTile, Direction.Up));
        if (isAllForwardImpossible) return false;

        bool isAllBackImpossible = possibleTiles[position.x, position.y + 1]
            .All(downTile => !CanAppendTile(tile, downTile, Direction.Down));
        if (isAllBackImpossible) return false;

        return true;
    }

    private void PlaceAllTiles()
    {
        for (int x = 1; x < MapSize.x - 1; x++)
            for (int y = 1; y < MapSize.y - 1; y++)
            {
                PlaceTile(x, y);
            }
    }

    private void EnqueueNeighboursToRecalc(Vector2Int position)
    {
        recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x + 1, position.y));
        recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x - 1, position.y));
        recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x, position.y + 1));
        recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x, position.y - 1));
    }

    private void PlaceTile(int x, int y)
    {
        if (possibleTiles[x, y].Count == 0) return;

        Tile selectedTile = GetRandomTile(possibleTiles[x, y]);
        Vector3 position = new Vector3(x, y, 0);
        spawnedTiles[x, y] = Instantiate(selectedTile, position, selectedTile.transform.rotation);
    }

    private Tile GetRandomTile(List<Tile> availableTiles)
    {
        List<float> chances = new List<float>();
        for (int i = 0; i < availableTiles.Count; i++)
        {
            chances.Add(availableTiles[i].Weight);
        }

        float value = Random.Range(0, chances.Sum());
        float sum = 0;

        for (int i = 0; i < chances.Count; i++)
        {
            sum += chances[i];
            if (value < sum)
            {
                return availableTiles[i];
            }
        }

        return availableTiles[availableTiles.Count - 1];
    }

    private bool CanAppendTile(Tile existingTile, Tile tileToAppend, Direction direction)
    {
        if (existingTile == null) return true;

        if (direction == Direction.Right)
        {
            return Enumerable.SequenceEqual(existingTile.allowedTilesRight, tileToAppend.allowedTilesLeft);
        }
        else if (direction == Direction.Left)
        {
            return Enumerable.SequenceEqual(existingTile.allowedTilesLeft, tileToAppend.allowedTilesRight);
        }
        else if (direction == Direction.Up)
        {
            return Enumerable.SequenceEqual(existingTile.allowedTilesTop, tileToAppend.allowedTilesBottom);
        }
        else if (direction == Direction.Down)
        {
            return Enumerable.SequenceEqual(existingTile.allowedTilesBottom, tileToAppend.allowedTilesTop);
        }
        else
        {
            throw new ArgumentException("Wrong direction value, should be Vector3.left/right/down/up",
                nameof(direction));
        }
    }
}
