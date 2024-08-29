# Researched stuffs related to Game Natives

#### task_heli_mission- there are 19 flags starting from 0.
anyways according to my findings- TASK_HELI_MISSION does these stuffs at specified MissionFlags
```csharp
/*
* 0
* 1
* 2 - Chase
* 3 - Chase
* 4 - Flee
* 5 - Landing?
* 6 - Chase but in respectful way
* 7 - 
* 8 - 
* 9 - 
* 10 - 
* 11 -
* 12 -
* 13 -
* 14 -
* 15 -
* 16 -
* 17 - 
* 18 - Wander and fly here there.
* 19 - flee and deletes
*/
```

#### task_vehicle_mission- there are flags too 
will research about this later.

##



WeaponSlots - CombatTweaks.cs
 //Set of Weapons for them.
        private static eWeaponType[] SwatWeaponList = {
            eWeaponType.WEAPON_M4,
            eWeaponType.WEAPON_MP5,
            eWeaponType.WEAPON_MICRO_UZI,
            eWeaponType.WEAPON_SHOTGUN,
            eWeaponType.WEAPON_BARETTA,
            eWeaponType.WEAPON_SNIPERRIFLE
        };

        private static eWeaponType[] FbiWeaponList = {
            eWeaponType.WEAPON_M4,
            eWeaponType.WEAPON_MP5,
            eWeaponType.WEAPON_BARETTA,
            eWeaponType.WEAPON_SHOTGUN
        };

        private static eWeaponType[] ArmouredWeaponList = {
            eWeaponType.WEAPON_AK47,
            eWeaponType.WEAPON_M4,
            eWeaponType.WEAPON_MP5,
            eWeaponType.WEAPON_MICRO_UZI
        };

        private static eWeaponType[] Handguns = {
            eWeaponType.WEAPON_DEAGLE,
            eWeaponType.WEAPON_PISTOL
        };



