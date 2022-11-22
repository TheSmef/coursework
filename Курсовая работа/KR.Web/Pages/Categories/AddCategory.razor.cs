using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;

namespace KR.Web.Pages.Categories
{
    public partial class AddCategory
    {
        [Inject]
        private CategoryService CategoryService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        private Category? category = new Category();
        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        private async Task HandleAdd()
        {
            try
            {
                if (!CategoryService.CheckCategoryName(category.Name))
                {
                    Error = ConstantValues.ERROR_CATEGORY_NAME_EXISTS;
                    HaveErrors = true;
                    return;
                }
                await CategoryService.AddCategory(category);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_CATEGORY_ADD;
                HaveErrors = true;
            }
        }

        protected async Task Close(MouseEventArgs? args)
        {
            DialogService.Close(null);
        }
    }
}