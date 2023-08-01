using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoneWheel : MonoBehaviour
{
    public int maxNumber = 9;  // Assuming numbers from 0-9
    public float degreesPerNumber;  // This will store how many degrees represent one number
    public TMP_Text numberDisplay;
    private float currentRotation;
    private float yRotation;

    public int currentNumber;

    private void Awake()
    {
        degreesPerNumber = 360f / (maxNumber + 1);  // +1 to include zero
    }

    private void Update()
    {
        UpdateNumberBasedOnRotation();
    }

    void UpdateNumberBasedOnRotation()
    {
        Vector3 euler = transform.rotation.eulerAngles;
        yRotation = Mathf.FloorToInt(euler.x);
        Debug.Log("X angle: " + euler.x + " Z angle: " + euler.z + "Y angle: " + euler.y);
        currentRotation = transform.localEulerAngles.y % 360f;
        currentNumber = Mathf.FloorToInt(currentRotation);
        //currentNumber = Mathf.FloorToInt(currentRotation / degreesPerNumber);

        DisplayWheelNumber();
        // Update the visual representation of the number if needed. 
        // For example, if you have a text mesh or a material change.
    }

    void DisplayWheelNumber()
    {
        //numberDisplay.text = currentNumber.ToString();
        numberDisplay.text = yRotation.ToString();
    }

    public int GetCurrentNumber()
    {
        return currentNumber;
    }
}
