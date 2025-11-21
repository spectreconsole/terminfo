namespace TermInfo;

using System;
using System.IO;

internal sealed class ByteReader
{
    private readonly BinaryReader _reader;

    public int Position { get; set; }

    public ByteReader(Stream stream)
    {
        _reader = new BinaryReader(stream);
    }

    public bool Eof()
    {
        return _reader.PeekChar() == -1;
    }

    public int ReadByte()
    {
        var result = _reader.Read();
        if (result != -1)
        {
            Position++;
        }

        return result;
    }

    public byte[] ReadBytes(int count)
    {
        var buffer = new byte[count];
        var read = Read(buffer, count);
        if (read != count)
        {
            throw new InvalidOperationException("Could not read the requested number of bytes.");
        }

        return buffer;
    }

    public int Read(byte[] buffer, int count)
    {
        var result = _reader.Read(buffer, 0, count);
        Position += result;
        return result;
    }
}