using UnityEngine;
using FischlWorks_FogWar;

public class FogAdding : MonoBehaviour
{
    [SerializeField]
    private csFogWar fogWar = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        try
        {
            fogWar = GameObject.Find("FogWar").GetComponent<csFogWar>();
        }
        catch
        {
            Debug.LogErrorFormat("Failed to fetch csFogWar component. " +
                "Please rename the gameobject that the module is attached to as \"FogWar\", " +
                "or change the implementation located in the csFogVisibilityAgent.cs script.");
            return; // Early exit if fogWar is not found
        }

        // Add the fog revealer and get its index
        fogWar.AddFogRevealer(new csFogWar.FogRevealer(transform, 10, true));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
