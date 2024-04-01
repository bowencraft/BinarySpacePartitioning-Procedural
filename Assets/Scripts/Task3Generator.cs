using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Task3Generator : MonoBehaviour {
   
	public GameObject wallTilePrefab;

    public int gridWidth = 128;
    public int gridHeight = 64;

    public const float TILE_SIZE = 2f;

    public int minDesiredNodes = 12;
    public int maxDesiredNodes = 16;

    private TreeNode _root;

    private int[,] _wallGrid;

	void Start() {
        _wallGrid = new int[gridWidth, gridHeight];
        resetWallGrid();

        _root = SpaceTree.createTree(1, 0, gridWidth - 1, 0, gridHeight - 1);
        _root.fillSpace(_wallGrid);

        spawnGrid(_wallGrid);
    }

    public void resetWallGrid() {
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                _wallGrid[x, y] = 1;
            }
        }
    }

    public void spawnGrid(int[,] grid) {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        for (int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                if (grid[x,y] == 1) {
                    GameObject wallObj = Instantiate(wallTilePrefab);
                    wallObj.transform.parent = transform;
                    wallObj.transform.localPosition = new Vector3(x * TILE_SIZE, y * TILE_SIZE, 0);
                }
            }
        }

        // Now move ourself based on the size of the wall. 
        float ourX = -(grid.GetLength(0) * TILE_SIZE)/2f + TILE_SIZE / 2f;
        float ourY = -(grid.GetLength(1) * TILE_SIZE)/2f + TILE_SIZE / 2f;
        transform.position = new Vector3(ourX, ourY);

        foreach (TreeNode leafNode in _root.getAllLeaves()) {
            Debug.Log("Leaf Node (room) with boundaries: (" + leafNode.spaceMinX + ", " + leafNode.spaceMinY + ") -> (" + leafNode.spaceMaxX + ", " + leafNode.spaceMaxY + ")");
                 
        }

    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            resetWallGrid();
            _root.doRandomSplit();
            _root.fillSpace(_wallGrid);
            ApplyCellularAutomata(3); 
            spawnGrid(_wallGrid);
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            resetWallGrid();
            int desiredNodes = Random.Range(minDesiredNodes, maxDesiredNodes + 1);
            _root = SpaceTree.createTree(desiredNodes, 0, gridWidth-1, 0, gridHeight-1);
            _root.fillSpace(_wallGrid);
            ApplyCellularAutomata(3); 
            spawnGrid(_wallGrid);
        }
    }
    
    
    public void ApplyCellularAutomata(int iterations) {
        int[,] newGrid = new int[gridWidth, gridHeight];

        for (int iteration = 0; iteration < iterations; iteration++) {
            for (int x = 0; x < gridWidth; x++) {
                for (int y = 0; y < gridHeight; y++) {
                    int wallNeighbors = CountWallNeighbors(x, y);

                    newGrid[x, y] = (wallNeighbors < 2 || wallNeighbors > 5)? 1 : 0;
                }
            }

            // 复制新的状态到主 grid
            for (int x = 0; x < gridWidth; x++) {
                for (int y = 0; y < gridHeight; y++) {
                    _wallGrid[x, y] = newGrid[x, y];
                }
            }
        }
    }
    
    public int CountWallNeighbors(int x, int y) {
        int count = 0;
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                int nx = x + i;
                int ny = y + j;

                // 边界检查
                if (i == 0 && j == 0) continue; // 跳过自身
                if (nx < 0 || ny < 0 || nx >= gridWidth || ny >= gridHeight) {
                    count++; // 边界外视为墙
                } else if (_wallGrid[nx, ny] == 1) {
                    count++;
                }
            }
        }
        return count;
    }


}
