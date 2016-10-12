using System;
using UnityEngine;

public interface PlayerCommand : Command<Player>{}

public class ShootCommand : PlayerCommand
{
    public void Execute(Player t)
    {
        t.WeaponManager.Weapon.Shoot();
    }
}

public class NextWeaponCommand : PlayerCommand
{
    public void Execute(Player t)
    {
        t.WeaponManager.nextWeapon();
    }
}

public class JumpCommand : PlayerCommand
{
    public void Execute(Player t)
    {
        t.Jump();
    }
}
