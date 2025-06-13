
using NetCommon.Buffers;

var writer = new TextWriterSlim(stackalloc char[2]);

writer.WriteText("hallo");


_ = "";