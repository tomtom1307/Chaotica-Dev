using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public WeaponDataSO dataSO;
    GameObject model;
    WeaponInstance weaponInstance;


    public void CreateWeapon(WeaponInstance WI)
    {
        weaponInstance = WI;
        dataSO = WI.data;
        CreateWeapon(dataSO);
    }

    public void CreateWeapon(WeaponDataSO data)
    {
        AddWeaponVisual(data);
        model.AddComponent<Rigidbody>();
    }
    WeaponPickup WP;
    public void AddWeaponVisual(WeaponDataSO data)
    {
        model = Instantiate(data.model);
        WP = model.AddComponent<WeaponPickup>();
        WP.instance = weaponInstance;
        model.gameObject.layer = 0;
        model.transform.position = transform.position;
        model.transform.localRotation = Quaternion.identity;
        model.transform.localScale = data.DroppedWeaponSize;
        BoxCollider BC = null;
        Renderer r = null;
        if (!model.TryGetComponent<Renderer>(out r))
        {
            
            r = model.GetComponentInChildren<Renderer>();
            if (r != null)
            {
                BC = r.gameObject.AddComponent<BoxCollider>();
                r.gameObject.layer = 0;

            }
        }
        else
        {
            
            BC = r.gameObject.AddComponent<BoxCollider>();
            BC.gameObject.layer = 0;
        }
        if (BC == null || r == null)
        {
            Debug.LogError("Weapon Pickup manager Ate Shit Adding Collider");
            return;
        }
        BC.gameObject.tag = "Interactable";
        Interactable inter = BC.gameObject.AddComponent<Interactable>();
        inter.onInteraction.AddListener(WP.Pickup);
    }


    void Start()
    {
        if (dataSO != null)
        {
            weaponInstance = new WeaponInstance(dataSO);
            CreateWeapon(dataSO);
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
