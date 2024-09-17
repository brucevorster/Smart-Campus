using UmojaCampus.Shared.DTO.Outputs;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.QuickGrid;
using UmojaCampus.Shared.Resources;
namespace UmojaCampus.Client.Pages.Qualification
{
    public partial class QualificationList
    {
        private const string ModalId = "deleteModal";

		private Guid SelectedRecordId;
        private string SelectedRecordName;

		private bool IsLoading = false;
        private bool ApiError = false;
        private string ErrorMessage { get; set; } = string.Empty;
        protected async override Task OnInitializedAsync()
        {
            await GetData();
        }
        private async Task GetData()
		{
            IsLoading = true;

			var httpClient = await _httpClient.GetPrivateHttpClient();
			var result = await httpClient.GetAsync("api/qualifications");

            if(result.IsSuccessStatusCode)
            {
				var data = await result.Content.ReadFromJsonAsync<List<QualificationDto>>();
                Items = data.OrderBy(x => x.Name).AsQueryable();
            }
            else
            {
                ErrorMessage = result.ReasonPhrase;
                ApiError = true;
            }

            IsLoading = false;
        }

        public string SearchTerm { get; set; } = string.Empty;

        void OnSearchChanged(string search)
        {
            SearchTerm = search;
            StateHasChanged();
        }
        IQueryable<QualificationDto> Items;
        readonly PaginationState Pagination = new() { ItemsPerPage = 10 };
        IQueryable<QualificationDto> FilteredItems =>
            Items?.Where(x => x.Name.Contains(SearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                         x.Description.Contains(SearchTerm, StringComparison.CurrentCultureIgnoreCase));

        readonly GridSort<QualificationDto> NameSort = GridSort<QualificationDto>.ByDescending(x => x.Name);

		private async Task ShowDeleteModal(Guid recordId, string recordName)
		{
			SelectedRecordId = recordId;
            SelectedRecordName = recordName;
			await JsInterop.ShowModal(ModalId);
		}

        private string AlertMessage;
        private bool AlertVisible;
        private async Task DeleteRecord(Guid recordId)
		{
            AlertVisible = false;

			var httpClient = await _httpClient.GetPrivateHttpClient();
			var result = await httpClient.DeleteAsync($"api/qualifications/delete/{recordId}");

            if (result.IsSuccessStatusCode)
            {
                Items = Items.Where(x => x.Id != recordId);
                await JsInterop.HideModal(ModalId);
                AlertVisible = false;
            }
            else
            {
				AlertMessage = ErrorResource.DeleteError;
				AlertVisible = true;
				await JsInterop.HideModal(ModalId);
			}
        }
	}
}