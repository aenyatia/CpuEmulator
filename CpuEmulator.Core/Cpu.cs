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

    private byte _cycles;

    public byte A { get; internal set; }
    public byte X { get; internal set; }
    public byte Y { get; internal set; }
    public byte SR { get; internal set; }
    public byte SP { get; internal set; }
    public ushort PC { get; internal set; }

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

    public void Run()
    {
        if (_cycles == 0)
        {
            // fetch

            // decode

            // execute
        }

        _cycles--;
    }

    public void Reset()
    {
        var lowByte = ReadByte(RstVectorL);
        var highByte = ReadByte(RstVectorH);

        PC = CreateAddress(lowByte, highByte);

        A = 0x00;
        X = 0x00;
        Y = 0x00;

        SP = 0xFD;
        SR = 0x00;

        _cycles = 8;
    }

    public void Irq()
    {
        if (!GetFlag(Flag.I)) return;

        StackPush(HighByte(PC));
        StackPush(LowByte(PC));

        SetFlag(Flag.B, false);
        SetFlag(Flag.R, true);
        SetFlag(Flag.I, true);

        StackPush(SR);

        var lowByte = ReadByte(IrqVectorL);
        var highByte = ReadByte(IrqVectorH);

        PC = CreateAddress(lowByte, highByte);

        _cycles = 7;
    }

    public void Nmi()
    {
        StackPush(HighByte(PC));
        StackPush(LowByte(PC));

        SetFlag(Flag.B, false);
        SetFlag(Flag.R, true);
        SetFlag(Flag.I, true);

        StackPush(SR);

        var lowByte = ReadByte(NmiVectorL);
        var highByte = ReadByte(NmiVectorH);

        PC = CreateAddress(lowByte, highByte);

        _cycles = 8;
    }

    internal byte ReadByte(ushort address)
    {
        return _memory.ReadByte(address);
    }
    internal void WriteByte(ushort address, byte value)
    {
        _memory.WriteByte(address, value);
    }

    internal void StackPush(byte value)
    {
        WriteByte((ushort)(0x0100 + SP), value);
        if (SP == StackBot) SP = StackTop;
        else SP--;
    }
    internal byte StackPop()
    {
        if (SP == StackTop) SP = StackBot;
        else SP++;

        return ReadByte((ushort)(0x0100 + SP));
    }

    internal void SetFlag(Flag flag, bool value)
    {
        if (value) SR |= (byte)flag;
        else SR &= (byte)~flag;
    }
    internal bool GetFlag(Flag flag)
    {
        return (SR & (byte)flag) != 0x00;
    }

    private static ushort CreateAddress(byte lowByte, byte highByte)
    {
        return (ushort)((highByte << 8) | lowByte);
    }
    private static byte HighByte(ushort address)
    {
        return (byte)((address >> 8) & 0x00FF);
    }
    private static byte LowByte(ushort address)
    {
        return (byte)(address & 0x00FF);
    }
}