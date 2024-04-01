using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A rare example of a class not named after the script its in. 
// Instead, trees are defined almost entirely be the behaviour of their nodes, so we do most of our definition here
public class TreeNode {

    public static float defaultDistanceBetweenNodes = 1.5f;

    public TreeNode parent;
    public TreeNode leftChild;
    public TreeNode rightChild;

    // Calculated for displaying the nodes in tree-form.
    public float treeNodeX;
    public float treeNodeY;

    // The limits of space represented by this node.
    public int spaceMinX;
    public int spaceMaxX;
    public int spaceMinY;
    public int spaceMaxY;

    // The center of the space represented by this node.
    public int centerX;
    public int centerY;

    // The actual boundaries this node uses to fill space.
    int xStart;
    int yStart;
    int xEnd;
    int yEnd;

    // Whether or not this node currently represents space (i.e. whether we're in task 2 or task 1 effectively)
    public bool hasSpace {
        get { return spaceMinX != spaceMaxX || spaceMinY != spaceMaxY; }
    }

    // The minimum amount of space (either vertically or horizontally)
    // that this node must have to be able to split. 
    public const int MIN_SPACE_FOR_SPLIT = 8;

    public IEnumerable<TreeNode> getAllLeaves() {
        if (isLeaf) {
            yield return this;
        }
        else {
            foreach(TreeNode leftLeaf in leftChild.getAllLeaves()) {
                yield return leftLeaf;
            }
            foreach(TreeNode rightLeaf in rightChild.getAllLeaves()) {
                yield return rightLeaf;
            }
        }
    }


    // Whether or not this node represents enough space to split.
    public bool canSplit() {
        if (!isLeaf) {
            return leftChild.canSplit() || rightChild.canSplit();
        }
        if (!hasSpace) {
            return true;
        }
        bool canSplitHorizontally = (spaceMaxX - spaceMinX) >= MIN_SPACE_FOR_SPLIT;
        bool canSplitVertically = (spaceMaxY - spaceMinY) >= MIN_SPACE_FOR_SPLIT;
        return canSplitVertically || canSplitHorizontally;
    }


    // A function that fills a grid with the appropriate values based on this node's space.
    public void fillSpace(int[,] wallGrid) {
        // Fill the space represented by this node. 
        // "1" indicates that there's a wall at that location.
        // "0" indicates free space. 
        // Can be expanded with more indices to represent things like enemies and treasures
        if (isLeaf) {
            for (int x = xStart; x <= xEnd; x++) {
                for (int y = yStart; y <= yEnd; y++) {
                    wallGrid[x, y] = 0;
                }
            }
        }
        else {
            // Here, we have our children fill their space then we connect their centers. 
            leftChild.fillSpace(wallGrid);
            rightChild.fillSpace(wallGrid);

            int currentX;
            int currentY;
            int targetX;
            int targetY;
            // Randomly decide the order we'll connect them (i.e. either starting at the left child or right child)
            if (Random.value <= 0.5f) {
                currentX = leftChild.centerX;
                currentY = leftChild.centerY;
                targetX = rightChild.centerX;
                targetY = rightChild.centerY;
            }
            else {
                currentX = rightChild.centerX;
                currentY = rightChild.centerY;
                targetX = leftChild.centerX;
                targetY = leftChild.centerY;
            }
            while (currentX != targetX) {
                wallGrid[currentX, currentY] = 0;
                if (currentX < targetX) {
                    currentX++;
                }
                else {
                    currentX--;
                }
            }
            while (currentY != targetY) {
                wallGrid[currentX, currentY] = 0;
                if (currentY < targetY) {
                    currentY++;
                }
                else {
                    currentY--;
                }
            }
        }
    }

    // A function for splitting the space between our two child nodes.
    public void splitSpace() {
        // Split the space we cover amongst our children. 

        // Start by giving both children the same space as us.
        leftChild.spaceMinX = spaceMinX;
        leftChild.spaceMaxX = spaceMaxX;
        leftChild.spaceMinY = spaceMinY;
        leftChild.spaceMaxY = spaceMaxY;

        rightChild.spaceMinX = spaceMinX;
        rightChild.spaceMaxX = spaceMaxX;
        rightChild.spaceMinY = spaceMinY;
        rightChild.spaceMaxY = spaceMaxY;

        bool canSplitHorizontally = (spaceMaxX - spaceMinX) >= MIN_SPACE_FOR_SPLIT;
        bool canSplitVertically = (spaceMaxY - spaceMinY) >= MIN_SPACE_FOR_SPLIT;

        if (canSplitHorizontally && Random.value <= 0.5f) {
            // Choose a dividing line. 
            int divLocation = (spaceMinX+spaceMaxX)/2;

            leftChild.spaceMinX = spaceMinX;
            leftChild.spaceMaxX = divLocation - 1;
            rightChild.spaceMinX = divLocation + 1;
            rightChild.spaceMaxX = spaceMaxX;
        }
        else if (canSplitVertically) {

            int divLocation = (spaceMinY + spaceMaxY) / 2;

            leftChild.spaceMinY = spaceMinY;
            leftChild.spaceMaxY = divLocation - 1;
            rightChild.spaceMinY = divLocation + 1;
            rightChild.spaceMaxY = spaceMaxY;
        }

        leftChild.centerX = (leftChild.spaceMinX + leftChild.spaceMaxX) / 2;
        leftChild.centerY = (leftChild.spaceMinY + leftChild.spaceMaxY) / 2;
        rightChild.centerX = (rightChild.spaceMinX + rightChild.spaceMaxX) / 2;
        rightChild.centerY = (rightChild.spaceMinY + rightChild.spaceMaxY) / 2;
        leftChild.chooseBoundaries();
        rightChild.chooseBoundaries();
    }

    // Recursive function to position the nodes in world space. 
    // Returns the x position of the rightmost part of this subtree
    public float assignTreeNodeXY(float lastX, float y) {
        treeNodeY = y;
        treeNodeX = defaultDistanceBetweenNodes / 2f + (leftChild == null ? lastX : leftChild.assignTreeNodeXY(lastX, y - defaultDistanceBetweenNodes));
        return rightChild == null ? treeNodeX : rightChild.assignTreeNodeXY(treeNodeX, y - defaultDistanceBetweenNodes);
    }

    // Whether or not this node is a leaf (i.e. whether or not it has children).
    public bool isLeaf {
        get { return leftChild == null && rightChild == null; }
    }

    // Function telling a node to attempt to split (only works on leaves and nodes with sufficient space).
    public bool tryToSplit() {
        if (!isLeaf) {
            return false;
        }
        if (!canSplit()) {
            return false;
        }
        TreeNode newLeftChild = new TreeNode();
        newLeftChild.parent = this;
        leftChild = newLeftChild;

        TreeNode newRightChild = new TreeNode();
        newRightChild.parent = this;
        rightChild = newRightChild;

        if (hasSpace) {
            splitSpace();
        }

        return true;
    }

    // The total number of nodes in the sub-tree starting from this node.
    public int totalNumberOfNodes() {
        int numInLeftBranch = 0;
        if (leftChild != null) {
            numInLeftBranch = leftChild.totalNumberOfNodes();
        }
        int numInRightBranch = 0;
        if (rightChild != null) {
            numInRightBranch = rightChild.totalNumberOfNodes();
        }
        return 1 + numInLeftBranch + numInRightBranch;
    }

    // Function that spawns a display showing the tree nodes (i.e. a tree view)
    public GameObject spawnDisplay(GameObject nodePrefab, GameObject parentNodeObj) {

        // Start by spawning our display the relevant position.
        GameObject nodeObj = GameObject.Instantiate(nodePrefab, new Vector3(treeNodeX, treeNodeY), Quaternion.identity);
        if (parentNodeObj != null) {
            nodeObj.transform.parent = parentNodeObj.transform;
            LineRenderer lineToParent = nodeObj.GetComponent<LineRenderer>();
            lineToParent.SetPosition(0, Vector3.zero);
            lineToParent.SetPosition(1, -nodeObj.transform.localPosition);
        }

        if (leftChild != null) {
            leftChild.spawnDisplay(nodePrefab, nodeObj);
        }
        if (rightChild != null) {
            rightChild.spawnDisplay(nodePrefab, nodeObj);
        }
        return nodeObj;
    }

    // YOUR CODE GOES HERE!
    public bool doRandomSplit()
    {
        // First, check if the current node is a leaf and can split.
        if (isLeaf)
        {
            return tryToSplit();
        }
        else
        {
            
            bool leftCanSplit = leftChild != null && leftChild.canSplit();
            bool rightCanSplit = rightChild != null && rightChild.canSplit();
        
            if (leftCanSplit && rightCanSplit)
            {
                
                if (Random.value < 0.5f)
                {
                    return leftChild.doRandomSplit() || rightChild.doRandomSplit();
                }
                else
                {
                    return rightChild.doRandomSplit() || leftChild.doRandomSplit();
                }
            }
            else if (leftCanSplit)
            {
                // Only the left child can split.
                return leftChild.doRandomSplit();
            }
            else if (rightCanSplit)
            {
                // Only the right child can split.
                return rightChild.doRandomSplit();
            }
        }
    
        // If we reach here, it means neither this node nor its children can split.
        return false;
    }


    public void chooseBoundaries() {
        
        int availableWidth = spaceMaxX - spaceMinX - 1; // -1 to avoid using the exact boundary
        int availableHeight = spaceMaxY - spaceMinY - 1; // -1 for the same reason

        
        int minWidth = Mathf.Min(MIN_SPACE_FOR_SPLIT / 2, availableWidth);
        int minHeight = Mathf.Min(MIN_SPACE_FOR_SPLIT / 2, availableHeight);

        
        int usedWidth = Mathf.Max(minWidth, Random.Range(minWidth, availableWidth));
        int usedHeight = Mathf.Max(minHeight, Random.Range(minHeight, availableHeight));

        
        int maxXStart = Mathf.Max(spaceMinX + 1, centerX - usedWidth / 2);
        int maxYStart = Mathf.Max(spaceMinY + 1, centerY - usedHeight / 2);

        
        xStart = Mathf.Min(maxXStart, centerX + 1);
        yStart = Mathf.Min(maxYStart, centerY + 1);

        
        xEnd = Mathf.Min(xStart + usedWidth, spaceMaxX - 1);
        yEnd = Mathf.Min(yStart + usedHeight, spaceMaxY - 1);

        
        xEnd = Mathf.Clamp(xEnd, xStart, spaceMaxX - 1);
        yEnd = Mathf.Clamp(yEnd, yStart, spaceMaxY - 1);
    }


}


public class SpaceTree {

    // Here we have some static functions to operate on trees. 
    public static GameObject spawnTreeDisplay(TreeNode rootNode, GameObject nodeDisplayPrefab, Transform parentObj) {
        rootNode.assignTreeNodeXY(parentObj.position.x, parentObj.position.y);
        GameObject rootObj = rootNode.spawnDisplay(nodeDisplayPrefab, null);
        rootObj.transform.parent = parentObj;
        rootObj.transform.localPosition = Vector3.zero;
        return rootObj;
    }

    public static TreeNode createTree(int desiredNodes=1, int spaceMinX=0, int spaceMaxX=0, int spaceMinY=0, int spaceMaxY=0) {
        TreeNode root = new TreeNode();

        root.spaceMinX = spaceMinX;
        root.spaceMaxX = spaceMaxX;
        root.spaceMinY = spaceMinY;
        root.spaceMaxY = spaceMaxY;
        root.centerX = (root.spaceMinX + root.spaceMaxX) / 2;
        root.centerY = (root.spaceMinY + root.spaceMaxY) / 2;
        root.chooseBoundaries();

        // A failsafe to ensure we eventually finish even if we can't build a tree of the desired size.
        int failedAttempts = 0;
        int maxNumTries = 10;

        while (failedAttempts < maxNumTries && root.totalNumberOfNodes() < desiredNodes) {
            bool successfulSplit = root.doRandomSplit();
            if (!successfulSplit) {
                failedAttempts++;
            }
        }
        return root;
    }
}

