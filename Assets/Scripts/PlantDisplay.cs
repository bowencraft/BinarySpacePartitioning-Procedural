using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDisplay : MonoBehaviour {

    public struct PenState {
        public Vector3 position;
        public float angle;
        public PenState(Vector3 startPosition, float startAngle) {
            position = startPosition;
            angle = startAngle;
        }
    }

    public Transform penObj;

    public GameObject segmentStampPrefab;

    public float unitsPerMove = 2f;
    public float degreesPerTurn = 90;

    public void displayPlantFromString(string displayString) {

        // First, delete any leftover display objects. 
        foreach (Transform child in transform) {
            if (child != penObj) {
                Destroy(child.gameObject);
            }
        }

        // Create our initial penstate. 
        // Initially we're facing up
        penObj.localPosition = Vector3.zero;
        penObj.localEulerAngles = new Vector3(0, 0, 90);

        Stack<PenState> penStack = new Stack<PenState>();

        for (int c = 0; c < displayString.Length; c++) {

            char instruction = displayString[c];

            if (instruction == 'F') {
                // Example of a drawing instruction. Moves the pen forward and stamps a line segment. 

                // Move and draw. 
                Vector3 startPos = penObj.localPosition;
                penObj.localPosition += penObj.right * unitsPerMove;
                Vector3 endPos = penObj.localPosition;

                // Stamp at the location in between the two points. 
                GameObject stampObj = Instantiate(segmentStampPrefab);
                stampObj.transform.parent = transform;
                stampObj.transform.localPosition = (startPos + endPos) / 2f;
                stampObj.transform.localEulerAngles = penObj.localEulerAngles;
            }
            else if (instruction == '-') {
                // Turn the pen counterclockwise

                // YOUR CODE FOR TASK 2-A HERE! 
                // Make sure you use the "degreesPerTurn" value.
            }
            else if (instruction == '+') {
                // Turn the pen clockwise

                // YOUR CODE FOR TASK 2-A HERE!
            }
            else if (instruction == '[') {
                // Stores the current state of the pen on the stack. 

                // YOUR CODE FOR TASK 2-B HERE!
                // You can make use of the provided "PenState" class and the "penStack"
            }
            else if (instruction == ']') {
                // Retrieve the last pen state from the stack and apply it to our pen.


                // YOUR CODE FOR TASK 2-B HERE!
                // You can make use of the provided "PenState" class and the "penStack"
            }

        }



    }
}
