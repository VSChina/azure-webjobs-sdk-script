rmdir Generated /s /q
mkdir Generated
..\..\..\..\packages\Grpc.Tools.1.3.0\tools\windows_x86\protoc.exe --csharp_out Generated --grpc_out Generated RpcInvokeFunction.proto --plugin=protoc-gen-grpc=..\..\..\..\packages\Grpc.Tools.1.3.0\tools\windows_x86\grpc_csharp_plugin.exe