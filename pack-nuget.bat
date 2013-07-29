%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe src\PostmarkDotNet.sln /t:Clean,Rebuild /p:Configuration=Release /fileLogger
copy LICENSE bin
NuGet.exe pack postmark.nuspec -Basepath bin