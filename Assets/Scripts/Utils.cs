using UnityEngine;

public class Utils {
    public static void SetLayerRecursively(GameObject obj, int layerIndex)
    {
        obj.layer = layerIndex;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layerIndex);
    }
}
