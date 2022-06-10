using System;
using System.Linq;
using System.Threading;

using XperienceAdapter.Repositories;
using XperienceAdapter.Services;
using Business.Models;
using Kentico.Content.Web.Mvc;
using System.Threading.Tasks;
using CMS.DocumentEngine;
using System.Collections.Generic;
using CMS.DataEngine;

namespace Business.Repositories
{
    /// <summary>
    /// Stores the home.
    /// </summary>
    public class HomeSectionRepository : BasePageRepository<HomeSection, CMS.DocumentEngine.Types.Site1.HomeSection>
    {
        private readonly IPageUrlRetriever pageUrlRetriever;

        public HomeSectionRepository(
            IPageRetriever pageRetriever,
            IPageUrlRetriever pageUrlRetriever,
            IPageDataContextRetriever pageDataContextRetriever) 
            : base(pageRetriever, pageDataContextRetriever)
        {
            this.pageUrlRetriever = pageUrlRetriever;
        }

        public override void MapDtoProperties(CMS.DocumentEngine.Types.Site1.HomeSection section, HomeSection dto)
        {
            dto.Id = section.HomeSectionID;
            dto.Title = section.HomeSectionTitle;
            dto.Detail = section.HomeSectionDetail;
        }

        public async Task<IEnumerable<HomeSection>> GetAllAsync(string homePath, int count = 100)
        {
            var homeSections = await pageRetriever.RetrieveAsync<CMS.DocumentEngine.Types.Site1.HomeSection>(
                query => {
                    query.Path(homePath, PathTypeEnum.Children)
                                        .TopN(count)
                                        .OrderBy("NodeOrder");
                    System.Diagnostics.Debug.WriteLine(query.GetFullQueryText());
                                        },
                buildCacheAction: cache => cache
                        .Key($"{nameof(HomeSectionRepository)}|GetAll|{homePath}|{count}")
                        // Include path dependency to flush cache when a new child page is created or page order is changed.
                        .Dependencies((_, builder) => builder.PagePath(homePath, PathTypeEnum.Children).PageOrder()));

            var results = new List<HomeSection>();
            foreach (var homeSection in homeSections)
            {
                var dto = MapBasicDtoProperties(homeSection);
                MapDtoProperties(homeSection, dto);
                var sectionPage = pageUrlRetriever.Retrieve(homeSection, true);
                dto.Url = sectionPage.RelativePath;
                results.Add(dto);
            }
            return results;
        }
    }
}
