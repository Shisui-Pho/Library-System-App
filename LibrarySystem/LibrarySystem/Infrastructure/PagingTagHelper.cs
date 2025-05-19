using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LibrarySystem.Infrastructure;
[HtmlTargetElement("div", Attributes ="page-model")]
public class PagingTagHelper : TagHelper
{
    private readonly IUrlHelperFactory _urlHelperFactory;
    public PagingTagHelper(IUrlHelperFactory urlHelperFactory)
    {
        this._urlHelperFactory = urlHelperFactory;
    }

    //Properties
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    //-Html bound attributes
    /// <summary>
    /// The action of the controller to be called when a page is selected.
    /// </summary>
    public string Action {  get; set; }
    /// <summary>
    /// Contains all the information about the Paging.
    /// </summary>
    public PagingInfomation PageModel { get; set; }
    /// <summary>
    /// Indicates whether css attributes will be applied to the pages or not.
    /// </summary>
    public bool IsCssEnabled {  get; set; }
    /// <summary>
    /// The css class to be applied to the currently selected page.
    /// </summary>
    public string SelectedPageClass {  get; set; }
    /// <summary>
    /// The css class to be applied to the pages that have not been selected yet.
    /// </summary>
    public string UnSelectedPageClass { get; set; }
    /// <summary>
    /// Css page class that is common in both the two different pages(Current and default pages)
    /// </summary>
    public string CommonPageClass { get; set; }


    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        //Define the outer container
        TagBuilder div = new("div");
        var urlBuilder = _urlHelperFactory.GetUrlHelper(ViewContext);

        for (int i = 1; i <= PageModel.TotalNumberOfPages; i++)
        {
            //Create a link element
            TagBuilder a = new ("a");

            //Apply css classes
            if (IsCssEnabled)
            {
                //Apply common css
                a.AddCssClass(CommonPageClass);
                a.AddCssClass(i == PageModel.CurrentPageNumber ? SelectedPageClass : UnSelectedPageClass);
            }
            
            a.Attributes["href"] = urlBuilder.Action(Action, new { page = i });
            a.InnerHtml.Append(i.ToString());//Add the page name
            div.InnerHtml.AppendHtml(a);//Add the link to the div
        }//end for

        div.AddCssClass("btn-group");
        output.Content.AppendHtml(div);
    }//Process
}//class
