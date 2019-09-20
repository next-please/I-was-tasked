using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketSlot : MonoBehaviour
{
    private GameObject character;

    public bool isOccupied;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        Debug.Log("hi");
    }

    public void SetOccupant(Piece piece, GameObject characterModel)
    {
        isOccupied = true;
        InstantiateModelPrefab(characterModel);
        Debug.Log("set occupant!");
    }

    public void ClearSlot()
    {
        isOccupied = false;
        if (character)
        {
            Destroy(character);
        }
        Debug.Log("slot cleared");
    }

    private void InstantiateModelPrefab(GameObject characterModel)
    {
        GameObject modelPrefab = Instantiate(characterModel) as GameObject;
        modelPrefab.transform.SetParent(this.transform);
        modelPrefab.transform.localPosition = new Vector3(0, 0.5f, 0);
        modelPrefab.transform.localScale = Vector3.one;
        modelPrefab.transform.rotation = this.transform.rotation;

        character = modelPrefab;
        // animator = modelPrefab.GetComponent<Animator>();
    }
}
