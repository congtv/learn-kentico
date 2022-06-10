using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CMS.DocumentEngine;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using XperienceAdapter.Repositories;
using Business.Models;
using Site1.Controllers;
using Business.Repositories;

[assembly: RegisterPageRoute(
    CMS.DocumentEngine.Types.Site1.Home.CLASS_NAME, 
    typeof(HomeController), 
    ActionName = nameof(HomeController.Index))]
[assembly: RegisterPageRoute(
    CMS.DocumentEngine.Types.Site1.HomeSection.CLASS_NAME, 
    typeof(HomeController), 
    ActionName = nameof(HomeController.Detail))]
namespace Site1.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPageDataContextRetriever _pageDataContextRetriever;

        public HomeController(IPageDataContextRetriever pageDataContextRetriever)
        {
            _pageDataContextRetriever = pageDataContextRetriever ?? throw new ArgumentNullException(nameof(pageDataContextRetriever));
        }

        public async Task<IActionResult> Index(
            [FromServices] HomeRepository homeRepository,
            [FromServices] HomeSectionRepository homeSectionRepository,
            CancellationToken cancellationToken)
        {
            if (_pageDataContextRetriever.TryRetrieve<CMS.DocumentEngine.Types.Site1.Home>(out var pageDataContext)
                && pageDataContext.Page != null)
            {
                var homePath = pageDataContext.Page.NodeAliasPath;
                var home = (await homeRepository.GetPagesInCurrentCultureAsync(
                    cancellationToken,
                    filter => filter
                        .Path(homePath, PathTypeEnum.Section)
                        .TopN(1),
                    buildCacheAction: cache => cache
                        .Key($"{nameof(HomeController)}|Home")
                        .Dependencies((_, builder) => builder
                            .PageType(CMS.DocumentEngine.Types.Site1.Home.CLASS_NAME)
                            .PagePath(homePath, PathTypeEnum.Children)),
                    includeAttachments: true))
                        .FirstOrDefault();

                var homeSections = await homeSectionRepository.GetAllAsync(homePath);

                if (home != null)
                {
                    home.HomeSections.AddRange(homeSections);
                    var viewModel = GetPageViewModel(pageDataContext.Metadata, home);
                    return View(viewModel);
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> Detail(
            [FromServices]HomeSectionRepository homeSectionRepository,
            CancellationToken cancellationToken)
        {
            if (_pageDataContextRetriever.TryRetrieve<CMS.DocumentEngine.Types.Site1.HomeSection>(out var pageDataContext)
                && pageDataContext.Page != null)
            {
                var homeSectionPath = pageDataContext.Page.NodeAliasPath;

                var homeSection = (await homeSectionRepository.GetPagesInCurrentCultureAsync(
                    cancellationToken,
                    filter => filter
                        .Path(homeSectionPath, PathTypeEnum.Single)
                        .TopN(1),
                    buildCacheAction: cache => cache
                        .Key($"{nameof(HomeController)}|Detail|{homeSectionPath}")
                        .Dependencies((_, builder) => builder
                            .PageType(CMS.DocumentEngine.Types.Site1.Home.CLASS_NAME)
                            .PagePath(homeSectionPath, PathTypeEnum.Single)),
                    includeAttachments: true))
                        .FirstOrDefault();

                if (homeSection != null)
                {
                    var viewModel = GetPageViewModel(pageDataContext.Metadata, homeSection);
                    return View(viewModel);
                }
            }

            return NotFound();
        }
    }
}
