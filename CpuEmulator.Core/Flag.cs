﻿namespace CpuEmulator.Core;

public enum Flag
{
    C = 1 << 0,
    Z = 1 << 1,
    I = 1 << 2,
    D = 1 << 3,
    B = 1 << 4,
    R = 1 << 5,
    V = 1 << 6,
    N = 1 << 7
}