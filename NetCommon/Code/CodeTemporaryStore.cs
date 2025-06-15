using NetCommon.Buffers;
using NetCommon.Buffers.Dynamic;
using NetCommon.Code.Classes;

namespace NetCommon.Code;

public readonly ref struct CodeTemporaryStore
{
   public readonly DynamicSpanStore<ClassDeclaration> Classes;
   
   public CodeTemporaryStore(Span<byte> buffer)
   {
      Classes = new DynamicSpanStore<ClassDeclaration>(buffer);
   }

   public void GetAllocationStats(out StoreAllocationStats stats)
   {
      stats = new StoreAllocationStats()
      {
         Classes = new StoreAllocationStat(Classes.HasHeapAllocation, Classes.Capacity)
      };
   }
   
   public struct StoreAllocationStats
   {
      public StoreAllocationStat Classes;

      public override string ToString()
      {
         var codeWriter = new CodeTextWriter(
            stackalloc char[512],
            stackalloc char[2]);
         
         codeWriter.WriteLine($"Classes: {Classes.ToString()}");
         
         return codeWriter.ToString();
      }
   }

   public readonly struct StoreAllocationStat
   {
      public readonly bool IsHeapAllocated;
      public readonly int Capacity;

      public StoreAllocationStat(bool isHeapAllocated, int capacity)
      {
         IsHeapAllocated = isHeapAllocated;
         Capacity = capacity;
      }

      public override string ToString()
      {
         return $"[Heap: {IsHeapAllocated}, Capacity: {Capacity}]";
      }
   }
}