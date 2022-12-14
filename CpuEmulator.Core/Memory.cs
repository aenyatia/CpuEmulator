namespace CpuEmulator.Core;

public class Memory : IMemory
{
    private readonly byte[] _memory = new byte[65536];

    public byte ReadByte(ushort address)
    {
        return _memory[address];
    }

    public void WriteByte(ushort address, byte value)
    {
        _memory[address] = value;
    }
}