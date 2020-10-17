[![NuGet](https://img.shields.io/nuget/v/TesserNet.svg)](https://www.nuget.org/packages/TesserNet/)  

# TesserNet
TesserNet provides high level bindings for Tesseract in .NET.
The library comes with all required native libraries and a trained English model, meaning you don't need any additional setup to get the library up and running!
Additionally, the library provides a simple Tesseract instance pooling system (through the `TesseractPool` class) so you can carelessly make asynchronous OCR invocations.

## Limitations
Currently only Windows x64 and x86 are supported. I plan on adding support for unix systems in the future.

## Downloads
[TesserNet](https://www.nuget.org/packages/TesserNet/)
[TesserNet for System.Drawing](https://www.nuget.org/packages/TesserNet.System.Drawing/)
[TesserNet for ImageSharp](https://www.nuget.org/packages/TesserNet.ImageSharp/)

## Usage
There are a few example project available for you to try out in the `src` directory.
Note that the `TesserNet.Example.System.Drawing` example uses .NET Framework,
meaning it will only run on Windows.

To start off, one first needs to add the following import:
```cs
using TesserNet;
```

One can then create a `Tesseract` instace:
```cs
Tesseract tesseract = new Tesseract();
```

With that instance one can now perform OCR.
```cs
string result = tesseract.Read(...);
```

By default, the following `Read` methods are provided:
```cs
string Read(byte[] data, int width, int height, int bytesPerPixel);
string Read(byte[] data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);
Task<string> ReadAsync(byte[] data, int width, int height, int bytesPerPixel);
Task<string> ReadAsync(byte[] data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);
```

Additionally, if one prefers to use System.Drawing or ImageSharp, it is possible to also add a dependency to
[TesserNet.System.Drawing](https://www.nuget.org/packages/TesserNet.System.Drawing/) or
[TesserNet.ImageSharp](https://www.nuget.org/packages/TesserNet.ImageSharp/) respectively.
Adding either of these dependencies adds the following `Read` methods:
```cs
string Read(Image image);
string Read(Image image, Rectangle rectangle);
Task<string> ReadAsync(Image image);
Task<string> ReadAsync(Image image, Rectangle rectangle);
```

Furthermore, when trying to use concurrency, it might be useful to have a look at the `TesseractPool` class:
```cs
TesseractPool pool = new TesseractPool();
```

The `TesseractPool` class provides a pooling mechanism for running the OCR on multiple `Tesseract` instances, without having to manually deal with all the different instances.
The class has the following methods:
```cs
Task<string> ReadAsync(byte[] data, int width, int height, int bytesPerPixel);
Task<string> ReadAsync(byte[] data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);
```

And when either of the aforementioned image processing bridging libraries are present:
```cs
Task<string> ReadAsync(Image image);
Task<string> ReadAsync(Image image, Rectangle rectangle);
```
