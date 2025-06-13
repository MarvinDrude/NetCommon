using System.Buffers;

namespace NetCommon.Buffers;

public static class ArrayPoolAllocator<T>
{
   private static readonly ArrayPool<T> _pool = ArrayPool<T>.Shared;

   public static ArrayPoolAllocation<T> Create(int length)
   {
      return new ArrayPoolAllocation<T>(_pool, length);
   }
}