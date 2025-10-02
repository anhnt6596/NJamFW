using System;
using System.Collections.Generic;
[Serializable]
public class UnitStatus
{
    public UnitStatus(UnitStatusEnum type, float pr1)
    {
        this.type = type;
        @params = new List<float>() { pr1 };
    }

    public UnitStatus(UnitStatusEnum type, float pr1, float pr2)
    {
        this.type = type;
        @params = new List<float>() { pr1, pr2 };
    }

    public UnitStatusEnum type;
    public List<float> @params;
}