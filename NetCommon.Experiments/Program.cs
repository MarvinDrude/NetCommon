
using NetCommon.Buffers;

var writer = new CodeTextWriter(
   stackalloc char[16],
   stackalloc char[16]);

writer.WriteText("hallo");
writer.UpIndent();
writer.WriteLine("asadsa");
writer.UpIndent();
writer.WriteLine("dsadsa");

_ = "";