using CpuEmulator.Core;
using FluentAssertions;
using Xunit;

namespace CpuEmulator.Tests;

public class CpuTests
{
    [Fact]
    public void Test()
    {
        // arrange
        var memory = new Memory();
        memory.WriteByte(0xFFFC, 0x11);
        memory.WriteByte(0xFFFD, 0x22);
        
        var cpu = new Cpu(memory);

        // act
        cpu.Reset();

        // assert
        cpu.A.Should().Be(0x00);
        cpu.X.Should().Be(0x00);
        cpu.Y.Should().Be(0x00);

        cpu.SP.Should().Be(0xFD);
        cpu.SR.Should().Be(0x00);

        cpu.PC.Should().Be(0x2211);
    }
}