using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Kr.Models;
using KR.Web.Services;
using KR.Web.Constants;


namespace KR.Web.Pages.Categories
{
    public partial class CategoryPage
    {
        Category _category;
        [Inject]
        private CategoryService CategoryService { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<Category>? grid;


        private Category category
        {
            get;

            set;
        }

        private IEnumerable<Category> getCategoriesResult;


        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        private async Task Load()
        {
            getCategoriesResult = await CategoryService.GetCategories();
        }

        private async Task CategoryRowSelect(Category args)
        {
            await DialogService.OpenAsync<EditCategory>(ConstantValues.EDIT_CATEGORY, new Dictionary<string, object>() { { ConstantValues.Id_Category, args.Id_Category } });
            CategoryService.Reload();
            await grid.Reload();
        }

        private async Task AddCategory()
        {
            await DialogService.OpenAsync<AddCategory>(ConstantValues.ADD_CATEGORY);
            CategoryService.Reload();
            await grid.Reload();
        }




        private async Task DeleteCategory(MouseEventArgs args, dynamic data)
        {
            try
            {
                ConfirmOptions options = new ConfirmOptions();
                options.CancelButtonText = ConstantValues.CANCEL;
                options.OkButtonText = ConstantValues.OK_DELETE;
                if (await DialogService.Confirm(ConstantValues.DELETE_RECORD, ConstantValues.DELETE_RECORD_TITLE, options) == true)
                {
                    if (!CategoryService.DeleteCategory(data))
                    {
                        await DialogService.OpenAsync<ErrorDelete>(ConstantValues.DELETE_ERROR);
                    }
                    CategoryService.Reload();
                    await grid.Reload();
                }
            }
            catch 
            {
                await grid.Reload();
            }
        }

    }
}