namespace NetCommon.Buffers.Dynamic;

public interface ISerializableStruct<T>
   where T : allows ref struct
{
   static abstract void WriteTo(scoped Span<byte> buffer, scoped in T instance);
   static abstract void ReadFrom(ReadOnlySpan<byte> buffer, out T instance);
   
   static abstract int GetSerializedSize(scoped in T instance);
}