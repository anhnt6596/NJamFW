using System;

[Serializable]
public struct DeffenseStats
{
    public DeffenseStats(short armor, short magicRes)
    {
        this.armor = armor;
        this.magicRes = magicRes;
    }
    public short armor;
    public short magicRes;
}