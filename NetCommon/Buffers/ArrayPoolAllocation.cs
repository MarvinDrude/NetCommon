using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetCommon.Buffers;

[StructLayout(LayoutKind.Auto)]
public readonly struct ArrayPoolAllocation<T> : IMemoryOwner<T>
{
   private readonly ArrayPool<T> _pool;
   private readonly T[] _memory;

   public int Length { get; }
   
   public ArrayPoolAllocation(ArrayPool<T> pool, int length)
   {
      _pool = pool;
      _memory = pool.Rent(length);

      Length = length;
   }

   public Span<T> Span => _memory.AsSpan(0, Length);
   
   public Memory<T> Memory => _memory.AsMemory(0, Length);

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void CopyTo(ArrayPoolAllocation<T> other)
   {
      Span.CopyTo(other.Span);
   }
   
   public void Dispose()
   {
      _pool.Return(_memory);
   }
}