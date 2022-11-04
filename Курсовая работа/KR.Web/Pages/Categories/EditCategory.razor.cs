using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using KR.Web;
using KR.Web.Shared;
using KR.Web.Constants;
using KR.Web.Models;
using KR.Web.Services;
using Kr.Models;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

        Category category = new Category();
        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Category categoryCheck = await CategoryService.GetCategoryById(Id_Category);
            if (categoryCheck == null)
            {
                await Close(null);
                return;
            }
            category =  categoryCheck;
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