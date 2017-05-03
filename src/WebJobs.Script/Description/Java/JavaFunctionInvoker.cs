// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Script.Binding;

namespace Microsoft.Azure.WebJobs.Script.Description
{
    public class JavaFunctionInvoker : FunctionInvokerBase
    {
        // private readonly ReadOnlyCollection<FunctionBinding> _inputBindings;
        // private readonly ReadOnlyCollection<FunctionBinding> _outputBindings;

        internal JavaFunctionInvoker(
            ScriptHost host,
            FunctionMetadata functionMetadata,
            /*Collection<FunctionBinding> inputBindings,
            Collection<FunctionBinding> outputBindings,*/
            ITraceWriterFactory traceWriterFactory = null)
            : base(host, functionMetadata, traceWriterFactory)
        {
            // _inputBindings = new ReadOnlyCollection<FunctionBinding>(inputBindings);
            // _outputBindings = new ReadOnlyCollection<FunctionBinding>(outputBindings);
        }

        protected override Task InvokeCore(object[] parameters, FunctionInvocationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
