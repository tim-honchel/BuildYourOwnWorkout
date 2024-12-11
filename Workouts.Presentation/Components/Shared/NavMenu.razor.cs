using System.Runtime.CompilerServices;
using Workouts.Presentation.Services;

[assembly: InternalsVisibleTo("Workouts.UnitTests")]
namespace Workouts.Presentation.Components.Shared
{
    public partial class NavMenu
    {
        private IApiCallService _service;
        public NavMenu(IApiCallService service)
        {
            _service = service;
        }
        protected override async Task OnInitializedAsync()
        {
            LoadUserInfo();
            await base.OnInitializedAsync();
        }

        internal void LoadUserInfo()
        {
        }
    }
}