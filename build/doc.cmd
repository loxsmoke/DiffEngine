dotnet tool install -g loxsmoke.mddox
pushd ..\src\DiffEngine
dotnet publish -o publish
pushd publish
mddox DiffEngine.dll -o ..\..\..\DiffEngine.md
popd
popd
