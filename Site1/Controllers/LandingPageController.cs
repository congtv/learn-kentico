using CMS.DocumentEngine;
using Common.Configuration;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Site1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XperienceAdapter.Localization;
using XperienceAdapter.Models;
using XperienceAdapter.Repositories;

[assembly: RegisterPageRoute(CMS.DocumentEngine.Types.Site1.LandingPage.CLASS_NAME, typeof(LandingPageController))]
namespace Site1.Controllers
{
    public class LandingPageController : BaseController
    {
        private readonly IPageDataContextRetriever _pageDataContextRetriever;

        private readonly IPageRepository<BasicPage, TreeNode> _landingPageRepository;

        public LandingPageController(
            ILogger<LandingPageController> logger,
            IOptionsMonitor<XperienceOptions> optionsMonitor,
            IStringLocalizer<SharedResource> stringLocalizer,
            IPageDataContextRetriever pageDataContextRetriever,
            IPageRepository<BasicPage, TreeNode> landingPageRepository)
        {
            _pageDataContextRetriever = pageDataContextRetriever ?? throw new ArgumentNullException(nameof(pageDataContextRetriever));
            _landingPageRepository = landingPageRepository ?? throw new ArgumentNullException(nameof(landingPageRepository));
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            if (_pageDataContextRetriever.TryRetrieve<CMS.DocumentEngine.Types.Site1.LandingPage>(out var pageDataContext)
                && pageDataContext.Page != null)
            {
                var landingPagePath = pageDataContext.Page.NodeAliasPath;

                if (!string.IsNullOrEmpty(landingPagePath))
                {
                    var landingPage = (await _landingPageRepository.GetPagesInCurrentCultureAsync(
                        cancellationToken,
                        filter => filter
                            .Path(landingPagePath, PathTypeEnum.Single)
                            .TopN(1),
                        buildCacheAction: cache => cache
                            .Key($"{nameof(LandingPageController)}|Page|{landingPagePath}")
                            .Dependencies((_, builder) => builder
                                .PageType(CMS.DocumentEngine.Types.Site1.LandingPage.CLASS_NAME)
                                .PagePath(landingPagePath, PathTypeEnum.Single))))
                            .FirstOrDefault();

                    if (landingPage != null)
                    {
                        var viewModel = GetPageViewModel(pageDataContext.Metadata, landingPage);

                        return View(viewModel);
                    }

                }
            }

            return NotFound();
        }
    }
}