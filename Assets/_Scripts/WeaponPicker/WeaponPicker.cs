using System.Collections.Generic;
using UnityEngine;

public class WeaponPicker : MonoBehaviour
{
    [SerializeField] WeaponTable weaponTable;
    [SerializeField] WeaponSpawner[] weaponSpawners;
    [SerializeField] WeaponDataSO[] pickedWeapons;

    private void Start()
    {
        if (weaponTable == null) return;
        pickedWeapons = new WeaponDataSO[weaponSpawners.Length];
        List<PossibleWeapon> possibleWeapons = new List<PossibleWeapon>(weaponTable.possibleWeapons);
        List<int> weightThresholds;

        // Picking random weapon data for each spawner from weaponTable
        for(int j = 0; j < weaponSpawners.Length; j++)
        {
            weightThresholds = new List<int>();
            int sum = 0;
            // Running sum of weights in order
            foreach (PossibleWeapon weapon in possibleWeapons)
            {
                weightThresholds.Add(weapon.weight + sum);
                sum += weapon.weight;
            }
            int RNG = Random.Range(0, sum);
            int previousThreshold = 0; int pickedItemIndex = 0;
            // Checking in which range the random number is to decide which weapon has been selected
            for (int i = 0; i < weightThresholds.Count; i++)
            {
                if ((RNG > previousThreshold) & (RNG <= weightThresholds[i])) { pickedItemIndex = i; break; }
                previousThreshold = weightThresholds[i];
            }
            // Removing weapon from possible weapon pool to avoid duplicates
            pickedWeapons[j] = possibleWeapons[pickedItemIndex].weaponData;
            possibleWeapons.RemoveAt(pickedItemIndex);
        }
    }
    
    public void SpawnWeapons()
    {
        for(int j = 0;j < weaponSpawners.Length;j++) 
        {
            weaponSpawners[j].CreateWeapon(pickedWeapons[j]);
        }
    }
}
