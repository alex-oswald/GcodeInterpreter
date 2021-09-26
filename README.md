# G-code Interpreter

[![Nuget](https://img.shields.io/nuget/v/GcodeInterpreter)](https://www.nuget.org/packages/GcodeInterpreter/)

Simple .NET G-code interpreter


The G-code interpreter will read a G-code file and produce a `GcodeProgram`
from it.

### Dependency injection setup

Add services to DI

```csharp
services.AddGcodeInterpreter();
```

### Using the interpreter

Inject `IGcodeInterpreter` into a class and use it to produce a `GcodeProgram`

```csharp
public class Class1
{
    private readonly IGcodeInterpreter _interpreter;

    public Class1(IGcodeInterpreter interpreter)
    {
        _interpreter = interpreter;
    }

    public async Task<GcodeProgram> Build(string path)
    {
        return await _interpreter.ParseAsync(path);
    }
}
```

A produced `GcodeProgram` contains a `Lines` property that contains the parsed lines of the G-code file.
Each `Line` contains a `Command` and `Parameters` property. A `Command` is of type `Field` and the `Parameters`
is of type `List<Field>`. A `Field` contains a `FieldLetter` and `Code` property that represents a G-code
command or parameter.


```csharp
// Loop through the G-code lines while printing each
// lines command
foreach (var line in program.Lines)
{
    Console.WriteLine(line.Command.ToString());
}
```

```csharp
// Print a line as it was in the G-code file
var line = program.Lines[0].ToString();
//M190 S60.000000
```

```csharp
// Run the program somehow
foreach (var line in program.Lines)
{
    switch (line.Command.FieldLetter.Letter)
    {
        case 'G':
            DoSomething(line.Command.Code, line.Parameters);
            break;
        case 'M':
            DoSomething(line.Command.Code, line.Parameters);
            break;
    }
}
```


### References

https://reprap.org/wiki/G-code