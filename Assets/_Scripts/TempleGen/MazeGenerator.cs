using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class MazeGenerator : MonoBehaviour
{

    [SerializeField] GameObject mazeCellPrefab;

    public Vector2Int MazeSize;

    [SerializeField] private MazeCell[,] _mazeGrid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        _mazeGrid = new MazeCell[MazeSize.x, MazeSize.y];

        for (int x = 0; x < MazeSize.x; x++)
        {
            for (int z = 0; z < MazeSize.y; z++)
            {
                print(_mazeGrid[x, z]);
                GameObject mc = Instantiate(mazeCellPrefab, gameObject.transform);
                _mazeGrid[x, z] = mc.GetComponent<MazeCell>();
                _mazeGrid[x, z].transform.localPosition = new Vector3(x, 0, z);
                if(x == MazeSize.x / 2)
                {
                    if(z == 0)
                    {
                        _mazeGrid[x,z].ClearFrontWall();
                        _mazeGrid[x,z].ClearBackWall();
                        _mazeGrid[x,z].ClearLeftWall();
                        _mazeGrid[x,z].ClearRightWall();
                    }
                    else if(z == MazeSize.y - 1)
                    {
                        _mazeGrid[x, z].ClearFrontWall();
                        _mazeGrid[x, z].ClearBackWall();
                        _mazeGrid[x, z].ClearLeftWall();
                        _mazeGrid[x, z].ClearRightWall();
                    }
                }
            }
        }
        yield return GenerateMaze(null, _mazeGrid[0,0]);

    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        yield return new WaitForSeconds(0.05f);

        MazeCell nextCell;
        do
        {

            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                yield return GenerateMaze(currentCell, nextCell);
            }
        }while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }


    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.localPosition.x;
        int z = (int)currentCell.transform.localPosition.z;
        Debug.Log(x);
        Debug.Log(z);
        if( x + 1 < MazeSize.x)
        {
            var cellToRight = _mazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }

        }

        if ((x - 1) >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if(cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if(z + 1 < MazeSize.y)
        {
            var cellToFront = _mazeGrid[x, z + 1];

            if(cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if(z-1 >= 0)
        {
            var cellToback = _mazeGrid[x, z - 1];

            if(cellToback.IsVisited == false)
            {
                yield return cellToback;
            }
        }



    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if(previousCell == null)
        {
            return;
        }

        //LeftToRight
        if(previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        //RightToLeft
        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        //UpToDown
        if(previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }

        //DownToUp
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
