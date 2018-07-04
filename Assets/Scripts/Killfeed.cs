using UnityEngine;

public class Killfeed : MonoBehaviour {
    [SerializeField]
    private GameObject killfeedItemPrefab;

    [SerializeField]
    private Transform killfeedList;

    private void Start()
    {
        GameManager.singleton.onPlayerKilledCallback += OnKill;
    }

    public void OnKill(string player, string source)
    {
        GameObject obj = Instantiate(killfeedItemPrefab);
        KillfeedItem item = obj.GetComponent<KillfeedItem>();
        item.Setup(source, player);
        Destroy(obj.gameObject, item.destroyAfter);
        obj.transform.SetParent(killfeedList);
        obj.transform.SetAsFirstSibling();
    }
}
