public static class GamePlayUtils
{
    public static Damage CalculateDamage(Damage input, DeffenseStats def)
    {
        switch (input.type)
        {
            case DamageEnum.Physical:
                {
                    float reduced = Configs.GamePlay.GetDmgRes(def.armor);
                    return new Damage(input.amount * (1 - reduced), input.type);
                }
            case DamageEnum.Magic:
                {
                    float reduced = Configs.GamePlay.GetDmgRes(def.magicRes);
                    return new Damage(input.amount * (1 - reduced), input.type);
                }
            case DamageEnum.True: return input;
            default: return input;
        }
    }
}