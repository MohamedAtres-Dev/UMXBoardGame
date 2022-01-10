using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// THis script is responsible for Generate tiles and reposition them 
/// </summary>
public class MapGenerator : MonoBehaviour
{

    #region Variables

    int cols, rows; //this will be used to define how many tiles for height and width
    float tileSize = 10; // this is used to define tile size so i can position them next to each other
    
    [Tooltip("This is used to modify the first position or tiles so it appear on the camera view ")]
    [SerializeField] Vector2 offset; 

    [Space]
    [Tooltip("Tiles Parent to Clean the Hierachy")]
    [SerializeField] Transform tilesParent; 

    [Tooltip("prefab of default tile")]
    public GameObject defaultTile;  

    [Tooltip("list of prefabes of special tiles")]
    public List<TilePrefab> tilePrefabs = new List<TilePrefab>();  


    [Space]
    [Tooltip("the number of pitfall tiles")]
    [SerializeField] private int _maxPitFall = 2;

    [Tooltip("THe number of ShortcutTiles")]
    [SerializeField] private int _maxShortCut = 2;  


    private Dictionary<Vector2, string> allTiles = new Dictionary<Vector2, string>(); // collect all the tiles in this dictionary

    private List<Vector2> tempShortcutPositions = new List<Vector2>(); // get positions of short cut tiles
    private List<Vector2> tempPitFallPositions = new List<Vector2>(); // get positions of short cut tiles

    #endregion

    #region Monobehaviour

    void Start()
    {
        //I'm doing this so I can later increase the grid also if other scripts like player movement depends on these value so the game manager 
        //will react as a connection manager
        rows = GameManager.Instance.numOfRows;
        cols = GameManager.Instance.numOfCols;
        tileSize = GameManager.Instance.tileSize;

        //Generate map on XZ plane
        GenerateMap();
    }
    #endregion

    #region Methods

    /// <summary>
    /// Generate the map of tiles also Include all special tiles like Shortcut and Pitfall
    /// This script is called once on the game scene starts
    /// </summary>
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


        //Assign special tiles to the tiles dictionary
        AssignSpecialTiles();

        //set the remaining tiles in the dictionary with default tile 
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {

                if (!allTiles.ContainsKey(new Vector2(i, j)))
                {

                    InstanstiateTile(defaultTile, new Vector2(i, j));
                }
                else
                {
                    foreach (var item in tilePrefabs)
                    {
                        string thisTileName;
                        allTiles.TryGetValue(new Vector2(i, j), out thisTileName);
                        if (item.tileName == thisTileName)
                        {
                            InstanstiateTile(item.prefab, new Vector2(i, j));
                        }
                    }
                   
                }

            }
        }


    }

    /// <summary>
    /// Before setting the default tile I can use this function to assign the positions of special tiles
    /// </summary>
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


    /// <summary>
    /// THis is a generic function to Instanstiate the tile gameobject
    /// I can use pool object but due to the fixed number of tiles and I create them all once
    /// </summary>
    /// <param name="go"></param>
    /// <param name="tilePos"></param>
    private void InstanstiateTile(GameObject go, Vector2 tilePos)
    {
        var tile = Instantiate(go, new Vector3(tilePos.x * tileSize + offset.x, 0, tilePos.y * tileSize + offset.y), Quaternion.identity, tilesParent);

        //set the index of 2d array of that tile so the player can use them later in movement
        tile.GetComponent<Tile>().currentTilePosition = tilePos; 
    }



    /// <summary>
    /// this function is called from pitfall and shortcut generation functions to make a test on given Point
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private bool CheckPointAvailable(Vector2 point)
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


    /// <summary>
    /// Generate or get the available positions of Shorcut tiles
    /// Conditions for short cut its z not equal height also not equal finish or Start tiles positions (0,0) , (height, width) , ( x < 2 && x != 0 , z != height) , (
    ///  Not to close to start tile at least 2 tiles away
    /// </summary>
    private void GenerateRandomShortcut()
    {      
        //Generate random point
        Vector2 point = new Vector2(Random.Range(0, rows), Random.Range(0, cols - 1));


        //check if the point available
        var canUse = CheckPointAvailable(point);



        if (canUse)
        {         
            //Check here Applicability of the point 
            if (CheckShortcutApplicable(point ))
            {

                //Add the point to the list of shortcut positions
                tempShortcutPositions.Add(point);
            }
            else
            {    
                //Change the coordinates of the point to be applicable
                var randOffset = 1;

                if(point.x + randOffset <= cols - 1 )
                {
                    tempShortcutPositions.Add(new Vector2(point.x + randOffset , point.y  ));
                }
                else
                {
                    tempShortcutPositions.Add(new Vector2(point.x - randOffset, point.y));
                }
            }

        }
        else
        {

            //Use recursion to generate another points to meet the num of needed shortcut tiles 
            GenerateRandomShortcut();
        }
    }


    /// <summary>
    /// This function is called from shortcut generation function to check applicablity of given point
    /// I need to make the tiles far from each other
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private bool CheckShortcutApplicable(Vector2 point)
    {
        if (tempShortcutPositions.Count == 0)
        {
            return true;
        }
        else if (
           (point.y - tempShortcutPositions[tempShortcutPositions.Count - 1].y) == 1 ||
           (tempShortcutPositions[tempShortcutPositions.Count - 1].y - point.y) == 1)
        {
            return false;
        }
        else
        {
            return true;
        }

    }


    /// <summary>
    /// Generate or get the available positions of Pitfall tiles
    /// conditions for pitfall its z not equal 0  also not equal finish or Start tiles positions (0,0) , (height, width) , ( z > 2 , z) 
    ///  Not to close to finish tile at least 2 tiles away
    /// </summary>
    private void GenerateRandomPitFall()
    {

        //Generate random point
        Vector2 point = new Vector2(Random.Range(0, rows), Random.Range(1, cols));

        //check if the point available
        var canUse = CheckPointAvailable(point);

        if (canUse)
        {
            
            //Check here Applicability of the point 
            if (CheckPitfallApplicable(point ))
            {
                //Add the point to the list of Pitfall positions
                tempPitFallPositions.Add(point);
            }
            else
            {
                //Change the coordinates of the point to be applicable
                var randOffset = 1;

                if (point.x + randOffset <= cols - 1)
                {
                    tempPitFallPositions.Add(new Vector2(point.x + randOffset, point.y));
                }
                else
                {
                    tempPitFallPositions.Add(new Vector2(point.x - randOffset, point.y));
                }

            }
        }
        else
        {
            //Use recursion to generate another points to meet the num of needed shortcut tiles 
            GenerateRandomPitFall();
        }

    }


    /// <summary>
    /// This function is called from pitfall generation function to check applicablity of given point
    /// I need to make the tiles far from each other also not above a shortcut tile
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private bool CheckPitfallApplicable(Vector2 point)
    {
        if (tempPitFallPositions.Count == 0)
        {
            return true;
        }
        else if (tempShortcutPositions.Contains(new Vector2(point.x , point.y - 1)) ||
            (point.y - tempPitFallPositions[tempPitFallPositions.Count - 1].y) == 1 ||
            (tempPitFallPositions[tempPitFallPositions.Count - 1].y - point.y) == 1)
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

/// <summary>
/// Make this custom class to set the prefab of special type with its name 
/// </summary>
[System.Serializable]
public class TilePrefab
{
    public GameObject prefab;
    public string tileName;
}