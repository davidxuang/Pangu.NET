# Pangu.NET

Paranoid text spacing for good readability, to automatically insert whitespace between CJK (Chinese, Japanese, Korean) and half-width characters (alphabetical letters, numerical digits and symbols).

## NuGet Package

[Pangu.NET on NuGet.org](https://www.nuget.org/packages/Pangu.NET)

## Usage

```csharp
using Pangu;

var text = "当你凝视着bug，bug也凝视着你";
var newText = Pangu.SpacingText(text); // 当你凝视着 bug，bug 也凝视着你
Pangu.SpacingText(ref text);
```

```csharp
using Pangu.Extensions;

var text = "与PM战斗的人，应当小心自己不要成为PM".SpacingText(); // 与 PM 战斗的人，应当小心自己不要成为 PM
```
