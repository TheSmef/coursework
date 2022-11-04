using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using KR.Web;
using KR.Web.Shared;
using Radzen;
using Radzen.Blazor;
using Blazored.LocalStorage;
using Kr.Models;
using System.ComponentModel;
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

        IEnumerable<Category> getCategoriesResult;


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