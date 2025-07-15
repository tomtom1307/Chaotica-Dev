using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponSpawner : MonoBehaviour
{
    public bool SpawnOnStart = false;
    public WeaponDataSO dataSO;
    GameObject model;
    public WeaponInstance weaponInstance;
    public VisualEffectAsset RarityVFX;

    public void CreateWeapon(WeaponInstance WI)
    {
        weaponInstance = WI;
        dataSO = WI.data;
        CreateWeapon(dataSO);
    }

    public void CreateWeapon(WeaponDataSO data)
    {
        AddWeaponVisual(data);

    }
    WeaponPickup WP;
    public void AddWeaponVisual(WeaponDataSO data)
    {
        model = Instantiate(data.model);
        WP = model.AddComponent<WeaponPickup>();
        WP.instance = weaponInstance;
        model.gameObject.layer = 0;
        model.transform.position = transform.position;
        model.transform.localRotation = Quaternion.Euler(data.DroppedWeaponQuaternion);
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
        BC.size *= 2;
        BC.isTrigger = true;
        Interactable inter = BC.gameObject.AddComponent<Interactable>();
        inter.onInteraction.AddListener(WP.Pickup);


        //VFX
        RarityVFX = Resources.Load<VisualEffectAsset>("Rarity");
        GameObject vfxHolder = new GameObject("VFXHolder");
        VisualEffect vfx = vfxHolder.AddComponent<VisualEffect>();
        vfxHolder.transform.parent = model.transform;
        vfxHolder.transform.localPosition = Vector3.zero; 
        vfx.visualEffectAsset = RarityVFX;
        vfx.SetVector4("Color", WeaponDataSO.GetColorByRarity(data.rarity));

        Destroy(gameObject);
    }


    void Start()
    {
        RarityVFX = Resources.Load<VisualEffectAsset>("Rarity");
        if (SpawnOnStart)
        {
            weaponInstance = new WeaponInstance(dataSO, 0,0);
            CreateWeapon(dataSO);
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
