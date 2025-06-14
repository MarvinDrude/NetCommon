using System.Buffers;

namespace NetCommon.Buffers;

public static class ArrayPoolAllocator<T>
{
   private static readonly ArrayPool<T> _pool = ArrayPool<T>.Shared;

   public static void Create(int length, out ArrayPoolAllocation<T> allocation)
   {
      allocation = new ArrayPoolAllocation<T>(_pool, length);
   }
}