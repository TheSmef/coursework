using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace KR.Web.Pages
{
    public partial class SuccessRestoreDialog
    {
        [Inject]
        private DialogService DialogService { get; set; }
        protected async Task Close(MouseEventArgs? args)
        {
            DialogService.Close(null);
        }
    }
}