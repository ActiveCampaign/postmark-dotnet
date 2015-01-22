NuGet.exe pack ./postmark.nuspec

mkdir sn-build
cd sn-build
mkdir PCL
mkdir Convenience
cd ..

copy src\Postmark.PCL\bin\Release\*.* sn-build\PCL
copy src\Postmark.Convenience\bin\Release\*.* sn-build\Convenience

build-bin\ildasm.exe sn-build\PCL\Postmark.dll /out=pcl.il
rm sn-build\PCL\Postmark.dll
%windir%\Microsoft.NET\Framework\v4.0.30319\ilasm.exe pcl.il /res:pcl.res /dll /key:src\Postmark.PCL\key.snk /out:sn-build\PCL\Postmark.dll

copy sn-build\PCL\Postmark.dll sn-build\Convenience\Postmark.dll

build-bin\ildasm.exe sn-build\Convenience\Postmark.Convenience.dll /out=Convenience.il
rm ./sn-build/Convenience/Postmark.Convenience.dll
%windir%\Microsoft.NET\Framework\v4.0.30319\ilasm.exe Convenience.il /res:Convenience.res /dll /key:src\Postmark.PCL\key.snk /out:sn-build\Convenience\Postmark.Convenience.dll

NuGet.exe pack ./postmark-strong.nuspec