using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LibrarySystem.Utils;
//Render ViewComponent as a string:
//https://gist.github.com/pauldotknopf/b424e9b8b03d31d67f3cce59f09ab17f?permalink_comment_id=3635387
public static class ControllerExtension
{
    public static async Task<string> RenderViewComponentToStringAsync(this Controller controller, string componentName, object arguments = null)
    {
        var viewComponentResult = controller.ViewComponent(componentName, arguments);
        var executor = controller.HttpContext.RequestServices.GetService(typeof(ViewComponentResultExecutor)) as ViewComponentResultExecutor;

        using var writer = new StringWriter();
        var viewContext = new ViewContext(
            controller.ControllerContext,
            new EmptyView(),
            controller.ViewData,
            controller.TempData,
            writer,
            new HtmlHelperOptions()
        );

        await executor.ExecuteAsync(viewContext, viewComponentResult);
        return writer.ToString();
    }//RenderViewComponentToStringAsync
}//class
public class EmptyView : IView
{
    public string Path => string.Empty;

    public Task RenderAsync(ViewContext context) => Task.CompletedTask;
}