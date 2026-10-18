using Ezra;
using Ezra.Abstractions;

IEzraModule[] modules = [];

return await new EzraApp(modules).RunAsync(args, Console.Out, Console.Error);
