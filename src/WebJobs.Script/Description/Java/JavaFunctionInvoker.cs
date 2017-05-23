// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

using Grpc.Core;
using Microsoft.Azure.WebJobs.Script.Binding;
using Microsoft.Azure.WebJobs.Script.Rpc.Messages;

namespace Microsoft.Azure.WebJobs.Script.Description
{
    public class JavaFunctionInvoker : FunctionInvokerBase
    {
        private readonly BindingMetadata _trigger;
        private readonly FunctionMetadata _function;

        // private readonly ReadOnlyCollection<FunctionBinding> _inputBindings;
        private readonly ReadOnlyCollection<FunctionBinding> _outputBindings;

        internal JavaFunctionInvoker(
            ScriptHost host,
            BindingMetadata triggerMetadata,
            FunctionMetadata functionMetadata,
            /*Collection<FunctionBinding> inputBindings,*/
            Collection<FunctionBinding> outputBindings,
            ITraceWriterFactory traceWriterFactory = null)
            : base(host, functionMetadata, traceWriterFactory)
        {
            _trigger = triggerMetadata;
            _function = functionMetadata;

            // _inputBindings = new ReadOnlyCollection<FunctionBinding>(inputBindings);
            _outputBindings = new ReadOnlyCollection<FunctionBinding>(outputBindings);
        }

        protected override async Task InvokeCore(object[] parameters, FunctionInvocationContext context)
        {
            var input = parameters[0] as HttpRequestMessage;
            var invocationId = context.ExecutionContext.InvocationId.ToString();
            var jarPath = _function.ScriptFile;
            var method = _function.EntryPoint;
            var dataType = _trigger.DataType ?? DataType.String;

            var channel = new Channel("localhost:50051", ChannelCredentials.Insecure);
            var client = new RpcFunction.RpcFunctionClient(channel);
            var request = new RpcFunctionInvokeMetadata
            {
                InvocationId = invocationId,
                ScriptFile = jarPath,
                EntryPoint = method,
                InputType = RpcDataType.String,
                OutputType = RpcDataType.String,
                InputValue = new RpcDataValue { StringValue = await input.Content.ReadAsStringAsync() }
            };
            var response = await client.RpcInvokeFunctionAsync(request);
            await channel.ShutdownAsync();

            var binder = context.Binder;
            foreach (var binding in _outputBindings)
            {
                await binding.BindAsync(new BindingContext
                {
                    TriggerValue = input,
                    Binder = binder,
                    BindingData = binder.BindingData,
                    Value = response.OutputValue.StringValue
                });
            }
        }
    }
}
