using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

// Structure to hold building data (sprite and prefab)
[System.Serializable]
public class BuildingData
{
    public string buildingName;
    public Sprite buildingSprite;
    public GameObject buildingPrefab;
}

public class BuildingMenuController : MonoBehaviour
{
    [SerializeField] private GameObject buildingButtonPrefab; // Prefab for the UI button
    [SerializeField] private Transform contentPanel; // Content GameObject of ScrollView (with Vertical Layout Group)
    [SerializeField] private Button mainBuildButton; // Main button to toggle menu
    [SerializeField] private GameObject scrollViewPanel; // ScrollView GameObject to toggle visibility
    [SerializeField] private List<BuildingData> buildings; // List of building data (set in Inspector)

    // Reference to your building placement system (adjust to match your project's script)
    [SerializeField] private GridBuildingSystem placementSystem; // Custom script for placing buildings

    private void Start()
    {
        // Ensure ScrollView is initially hidden
        scrollViewPanel.SetActive(false);

        // Add listener to main button to toggle ScrollView
        mainBuildButton.onClick.AddListener(ToggleBuildMenu);

        // Populate the ScrollView with buttons
        PopulateScrollView();
    }

    private void PopulateScrollView()
    {
        // Clear existing buttons (if any)
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Create a button for each building
        foreach (BuildingData building in buildings)
        {
            // Instantiate button prefab
            GameObject buttonObj = Instantiate(buildingButtonPrefab, contentPanel);
            
            // Get button components
            Button button = buttonObj.GetComponent<Button>();
            Image buttonImage = buttonObj.GetComponent<Image>();

            // Set button sprite
            if (building.buildingSprite != null)
            {
                buttonImage.sprite = building.buildingSprite;
            }
            else
            {
                Debug.LogWarning($"No sprite assigned for building: {building.buildingName}");
            }

            // Add click listener to select building
            button.onClick.AddListener(() => SelectBuilding(building.buildingPrefab));

            // Optional: Set button text or tooltip (if you have a Text child)
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = building.buildingName;
            }
        }
    }

    private void SelectBuilding(GameObject buildingPrefab)
    {
        // Pass the selected prefab to your placement system
        if (placementSystem != null)
        {
            placementSystem.InitializeWithBuilding(buildingPrefab);
        }
        else
        {
            Debug.LogWarning("BuildingPlacementSystem reference not set in BuildingMenuController.");
        }

        // Hide the ScrollView after selection
        scrollViewPanel.SetActive(false);
    }

    private void ToggleBuildMenu()
    {
        // Toggle ScrollView visibility
        scrollViewPanel.SetActive(!scrollViewPanel.activeSelf);
    }
}