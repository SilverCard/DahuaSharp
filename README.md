# DahuaSharp
## [EN]
Small C# library to access Dahua DVRs, it also works with Intelbras DVRs.
Currently, it's only possible to get the channels names and make a JPG snapshot of the recording.

## [PT-BR]
Biblioteca em C# para acessar DVRs da Dahua, que são vendidos no Brasil como Intelbras.
Atualmente, só é possível pegar os nomes dos canais e fazer uma captura em JPG da gravação.

Quick Example:
```csharp
var c = new DvrClient("192.168.13.37");
c.Connect();
c.Login("admin", "admin");
String[] channels = c.GetChannelNames();
byte[] b = c.Snapshot(0);
```


Inspired by TaniDvr (http://tanidvr.sourceforge.net)
