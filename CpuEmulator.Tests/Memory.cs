using CpuEmulator.Core;

namespace CpuEmulator.Tests;

public class Memory : IMemory
{
    public byte[] Ram { get; set; } = new byte[65536];

    public byte ReadByte(ushort address)
    {
        return Ram[address];
    }

    public void WriteByte(ushort address, byte value)
    {
        Ram[address] = value;
    }
}