using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using UmojaCampus.Shared.DTO.Inputs;
using UmojaCampus.Shared.DTO.Outputs;

namespace UmojaCampus.Client.Pages.Qualification
{
	public partial class SubmitQualification
	{
		[Parameter]
		public Guid? Id { get; set; }

		[Inject]
		private NavigationManager Navigation { get; set; }

		private SaveQualificationDto Model = new();
		private string Title;
		private string ButtonLabel;
		private bool IsLoading = false;
		private bool ApiError = false;
		private string ErrorMessage { get; set; } = string.Empty;

		protected override async Task OnParametersSetAsync()
		{
			// Initialize or fetch data based on the Id value
			if (Id.HasValue)
			{
				var id = Id.Value;
				var httpClient = await _httpClient.GetPrivateHttpClient();
				var result = await httpClient.GetAsync($"api/qualifications/qualification/{id}");

				if (result.IsSuccessStatusCode)
				{
					var data = await result.Content.ReadFromJsonAsync<QualificationDto>();
					Model = new SaveQualificationDto
					{
						Name = data.Name,
						Description = data.Description,
						ToDate = data.ToDate,
						FromDate = data.FromDate,
						Fees = data.Fees,
						TotalCredit = data.TotalCredit,
					};

					Title = "Edit Qualification";
					ButtonLabel = "Update";
				}
				else
				{
					ErrorMessage = result.ReasonPhrase;
					ApiError = true;
				}

				StateHasChanged();
				IsLoading = false;
			}
			else
			{
				Title = "Add Qualification";
				ButtonLabel = "Save";
			}
		}
		private async Task HandleSubmit()
		{
			IsLoading = true;

			if(Id == null)
			{
				var httpClient = await _httpClient.GetPrivateHttpClient();
				var result = await httpClient.PostAsJsonAsync("api/qualifications/create", Model);

				if (result.IsSuccessStatusCode)
				{
					Navigation.NavigateTo("/qualifications");
				}
				else
				{
					ErrorMessage = result.ReasonPhrase;
					ApiError = true;
				}
			}
			else
			{
				var httpClient = await _httpClient.GetPrivateHttpClient();
				var result = await httpClient.PutAsJsonAsync($"api/qualifications/update/{Id.Value}", Model);

				if (result.IsSuccessStatusCode)
				{
					Navigation.NavigateTo("/qualifications");
				}
				else
				{
					ErrorMessage = result.ReasonPhrase;
					ApiError = true;
				}
			}
			

			StateHasChanged();
			IsLoading = false;
		}
	}
}