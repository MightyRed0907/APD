using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OPConfig
{
    public bool isPlaced;
    public bool wasPlaced;
    public float placementSpeed = 0.3f;
    public OPItem Item;
    public Vector3 Position;
    public Quaternion Rotation;

    public UnityEvent OnPlacement;  
    public UnityEvent OnRemove;  

}
[System.Serializable]
public class OPSlot
{
    public bool isTaken;
    public bool wasTaken;
    public bool willClearSlot;
    public OPItem Item;
    public float placementSpeed = 0.3f;
    public Vector3 Position;
    public Quaternion Rotation;
}

public enum OPAreaType
{
    specified,
    slot
}

public class OPArea : MonoBehaviour
{
    public OPAreaType AreaType;
    public bool isActive = true;
    public bool changeParentOnPlace;
    public Transform targetParent;
    public Color highlightColor = Color.blue;
    public Transform referenceCam;
    public UnityEvent OnItemPlace;

    public List<OPConfig> Items = new List<OPConfig>();
    public List<OPSlot> SlotsPos = new List<OPSlot>();
    public string SlotItemName;

    public void PlaceObject(OPConfig placementConfig)
    {
        if (changeParentOnPlace)
        {
            placementConfig.Item.transform.SetParent(targetParent);
            placementConfig.Item.transform.SetAsLastSibling();
        }

        if (AreaType == OPAreaType.specified)
        {
            if(placementConfig.Item.currentAreaPlaced != null)
            {
                if (placementConfig.Item.currentAreaPlaced.Items.Contains(placementConfig))
                {
                    int iConf = placementConfig.Item.currentAreaPlaced.Items.IndexOf(placementConfig);
                    placementConfig.Item.currentAreaPlaced.Items[iConf].isPlaced = false;
                }
            }
            placementConfig.Item.GetComponent<ObjectLerper>().LocalLerpTowards(placementConfig.Position, placementConfig.placementSpeed);
            placementConfig.Item.GetComponent<ObjectRotator>().LerpRotation(placementConfig.Rotation, placementConfig.placementSpeed);
            placementConfig.OnPlacement.Invoke();
            placementConfig.Item.currentAreaPlaced = this;
            placementConfig.isPlaced = true;
            placementConfig.wasPlaced = true;
            OnItemPlace.Invoke();
        }
        else if (AreaType == OPAreaType.slot)
        {
            if (placementConfig.Item.currentAreaPlaced != null)
            {
                if (IsItemConfigPresent(placementConfig.Item.currentAreaPlaced, placementConfig.Item))
                {
                    foreach (OPSlot slot in placementConfig.Item.currentAreaPlaced.SlotsPos)
                    {
                        if (slot.Item == placementConfig.Item)
                        {
                            slot.isTaken = false;
                            slot.Item = null;
                            if (slot.willClearSlot)
                            {
                                slot.wasTaken = false;
                            }
                        }
                    }
                }
            }
            foreach (OPSlot slot in SlotsPos)
            {
                if (!slot.isTaken)
                {
                    placementConfig.Item.GetComponent<ObjectLerper>().LocalLerpTowards(slot.Position, placementConfig.placementSpeed);
                    placementConfig.Item.GetComponent<ObjectRotator>().LerpRotation(slot.Rotation, placementConfig.placementSpeed);
                    placementConfig.Item.currentAreaPlaced = this;
                    slot.isTaken = true;
                    slot.wasTaken = true;
                    slot.Item = placementConfig.Item;
                    placementConfig.OnPlacement.Invoke();
                    placementConfig.isPlaced = true;
                    placementConfig.wasPlaced = true;
                    OnItemPlace.Invoke();
                    break;
                }
            }

        }
    }

    public void ShowArea(OPArea oa)
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend)
        {
            rend.enabled = true;
           
            
            // Change the material of all hit colliders
            // to use a transparent shader.
            rend.material.shader = Shader.Find("Transparent/Diffuse");
            Color tempColor = rend.material.color;
            tempColor = highlightColor;
            tempColor.a = 0.35f;
            rend.material.color = tempColor;
            
        }
    }

    public int GetAvailableSlots()
    {
        int availSlots = 0;
        foreach (OPSlot slot in SlotsPos)
        {
            if (!slot.isTaken)
            {
                availSlots += 1;
            }
        }
        return availSlots;
    }
    
    public void HideArea(OPArea oa)
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend)
        {
            rend.enabled = false;

            /*
            // Change the material of all hit colliders
            // to use a transparent shader.
            rend.material.shader = Shader.Find("Transparent/Diffuse");
            Color tempColor = rend.material.color;
            tempColor.a = 0.3F;
            rend.material.color = tempColor;
            */
        }
    }
    public void AutoplaceObjectsOnArea()
    {
        StartCoroutine(IAutoplaceSpecificObjectsOnArea());
    }

    public IEnumerator IAutoplaceSpecificObjectsOnArea()
    {
        if(AreaType == OPAreaType.specified)
        {
            foreach (OPConfig oConf in Items)
            {
                while(oConf.Item.GetComponent<ObjectLerper>().IsCurrentlyLerping()) yield return null;
                PlaceObject(oConf);
                oConf.Item.GetComponent<Interactable>().isSetSequentially = false;
                oConf.Item.GetComponent<Interactable>().isInteractable = true;
            }
        }
        yield break;
    }

    public void AutoplaceObjectOnSlot(int objIndex)
    {
        StartCoroutine(IAutoplaceObjectOnSlot(objIndex));
    }

    public IEnumerator IAutoplaceObjectOnSlot(int objIndex)
    {
        if (AreaType == OPAreaType.slot)
        {
            while (Items[objIndex].Item.GetComponent<ObjectLerper>().IsCurrentlyLerping()) yield return null;
            PlaceObject(Items[objIndex]);
        }
        yield break;
    }

    public bool IsItemConfigPresent(OPArea area, OPItem item)
    {
        bool isPresent = false;
        if (area.Items.Count > 0)
        {
            foreach (OPConfig oConf in area.Items)
            {
                if (oConf.Item == item)
                {
                    /*
                    if (oConf.isPlaced)
                    {
                        isPresent = false;
                    }
                    else
                    {
                        isPresent = true;
                    }
                    */

                    isPresent = true;
                }
            }
        }
        else
        {
            isPresent = false;
        }

        return isPresent;
    }


}
