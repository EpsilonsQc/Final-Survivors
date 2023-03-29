namespace Final_Survivors.Core
{
    public enum Events 
    { 
        INIT, RESET, END_GAME, RESET_ENEMY,
        IDLE, MOVE, DASH, SHOOT, MELEE, TBAG, DASH_PLUS, DASH_MINUS,
        TIME_WARP, TIME_WARP_DAY_TO_NIGHT, TIME_WARP_NIGHT_TO_DAY, TIME_WARP_UPDATE, INSIDE_FLASHLIGHT, OUTSIDE_FLASHLIGHT,
        TAKE_DAMAGE, RETURN_BULLET, RELOAD, AMMO, SHIELD, HEALTH,
        CRIT_DMG, LOW_DMG, MISS, NORMAL_DMG,
        SCORE, SPAWNNORMAL, SPAWNELITE, SPAWNBOSS, NORMAL, ELITE, BOSS, NEXTSTAGE,
        PISTOL, MACHINE_GUN, SHOTGUN, SNIPER, SWORD, ROCKET_LAUNCHER,
        PAUSE_SPAWN, PLAY_SPAWN,
        SECRET_ROOM_IN, SECRET_ROOM_OUT, SPECIALCRATE_PICKUP
    }
}
