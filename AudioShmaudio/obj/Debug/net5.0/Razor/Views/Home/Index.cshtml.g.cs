#pragma checksum "D:\repos\AudioShmaudio\AudioShmaudio\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0f4aa3d6b1acb14df838e1e0f01411310212d238"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\repos\AudioShmaudio\AudioShmaudio\Views\_ViewImports.cshtml"
using AudioShmaudio;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\repos\AudioShmaudio\AudioShmaudio\Views\_ViewImports.cshtml"
using AudioShmaudio.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0f4aa3d6b1acb14df838e1e0f01411310212d238", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"76e8083e084694ce34dca251e1661b611aa5c58c", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\repos\AudioShmaudio\AudioShmaudio\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "АудиоШмаудио";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

<div class=""col-lg-2"" style=""background-color: #a99aa6"">
    <p>Тут будет чарт</p>
    <button class=""button-play"">Один</button>
    <button class=""button-second"">Два</button>
</div>
<div class=""col-lg-6"">
    <div class=""main-content"">
    </div>
</div>
<div class=""col-lg-4"" style=""background-color: #a99aa6"">
    Тут будет плеер
</div>


    <script>
        $("".button-play"").click(function () {
            $.ajax({
                url: '");
#nullable restore
#line 23 "D:\repos\AudioShmaudio\AudioShmaudio\Views\Home\Index.cshtml"
                 Write(Url.Action("Partial1"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"',
                type: 'GET',
                success: function (data) {
                    $('.main-content').html(data);
                },
                error: function () {
                    alert('Ошибочка');
                }
            });
        });

    $("".button-second"").click(function () {
        $.ajax({
            url: '");
#nullable restore
#line 36 "D:\repos\AudioShmaudio\AudioShmaudio\Views\Home\Index.cshtml"
             Write(Url.Action("Partial2"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"',
            type: 'GET',
            success: function (data) {
                $('.main-content').html(data);
            },
            error: function () {
                alert('Ошибочка');
            }
        });

    });
    </script>


");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
