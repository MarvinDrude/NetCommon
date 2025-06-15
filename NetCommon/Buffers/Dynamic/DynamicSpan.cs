namespace NetCommon.Buffers.Dynamic;

public ref struct DynamicSpan<T>
   where T : struct
{
   private readonly Span<byte> _buffer;
   
   public DynamicSpan(
      Span<byte> buffer)
   {
      _buffer = buffer;
   }
}