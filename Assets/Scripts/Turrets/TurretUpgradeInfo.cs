using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class TurretLevelInfo
{
    public TurretTypes type;
    public TurretField field;
    public int level;
    public float value;

    public TurretLevelInfo(TurretTypes type, TurretField field, int level, float value)
    {
        this.type = type;
        this.field = field;
        this.level = level;
        this.value = value;
    }
}

public enum TurretTypes
{
    MachineGun,
    TarSlinger,
    FlameThrower,
    RocketLauncher
}

public enum TurretField
{
    RangeRadius,
    Damage,
    RotationSpeed,
    ShotDelay,
    CoolDown,
	Cost
}

public static class TurretUpgradeInfo
{
    private static List<TurretLevelInfo> TurretLevelData = new List<TurretLevelInfo>()
    {
        // Machine Gun
		//Primary Trait
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.Damage, 0, 0.7f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.Damage, 1, 0.85f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.Damage, 2, 1.15f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.Damage, 3, 1.5f),
		//Secondary Trait
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.RotationSpeed, 0, 1.0f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.RotationSpeed, 1, 1.03f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.RotationSpeed, 2, 1.09f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.RotationSpeed, 3, 1.18f),
		//Secondary Trait
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.ShotDelay, 0, 0.1f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.ShotDelay, 1, 0.095f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.ShotDelay, 2, 0.088f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.ShotDelay, 3, 0.075f),

        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.CoolDown, 0, 1.2f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.CoolDown, 1, 1.2f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.CoolDown, 2, 1.2f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.CoolDown, 3, 1.2f),

        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.RangeRadius, 0, 3f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.RangeRadius, 1, 3f),
        new TurretLevelInfo(TurretTypes.MachineGun, TurretField.RangeRadius, 2, 3f),
		new TurretLevelInfo(TurretTypes.MachineGun, TurretField.RangeRadius, 3, 3f),

		new TurretLevelInfo(TurretTypes.MachineGun, TurretField.Cost, 0, 5f),
		new TurretLevelInfo(TurretTypes.MachineGun, TurretField.Cost, 1, 13f),
		new TurretLevelInfo(TurretTypes.MachineGun, TurretField.Cost, 2, 21f),
		new TurretLevelInfo(TurretTypes.MachineGun, TurretField.Cost, 3, 29f),

        // Tar Slinger
		//Secondary Trait
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.Damage, 0, 1.0f),	//tar slinger damage = splat size
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.Damage, 1, 1.1f),
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.Damage, 2, 1.25f),
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.Damage, 3, 1.5f),
		//Primary Trait
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.RotationSpeed, 0, 0.8f),
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.RotationSpeed, 1, 0.9f),
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.RotationSpeed, 2, 1.05f),
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.RotationSpeed, 3, 1.25f),
		//Primary Trait
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.ShotDelay, 0, 3.0f),
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.ShotDelay, 1, 2.8f),
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.ShotDelay, 2, 2.5f),
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.ShotDelay, 3, 2.0f),

        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.CoolDown, 0, 0),
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.CoolDown, 1, 0),
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.CoolDown, 2, 0),
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.CoolDown, 3, 0),

        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.RangeRadius, 0, 3.5f),
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.RangeRadius, 1, 3.5f),
        new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.RangeRadius, 2, 3.5f),
		new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.RangeRadius, 3, 3.5f),

		new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.Cost, 0, 5f),
		new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.Cost, 1, 12f),
		new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.Cost, 2, 20f),
		new TurretLevelInfo(TurretTypes.TarSlinger, TurretField.Cost, 3, 30f),

        // Flame Thrower
		//Primary Trait
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.Damage, 0, 3f),	//flame thrower dammage = flame duration
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.Damage, 1, 4.5f),
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.Damage, 2, 7f),
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.Damage, 3, 10.5f),

        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.RotationSpeed, 0, 1.5f),
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.RotationSpeed, 1, 1.5f),
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.RotationSpeed, 2, 1.5f),
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.RotationSpeed, 3, 1.5f),

        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.ShotDelay, 0, 0.1f),
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.ShotDelay, 1, 0.1f),
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.ShotDelay, 2, 0.1f),
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.ShotDelay, 3, 0.1f),

        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.CoolDown, 0, 0),
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.CoolDown, 1, 0),
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.CoolDown, 2, 0),
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.CoolDown, 3, 0),
		//Secondary Trait
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.RangeRadius, 0, 2.5f),
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.RangeRadius, 1, 2.7f),
        new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.RangeRadius, 2, 3.0f),
		new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.RangeRadius, 3, 3.5f),

		new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.Cost, 0, 8f),
		new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.Cost, 1, 15f),
		new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.Cost, 2, 24f),
		new TurretLevelInfo(TurretTypes.FlameThrower, TurretField.Cost, 3, 34f),

        // Rocket Launcher
		//Secondary Trait
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.Damage, 0, 20.0f),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.Damage, 1, 22.0f),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.Damage, 2, 25.0f),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.Damage, 3, 30.0f),

        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.RotationSpeed, 0, 1.0f),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.RotationSpeed, 1, 1.0f),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.RotationSpeed, 2, 1.0f),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.RotationSpeed, 3, 1.0f),

        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.ShotDelay, 0, 2.75f),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.ShotDelay, 1, 2.75f),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.ShotDelay, 2, 2.75f),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.ShotDelay, 3, 2.75f),

        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.CoolDown, 0, 0),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.CoolDown, 1, 0),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.CoolDown, 2, 0),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.CoolDown, 3, 0),
		//Primary Trait
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.RangeRadius, 0, 4.0f),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.RangeRadius, 1, 4.2f),
        new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.RangeRadius, 2, 4.5f),
		new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.RangeRadius, 3, 5.0f),

		new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.Cost, 0, 10f),
		new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.Cost, 1, 16f),
		new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.Cost, 2, 24f),
		new TurretLevelInfo(TurretTypes.RocketLauncher, TurretField.Cost, 3, 35f)
    };

    /// <summary>
    /// Fetches the value a turret type will have at a given level.  Used for help screens.
    /// </summary>
    /// <param name="turretType">Turret type to fetch.</param>
    /// <param name="turretField">Turret field type to fetch.</param>
    /// <param name="turretLevel">Turret level.</param>
    /// <returns>Returns value of TurretField for a givent TurretType at a particular level.</returns>
    public static float GetData(TurretTypes turretType, TurretField turretField, int turretLevel)
    {
        var collection = from tli in TurretLevelData
                         where tli.type == turretType
                         where tli.level == turretLevel
                         where tli.field == turretField
                         select tli;
        TurretLevelInfo found = collection.First();
        return found.value;
    }

    /// <summary>
    /// Fetches field for a particular turret. Used for most game play.
    /// </summary>
    /// <param name="turret">Turret instance.</param>
    /// <param name="field">TurretField to fetch.</param>
    /// <returns>Returns the value for a TuretField on a turret instance.</returns>
    public static float GetData(Turret turret, TurretField field)
    {
        var collection = from tli in TurretLevelData
                         where tli.type == turret.type
                         where tli.level == turret.Level
                         where tli.field == field
                         select tli;
        TurretLevelInfo found = collection.First();
        return found.value;
    }
}
