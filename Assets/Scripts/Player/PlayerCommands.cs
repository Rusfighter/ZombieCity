using System;
using UnityEngine;

public interface PlayerCommand : Command<Player>{}

public class ShootCommand : PlayerCommand
{
    public void Execute(Player t)
    {
        t.m_WeaponHandler.Weapon.Shoot();
    }
}

public class NextWeaponCommand : PlayerCommand
{
    public void Execute(Player t)
    {
        t.m_WeaponHandler.nextWeapon();
    }
}

public class JumpCommand : PlayerCommand
{
    public void Execute(Player t)
    {
        t.Jump();
    }
}
