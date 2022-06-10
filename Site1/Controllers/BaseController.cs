using Microsoft.AspNetCore.Mvc;
using Kentico.Content.Web.Mvc;
using Site1.Models;

namespace Site1.Controllers
{
    public abstract class BaseController : Controller
    {
        protected PageViewModel GetPageViewModel(
            IPageMetadata pageMetadata,
            string? message = default,
            bool displayMessage = true,
            bool displayAsRaw = default,
            MessageType messageType = MessageType.Info)
            =>
            PageViewModel.GetPageViewModel(
                pageMetadata,
                message,
                displayMessage,
                displayAsRaw,
                messageType);

        protected PageViewModel<TViewModel> GetPageViewModel<TViewModel>(
            IPageMetadata pageMetadata,
            TViewModel data,
            string? message = default,
            bool displayMessage = true,
            bool displayAsRaw = default,
            MessageType messageType = MessageType.Info)
            =>
            PageViewModel<TViewModel>.GetPageViewModel(
                data,
                pageMetadata,
                message,
                displayMessage,
                displayAsRaw,
                messageType);
    }
}
