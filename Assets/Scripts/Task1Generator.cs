using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Task1Generator : MonoBehaviour {

    public GameObject nodeDisplayPrefab;

    private GameObject _rootObj;
    private TreeNode _root;

    public int minDesiredNodes = 12;
    public int maxDesiredNodes = 16;

    void Start() {
        _root = SpaceTree.createTree();
        _rootObj = SpaceTree.spawnTreeDisplay(_root, nodeDisplayPrefab, transform);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (_rootObj != null) {
                Destroy(_rootObj);
            }
            _root.doRandomSplit();
            _rootObj = SpaceTree.spawnTreeDisplay(_root, nodeDisplayPrefab, transform);
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (_rootObj != null) {
                Destroy(_rootObj);
            }
            int desiredNodes = Random.Range(minDesiredNodes, maxDesiredNodes + 1);
            _root = SpaceTree.createTree(desiredNodes);
            _rootObj = SpaceTree.spawnTreeDisplay(_root, nodeDisplayPrefab, transform);
        }
    }
   
}
