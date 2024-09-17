using Microsoft.JSInterop;

namespace UmojaCampus.Client.Helpers
{
	public class JsInterop(IJSRuntime jsRuntme)
	{
		private readonly IJSRuntime _jsRuntime = jsRuntme;
		public async Task ShowModal(string elementId)
		{
			await _jsRuntime.InvokeVoidAsync("showBootstrapModal", elementId);
		}

		public async Task HideModal(string elementId)
		{
			await _jsRuntime.InvokeVoidAsync("hideBootstrapModal", elementId);
		}
	}
}
