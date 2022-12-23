namespace CpuEmulator.Core.AddressingModes;

internal interface IAddressingMode
{
    internal ushort Execute();
}