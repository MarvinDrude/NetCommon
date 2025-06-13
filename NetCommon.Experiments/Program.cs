
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using NetCommon.Buffers;
using NetCommon.Code.Modules;

var summary = BenchmarkRunner.Run<BenchmarkTest>();

// new BenchmarkTest().Run();
//
// _ = "";

[SimpleJob(RunStrategy.Throughput, iterationCount: 2)]
[MinColumn, MaxColumn, MeanColumn, MedianColumn, MemoryDiagnoser]
public class BenchmarkTest
{
   [Benchmark]
   public string Run()
   {
      var writer = new CodeTextWriter(
         stackalloc char[1024],
         stackalloc char[16]);

      var module = new NameSpaceModule(ref writer);
      module.EnableNullable(true);
      module.EnableNullable(true);

      writer.WriteText("hallo");
      writer.UpIndent();
      writer.WriteLine("asadsa");
      writer.UpIndent();
      writer.WriteLine("dsadsa");
      
      module.EnableNullable(true);
      module.EnableNullable(true);

      writer.WriteText("hallo");
      writer.UpIndent();
      writer.WriteLine("asadsa");
      writer.UpIndent();
      writer.WriteLine("dsadsa");
      
      return string.Empty;
   }
   
   [Benchmark]
   public string Run2()
   {
      var stringWriter = new StringBuilder();
      
      
      stringWriter.AppendLine("#nullable enable");
      stringWriter.AppendLine();
      stringWriter.AppendLine("#nullable enable");
      stringWriter.AppendLine();
      
      stringWriter.Append("hallo");
      stringWriter.Append("asadsa");
      stringWriter.AppendLine();
      stringWriter.AppendLine("\t\tdsadsa");
      stringWriter.AppendLine();

      stringWriter.AppendLine("#nullable enable");
      stringWriter.AppendLine();
      stringWriter.AppendLine("#nullable enable");
      stringWriter.AppendLine();
      
      stringWriter.Append("hallo");
      stringWriter.Append("asadsa");
      stringWriter.AppendLine();
      stringWriter.AppendLine("\t\tdsadsa");
      stringWriter.AppendLine();
      
      return string.Empty;
   }
}