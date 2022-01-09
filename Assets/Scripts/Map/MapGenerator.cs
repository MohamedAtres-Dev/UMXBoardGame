using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour
{

    #region Variables
    
    [SerializeField] int cols, rows; //this will be used to define how many tiles for height and width
    [SerializeField] float tileSize = 10; // this is used to define tile size so i can position them next to each other

    [SerializeField] Vector2 offset; // this is used to reposition the tiles in the current position in the camera field of view 
    
    [Space]
    [SerializeField] Transform tilesParent; // this is used to put the parent to clean the heirachy
    

    public GameObject defaultTile;
    public List<TilePrefab> tilePrefabs = new List<TilePrefab>();
   
    
    
    
    private List<Vector2> tempShortcutPositions = new List<Vector2>(); // get positions of short cut tiles
    private List<Vector2> tempPitFallPositions = new List<Vector2>(); // get positions of short cut tiles
    
    [Space]
    [SerializeField] private int _maxPitFall = 2; // the number of pitfall tiles
    [SerializeField] private int _maxShortCut = 2; // the number of shortcut tiles


    private Dictionary<Vector2, string> allTiles = new Dictionary<Vector2, string>(); // collect all the tiles in this dictionary



    #endregion

    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {
        //Generate map on XZ plane
        GenerateMap();
    }
    #endregion

    #region Methods
    private void GenerateMap()
    {
        //Get random shortcut tiles positions
        for (int i = 0; i < _maxShortCut; i++)
        {
            GenerateRandomShortcut();
        }

        //Get random pitfall tiles positions
        for (int i = 0; i < _maxPitFall; i++)
        {
            GenerateRandomPitFall();
        }


        //replace default tiles with special tiles 
        AssignSpecialTiles();

        //set all tiles in the dictionary with default tile 
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {

                if (!allTiles.ContainsKey(new Vector2(i, j)))
                {
                    var tile = Instantiate(defaultTile, new Vector3(i * tileSize + offset.x, 0, j * tileSize + offset.y), Quaternion.identity , tilesParent);
                }
                else
                {
                    foreach (var item in tilePrefabs)
                    {
                        string thisTileName;
                        allTiles.TryGetValue(new Vector2(i, j), out thisTileName);
                        if (item.tileName == thisTileName)
                        {
                            var tile = Instantiate(item.prefab, new Vector3(i * tileSize + offset.x, 0, j * tileSize + offset.y), Quaternion.identity, tilesParent);
                        }
                    }
                    Debug.Log("Special Type");
                }

            }
        }


    }

    private void AssignSpecialTiles()
    {
        //Start Tile Condition
        var startTile = new Vector2(0, 0);
        allTiles.Add(startTile, "Start");

        //Finish Tile Condition
        var finishTile = new Vector2(0, cols - 1);
        allTiles.Add(finishTile, "Finish");

        //// Shortcut Tile Condition
        for (int i = 0; i < _maxShortCut; i++)
        {
            allTiles.Add(tempShortcutPositions[i], "Shortcut");
        }

        // //Pitfall Tile Condition
        for (int i = 0; i < _maxPitFall; i++)
        {
            allTiles.Add(tempPitFallPositions[i], "Pitfall");
        }
    }

    private void GenerateRandomShortcut()
    {
        //Conditions for short cut its z not equal height also not equal finish or Start tiles positions (0,0) , (height, width) , ( x < 2 && x != 0 , z != height) , (
        //Not to close to start tile at least 2 tiles away
        //Must have 2 tiles 

        Vector2 point = new Vector2(Random.Range(0, rows), Random.Range(0, cols - 1));

        var canUse = CheckPointApplicable(point);

        if (canUse)
        {
            Debug.Log("Short Cut Point Available " + point.x + " " + point.y);
            tempShortcutPositions.Add(point);
        }
        else
        {
            GenerateRandomShortcut();
        }
    }

    private void GenerateRandomPitFall()
    {
        //conditions for pitfall its z not equal 0  also not equal finish or Start tiles positions (0,0) , (height, width) , ( z > 2 , z)
        //Not to close to finish tile at least 2 tiles away

        Vector2 point = new Vector2(Random.Range(0, rows), Random.Range(1, cols));

        var canUse = CheckPointApplicable(point);

        if (canUse)
        {
            Debug.Log("PitFall Point Available " + point.x + " " + point.y);
            tempPitFallPositions.Add(point);
        }
        else
        {
            GenerateRandomPitFall();
        }

    }


    private bool CheckPointApplicable(Vector2 point)
    {
        if (point.x == 0 && point.y == 0)
        {
            return false;

        }
        else if (point.x == 0 && point.y == cols - 1)
        {
            return false;
        }
        else if (tempPitFallPositions.Contains(point))
        {
            return false;
        }
        else if (tempShortcutPositions.Contains(point))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    #endregion
}


[System.Serializable]
public class TilePrefab
{
    public GameObject prefab;
    public string tileName;
}