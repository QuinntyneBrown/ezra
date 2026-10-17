using System.CommandLine;

var root = new RootCommand("Ezra: a modular .NET command-line tool.");
return await root.Parse(args).InvokeAsync();
