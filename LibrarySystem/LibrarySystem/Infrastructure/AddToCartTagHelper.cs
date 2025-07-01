using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace LibrarySystem.Infrastructure;
[HtmlTargetElement("div", Attributes = "cart-item-book-id")]
public class AddToCartTagHelper : TagHelper
{
    public int CartItemBookID { get; set; }
    public int CartItemQuantity { get; set; }

    public decimal CartItemPrice { get;set; }
    public string PageWhereItemWasAdded { get; set; }
    
    public string DivID { get; set; }
    [HtmlAttributeName("complementary-div")]
    public string IncrementDivID { get; set; }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Build form
        var form = BuildForm();

        // Button for adding to cart
        var button = BuildClickButton();
        button.Attributes.Add("data-origin", DivID);
        button.Attributes.Add("data-complementary", IncrementDivID);

        // Button container
        var buttonDiv = new TagBuilder("div");
        buttonDiv.AddCssClass("buttons");
        buttonDiv.InnerHtml.AppendHtml(button);

        form.InnerHtml.AppendHtml(buttonDiv);

        // Output only the form (no extra wrapping div)
        output.TagName = "form";
        output.Attributes.SetAttribute("method", "post");
        output.Attributes.SetAttribute("action", "/Cart/AddToCart");
        output.Content.SetHtmlContent(form.InnerHtml);
    }//Process
    private static TagBuilder BuildClickButton()
    {
        var button = new TagBuilder("button");
        button.Attributes.Add("type", "button");
        button.AddCssClass("ajax-cart-btn btn button-blue button-stretch-width");
        button.InnerHtml.Append("Add to cart");
        return button;
    }//BuildClickButton
    private TagBuilder BuildForm()
    {
        //Build form
        TagBuilder form = new("form");
        form.Attributes.Add("method", "post");
        form.Attributes.Add("action", "/Cart/AddToCart");
        //Build all fields
        //-Hiddent inputs 
        form.InnerHtml.AppendHtml(CreateHiddenInput("CartItem.BookID", CartItemBookID.ToString()));
        form.InnerHtml.AppendHtml(CreateHiddenInput("CartItem.Price", CartItemPrice.ToString("F2")));
        form.InnerHtml.AppendHtml(CreateHiddenInput("CartItem.Quantity", CartItemQuantity.ToString()));
        form.InnerHtml.AppendHtml(CreateHiddenInput("PageWhereItemWasAdded", PageWhereItemWasAdded));

        return form;
    }//BuildForm
    private static TagBuilder CreateHiddenInput(string name, string value)
    {
        var input = new TagBuilder("input");
        input.Attributes["type"] = "hidden";
        input.Attributes["name"] = name;
        input.Attributes["value"] = value;
        return input;
    }
}//class
