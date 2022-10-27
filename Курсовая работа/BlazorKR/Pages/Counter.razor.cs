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
using BlazorKR;
using BlazorKR.Shared;
using KR.API.Data;

namespace BlazorKR.Pages
{
    public partial class Counter
    {
        private int currentCount = 0;
      
        private void IncrementCount()
        {
            currentCount++;
        }
    }
}