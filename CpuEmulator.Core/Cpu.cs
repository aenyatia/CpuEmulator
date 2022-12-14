namespace CpuEmulator.Core;

public class Cpu
{
    private readonly IMemory _memory;

    private const ushort NmiVectorL = 0xFFFA;
    private const ushort NmiVectorH = 0xFFFB;
    private const ushort RstVectorL = 0xFFFC;
    private const ushort RstVectorH = 0xFFFD;
    private const ushort IrqVectorL = 0xFFFE;
    private const ushort IrqVectorH = 0xFFFF;

    private const byte StackTop = 0xFF;
    private const byte StackBot = 0x00;

    public byte A { get; private set; }
    public byte X { get; private set; }
    public byte Y { get; private set; }
    public byte SR { get; private set; }
    public byte SP { get; private set; }
    public ushort PC { get; private set; }

    public Cpu(IMemory memory,
        byte a = 0x00,
        byte x = 0x00,
        byte y = 0x00,
        byte sr = 0x00,
        byte sp = 0x00,
        ushort pc = 0x0000)
    {
        _memory = memory;

        A = a;
        X = x;
        Y = y;

        SR = sr;
        SP = sp;
        PC = pc;
    }

    private byte ReadByte(ushort address)
    {
        return _memory.ReadByte(address);
    }

    private void WriteByte(ushort address, byte value)
    {
        _memory.WriteByte(address, value);
    }

    private void StackPush(byte value)
    {
        WriteByte((ushort)(0x0100 + SP), value);
        if (SP == StackBot) SP = StackTop;
        else SP--;
    }

    private byte StackPop()
    {
        if (SP == StackTop) SP = StackBot;
        else SP++;

        return ReadByte((ushort)(0x0100 + SP));
    }

    private void SetFlag(Flag flag, bool value)
    {
        if (value) SR |= (byte)flag;
        else SR &= (byte)~flag;
    }

    private bool GetFlag(Flag flag)
    {
        return (SR & (byte)flag) != 0x00;
    }
}