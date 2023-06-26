using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportCar : Car
{
    public SportCar() : base("RWD")
    {
        MotorHP = 1500f;
        FrenoP = 900000f;
    }
}
