using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using BlazorComponents.ChartJs.Model;

namespace BlazorComponents.ChartJs
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.
  
    public class ChartJsBlazorJSInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;
        private readonly Lazy<Task<IJSObjectReference>> moduleTask2;

        public ChartJsBlazorJSInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/BlazorComponents.ChartJs/ChartJsBlazor.js").AsTask());
        }

        public async ValueTask<bool> DisplayBarChart(Chart myChart)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<bool>("setBarChart", myChart);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
