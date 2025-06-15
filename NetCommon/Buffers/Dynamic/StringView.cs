using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace NetCommon.Buffers.Dynamic;

public readonly ref struct StringView 
   : ISerializableStruct<StringView>
{
   public ReadOnlySpan<char> Span => _span;
   public readonly ReadOnlySpan<char> _span;
   
   public StringView(ReadOnlySpan<char> span)
   {
      _span = span;
   }

   public static int GetSerializedSize(scoped in StringView instance)
   {
      // char byte length + integer length header
      return instance._span.Length * 2 + 4;
   }
   
   public static void WriteTo(scoped Span<byte> buffer, scoped in StringView instance)
   {
      var byteLength = instance._span.Length * 2;
      BinaryPrimitives.WriteInt32BigEndian(buffer, byteLength);
      buffer = buffer[sizeof(int)..];

      instance._span.CopyTo(MemoryMarshal.Cast<byte, char>(buffer));
   }
   
   public static void ReadFrom(ReadOnlySpan<byte> buffer, out StringView instance)
   {
      var length = BinaryPrimitives.ReadInt32BigEndian(buffer);
      buffer = buffer.Slice(sizeof(int), length);

      var valueSpan = MemoryMarshal.Cast<byte, char>(buffer);
      instance = new StringView(valueSpan);
   }

   public override string ToString()
   {
      return _span.ToString();
   }
}