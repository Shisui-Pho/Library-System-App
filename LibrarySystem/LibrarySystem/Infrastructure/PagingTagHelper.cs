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

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        TagBuilder nav = new("nav");
        nav.Attributes["aria-label"] = "something something";

        //Nav list
        TagBuilder ul = new("ul");
        ul.AddCssClass("pagination");
        ul.AddCssClass("justify-content-center");
        ul.AddCssClass("mt-4");

        BuildLinks(ul);
        nav.InnerHtml.AppendHtml(ul);
        output.Content.AppendHtml(ul);
    }//Process
    private TagBuilder BuildLinks(TagBuilder ul)
    {
        var urlBuilder = _urlHelperFactory.GetUrlHelper(ViewContext);
        int current = PageModel.CurrentPageNumber;
        //Left arrow if needed
        if (PageModel.TotalNumberOfPages > 1)
        {
            var previousPage = current <= 1 ? 1 : current - 1;
            ul.InnerHtml.AppendHtml(GetPagingArrow("left",current != 1, previousPage));
        }
        for (int i = 1; i <= PageModel.TotalNumberOfPages; i++)
        {
            //Build list item
            TagBuilder li = new("li");
            li.AddCssClass("page-item");
            li.AddCssClass(i == PageModel.CurrentPageNumber ? "active" : "");

            //Build link
            TagBuilder a = new("a");
            a.AddCssClass("page-link");
            a.InnerHtml.Append(i.ToString());//For displaying
            a.Attributes["href"] = urlBuilder.Action(Action, new { page = i });
            //Link them
            li.InnerHtml.AppendHtml(a);
            ul.InnerHtml.AppendHtml(li);
        }//end for

        //Right arrow if needed
        if (PageModel.TotalNumberOfPages > 1)
        {
            var nextPage = current >= PageModel.TotalNumberOfPages ? PageModel.TotalNumberOfPages : current + 1;
            ul.InnerHtml.AppendHtml(GetPagingArrow("right", current != PageModel.TotalNumberOfPages, nextPage));
        }

        return ul;
    }//BuildLinks
    private TagBuilder GetPagingArrow(string direction, bool enabled, int page)
    {
        TagBuilder liArrow = new("li");
        liArrow.AddCssClass(enabled ? "" : "disabled");
        liArrow.AddCssClass("page-item");
        TagBuilder arrowLink = new("a");
        arrowLink.AddCssClass("page-link");
        arrowLink.Attributes["href"] = _urlHelperFactory.GetUrlHelper(ViewContext).Action(Action, new { page = page });
        TagBuilder icon = new("i");
        icon.AddCssClass($"fa fa-chevron-{direction}");
        arrowLink.InnerHtml.AppendHtml(icon);
        liArrow.InnerHtml.AppendHtml(arrowLink);
        return liArrow;
    }
}//class
