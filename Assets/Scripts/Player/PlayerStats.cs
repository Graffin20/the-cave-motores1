using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ViewmodelEntry
{
    public string name;
    public GameObject viewmodel;
}

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    [Header("Viewmodels")]
    [SerializeField] private List<ViewmodelEntry> viewmodelEntries = new List<ViewmodelEntry>();
    [SerializeField] private GameObject currentViewModel;
    private string _currentViewmodelName = "None";

    private Dictionary<string, GameObject> viewmodels = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Instance = this;

        // Build dictionary from serialized list
        viewmodels.Clear();
        foreach (var entry in viewmodelEntries)
        {
            if (!string.IsNullOrEmpty(entry.name) && entry.viewmodel != null)
            {
                viewmodels[entry.name] = entry.viewmodel;
            }
        }

        currentViewModel = viewmodels["None"];
        _currentViewmodelName = "None";
    }

    public void AddHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
    }

    public bool IsHealthFull()
    {
        return currentHealth >= maxHealth;
    }

    public void ChangeViewmodel(string viewmodelName)
    {
        foreach (var vm in viewmodels.Values)
        {
            vm.SetActive(false);
        }

        if (currentViewModel == viewmodels[viewmodelName]) 
        { 
            Debug.Log("Unequipped viewmodel: " + currentViewModel.name);
            currentViewModel = viewmodels["None"];
            _currentViewmodelName = "None";
            viewmodels["None"].SetActive(true);
            return;
        }

        if (viewmodels.ContainsKey(viewmodelName))
        {
            currentViewModel = viewmodels[viewmodelName];
            _currentViewmodelName = viewmodelName;
            viewmodels[viewmodelName].SetActive(true);
            Debug.Log("Changing viewmodel to: " + viewmodelName);
        }
        else
        {
            Debug.LogWarning("Viewmodel not found: " + viewmodelName);
        }
    }

    public string CurrentViewmodel => _currentViewmodelName;
}
