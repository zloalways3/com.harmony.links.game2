using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
  private Tile[,] _grid;
  private bool _canDrawConnection = false;

  private List<Tile> _connections = new List<Tile>();
  private Tile _connectionTile;

  private List<int> _solvedConnections = new List<int>();

  private int _dimensionX = 0;
  private int _dimensionY = 0;
  private int _solved = 0;
  private Dictionary<int, int> _amountToSolve = new Dictionary<int, int>();


 [SerializeField] Text labelLevelLinks;
  private int levelLinks;
  private int[] countColors = {0,2,2,2,2,2};
    [SerializeField] Color[] colorsLinks;
  [SerializeField] Sprite[] spritesLinks;
  [SerializeField] AudioSource musicLinks;
  [SerializeField] AudioSource soundLinks;
  void Start()
  {
    if(PlayerPrefs.GetInt("musicLinks",1)==1) musicLinks.Play();
     levelLinks  = PlayerPrefs.GetInt("levelLinks", 0);
    labelLevelLinks.text = $"Level\n{levelLinks + 1}";
    _dimensionX = transform.childCount;
    _dimensionY = transform.GetChild(0).transform.childCount;
    _grid = new Tile[_dimensionX, _dimensionY];
    List<Tile> tilesLinks = new List<Tile>();
    for (int y = 0; y < _dimensionX; y++)
    {
      var row = transform.GetChild(y).transform;
      row.gameObject.name = "" + y;
      for (int x = 0; x < _dimensionY; x++)
      {
        var tile = row.GetChild(x).GetComponent<Tile>();
        tile.gameObject.name = "" + x;
        tile.onSelected.AddListener(onTileSelected);
        //_CollectAmountToSolveFromTile(tile);
        _grid[x, y] = tile;
         tilesLinks.Add(tile);
      }
    }
    for(int i = 1;i<countColors.Length;i++)
     {
       while (countColors[i]>0)
        {
          Tile tmp = tilesLinks[Random.Range(0, tilesLinks.Count)];
          if(tmp.cid==0)
           {
            tmp.cid = i;
            tmp.isPlayble = true;
            tmp.MarkComponentRenderer.sprite = spritesLinks[i];
            //tmp.MarkComponentRenderer.color = colorsLinks[i];
            tmp.ConnectionComponentRenderer.color = colorsLinks[i];
            countColors[i]--;
           }
        }     
     }
    for (int y = 0; y < _dimensionX; y++)
    {
      for (int x = 0; x < _dimensionY; x++)
      {
        _grid[x, y].check();
        _CollectAmountToSolveFromTile(_grid[x, y]);
      }
    }
    SetGameStatus(_solved, _amountToSolve.Count);
    _OutputGrid();
  }

  private void endLinks() {
     int resultLinks = 0;
     if (_solved > 0) resultLinks = 1;
     if (_solved >= 3) resultLinks = 2;
     if (_amountToSolve.Count==0) resultLinks = 3;
     print(resultLinks);
     PlayerPrefs.SetInt("endLinks", resultLinks);
     PlayerPrefs.Save();
     SceneManager.LoadScene(5);
  }


  void _CollectAmountToSolveFromTile(Tile tile)
  {
    if (tile.cid > Tile.UNPLAYABLE_INDEX)
    {
      if (_amountToSolve.ContainsKey(tile.cid))
        _amountToSolve[tile.cid] += 1;
      else _amountToSolve[tile.cid] = 1;
    }
  }

  void _OutputGrid()
  {
    var results = "";
    int dimension = transform.childCount;
    for (int y = 0; y < dimension; y++)
    {
      results += "{";
      var row = transform.GetChild(y).transform;
      for (int x = 0; x < row.childCount; x++)
      {
        var tile = _grid[x, y];
        if (x > 0) results += ",";
        results += tile.cid;
      }
      results += "}\n";
    }
    Debug.Log("Main -> Start: _grid: \n" + results);
  }

  Vector3 _mouseWorldPosition;
  float _mouseGridX, _mouseGridY;

  void Update()
  {
    if (Time.timeSinceLevelLoad > 40f) endLinks();
    if (_canDrawConnection)
    {
      _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      _mouseGridX = (_mouseWorldPosition.x);
      _mouseGridY = (_mouseWorldPosition.y);
     // if (_mouseGridX < 0) _mouseGridX *= -2.4f-_mouseGridX;
     // if (_mouseGridY < 0) _mouseGridY *= -1.8f-_mouseGridY;      


      if (_CheckMouseOutsideGrid())
            {
                print("out");
                return;
            }
            float tmpX = (_mouseGridX + 2f) /  0.6f;
            float tmpY = (_mouseGridY  + 1f) / 0.6f;
       print($"{tmpX} {tmpY}");    
      Tile hoverTile = _grid[(int)(tmpX), 5-(int)(tmpY)];
      Tile firstTile = _connections[0];
      bool isDifferentActiveTile = hoverTile.cid > 0 && hoverTile.cid != firstTile.cid;

      if (hoverTile.isHighlighted || hoverTile.isSolved || isDifferentActiveTile) return;

      Vector2 connectionTilePosition = _FindTileCoordinates(_connectionTile);
      bool isPositionDifferent = IsDifferentPosition(_mouseGridX, _mouseGridY, connectionTilePosition);

      Debug.Log("Field -> OnMouseDrag(" + isPositionDifferent + "): " + _mouseGridX + "|" + _mouseGridY);

      if (isPositionDifferent)
      {
        var deltaX = System.Math.Abs(connectionTilePosition.x - ((int)tmpX));
        var deltaY = System.Math.Abs(connectionTilePosition.y  - (5-(int)tmpY));
        print($"{connectionTilePosition.x} {connectionTilePosition.y } || {(int)tmpX} {5-(int)tmpY}");
        bool isShiftNotOnNext = deltaX > 1 || deltaY > 1;
        bool isShiftDiagonal = (Mathf.Abs(deltaX) > 1 && Mathf.Abs(deltaY) > 1);
        Debug.Log("Field -> OnMouseDrag: isShiftNotOnNext = " + isShiftNotOnNext + "| isShiftDiagonal = " + isShiftDiagonal);
        if (isShiftNotOnNext || isShiftDiagonal) return;

        hoverTile.Highlight();
        hoverTile.SetConnectionColor(_connectionTile.ConnectionColor);

        _connectionTile.ConnectionToSide(
          5-(int)Mathf.Floor(tmpY) < connectionTilePosition.y,
          (int)Mathf.Floor(tmpX) > connectionTilePosition.x,
          5-(int)Mathf.Floor(tmpY) > connectionTilePosition.y,
          (int)Mathf.Floor(tmpX) < connectionTilePosition.x
        );

        _connectionTile = hoverTile;
        _connections.Add(_connectionTile);

        if (_CheckIfTilesMatch(hoverTile, firstTile))
        {
          _connections.ForEach((tile) => tile.isSolved = true);
          _canDrawConnection = false;
          _amountToSolve.Remove(firstTile.cid);
          if(PlayerPrefs.GetInt("soundsLinks",1)==1) soundLinks.Play();
          SetGameStatus(++_solved, _amountToSolve.Count + _solved);
          if (_amountToSolve.Keys.Count == 0)
          {
            endLinks();
            Debug.Log("GAME COMPLETE");
          }
        }
      }
    }
  }

  bool _CheckIfTilesMatch(Tile tile, Tile another)
  {
    return tile.cid > 0 && another.cid == tile.cid;
  }

  bool _CheckMouseOutsideGrid()
  {
    return _mouseGridY >= _dimensionY || _mouseGridY < -1.8f || _mouseGridX >= _dimensionX || _mouseGridX < -2.4f;
  }

  void onTileSelected(Tile tile)
  {
    Debug.Log("Field -> onTileSelected(" + tile.isSelected + "): " + _FindTileCoordinates(tile));
    if (tile.isSelected)
    {
      _connectionTile = tile;
      _connections = new List<Tile>();
      _connections.Add(_connectionTile);
      _canDrawConnection = true;
      _connectionTile.Highlight();
    }
    else
    {
      bool isFirstTileInConnection = _connectionTile == tile;
      if (isFirstTileInConnection) tile.HightlightReset();
      else if (!_CheckIfTilesMatch(_connectionTile, tile))
      {
        _ResetConnections();
      }
      _canDrawConnection = false;
    }
  }

  public void onRestart()
  {
    Debug.Log("Field -> onRestart");
    int dimension = transform.childCount;
    for (int y = 0; y < dimension; y++)
    {
      var row = transform.GetChild(y).transform;
      for (int x = 0; x < row.childCount; x++)
      {
        var tile = _grid[x, y];
        tile.ResetConnection();
        tile.HightlightReset();
        _CollectAmountToSolveFromTile(tile);
      }
    }
    _solved = 0;
    SetGameStatus(_solved, _amountToSolve.Count);
  }

  void SetGameStatus(int solved, int from)
  {

   // GameObject.Find("txtStatus").GetComponent<UnityEngine.UI.Text>().text = "Solve: " + solved + " from " + from;
  }

  void _ResetConnections()
  {
    Debug.Log("Field -> _ResetConnections: _connections.Count = " + _connections.Count);
    _connections.ForEach((tile) =>
    {
      tile.ResetConnection();
      tile.HightlightReset();
    });
  }

  Vector2 _FindTileCoordinates(Tile tile)
  {
    // Debug.Log("Field -> _FindTileCoordinates: " + tile.gameObject.name + " | " + tile.gameObject.transform.parent.gameObject.name);
    int x = int.Parse(tile.gameObject.name);
    int y = int.Parse(tile.gameObject.transform.parent.gameObject.name);
    return new Vector2(x, y);
  }

  public bool IsDifferentPosition(float gridX, float gridY, Vector2 position)
  {
    return position.x != gridX || position.y != gridY;
  }

  private class Connection
  {
    public Tile tile;
    public Vector2 position;
    public Connection(Tile tile, Vector2 position)
    {
      this.tile = tile;
      this.position = position;
    }

    public bool IsDifferentPosition(int gridX, int gridY)
    {
      return this.position.x != gridX || this.position.y != gridY;
    }
  }
}
