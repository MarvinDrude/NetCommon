namespace NetCommon.Buffers.Dynamic;

public ref struct DynamicSpanStore<T>
   where T : struct, ISerializableStruct<T>, allows ref struct
{
   public bool HasHeapAllocation => _poolAllocation.Length > 0;
   public int Capacity => _span.Capacity;
   
   public DynamicSpan<T> DynamicSpan => _span;
   
   private DynamicSpan<T> _span;
   private ArrayPoolAllocation<byte> _poolAllocation;
   
   public DynamicSpanStore(Span<byte> buffer)
   {
      _span = new DynamicSpan<T>(buffer);
   }

   public void Add(scoped in T item)
   {
      if (_span.Add(item))
      {
         return;
      }

      var requiredSize = T.GetSerializedSize(item);
      var newSize = Math.Max(requiredSize, _span.Capacity * 2);

      var before = _poolAllocation;
      
      ArrayPoolAllocator<byte>.Create(newSize, out _poolAllocation);
      _span.WrittenSpan.CopyTo(_poolAllocation.Span);

      if (before.Length > 0)
      {
         before.Dispose();
      }
      
      _span = new DynamicSpan<T>(_poolAllocation.Span, _span.Position, _span.Count);

      Add(item);
   }

   public bool RemoveAt(int index)
   {
      return _span.RemoveAt(index);
   }
}