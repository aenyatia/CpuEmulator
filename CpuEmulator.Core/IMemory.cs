namespace CpuEmulator.Core;

public interface IMemory
{
    public byte ReadByte(ushort address);
    public void WriteByte(ushort address, byte value);
}