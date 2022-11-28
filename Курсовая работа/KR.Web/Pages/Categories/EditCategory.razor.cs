using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;

namespace KR.Web.Pages.Categories
{
    public partial class EditCategory
    {
        [Inject]
        private CategoryService CategoryService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        [Parameter]
        public dynamic Id_Category { get; set; }

        private Category category = new Category();
        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Category categoryCheck = await CategoryService.GetCategoryById(Id_Category);
                if (categoryCheck == null)
                {
                    await Close(null);
                    return;
                }
                category = categoryCheck;
            }
            catch
            {
                await Close(null);
            }

        }

        private async Task HandleEdit()
        {
            try
            {
                Category? obj = await CategoryService.GetCategoryByIdForReadOnly(category.Id_Category);
                if (obj == null)
                {
                    Error = ConstantValues.ERROR_CATEGORY_EDIT;
                    HaveErrors = true;
                    return;
                }
                if (!CategoryService.CheckCategoryName(category.Name) && obj.Name != category.Name)
                {
                    Error = ConstantValues.ERROR_CATEGORY_NAME_EXISTS;
                    HaveErrors = true;
                    return;
                }
                await CategoryService.EditCategory(category);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_CATEGORY_EDIT;
                HaveErrors = true;
            }
        }

        protected async Task Close(MouseEventArgs? args)
        {
            DialogService.Close(null);
        }

    }
}