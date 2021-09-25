# G-code Interpreter

Simple .NET G-code interpreter


The G-code interpreter will read a G-code file and construct a `GcodeProgram`
from it.

### Dependency injection setup

Add services to DI

```csharp
services.AddGcodeInterpreter();
```

### Using the interpreter

Inject `IGcodeInterpreter` into a class and use it to generate a `GcodeProgram`

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


### References

https://reprap.org/wiki/G-code#Comments