using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using Kentico.Content.Web.Mvc;

using XperienceAdapter.Models;
using XperienceAdapter.Services;

namespace XperienceAdapter.Repositories
{
    /// <summary>
    /// Provides base functionality to retrieve pages.
    /// </summary>
    /// <typeparam name="TPageDto">Page DTO.</typeparam>
    /// <typeparam name="TPage">Xperience page.</typeparam>
    public abstract class BasePageRepository<TPageDto, TPage>
        where TPageDto : BasicPage, new()
        where TPage : TreeNode, new()
    {
        protected readonly IPageRetriever pageRetriever;
        protected readonly IPageDataContextRetriever pageDataContextRetriever;

        /// <summary>
        /// Default DTO factory method.
        /// </summary>
        protected virtual Func<TPageDto> DefaultDtoFactory => () => new TPageDto();

        public BasePageRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageDataContextRetriever = pageDataContextRetriever;
        }

        public virtual IEnumerable<TPageDto> GetAll() => GetPagesInCurrentCulture(
            buildCacheAction: cache => cache
                .Key($"{nameof(BasePageRepository<TPageDto, TPage>)}|{typeof(TPage).Name}")
                .Expiration(TimeSpan.FromSeconds(30)));

        public virtual async Task<IEnumerable<TPageDto>> GetAllAsync(CancellationToken? cancellationToken = default) =>
            await GetPagesInCurrentCultureAsync(
                cancellationToken,
                buildCacheAction: cache => cache
                    .Key($"{nameof(BasePageRepository<TPageDto, TPage>)}|{typeof(TPage).Name}")
                    .Expiration(TimeSpan.FromSeconds(30)));

        public virtual IEnumerable<TPageDto> GetPagesInCurrentCulture(
            Action<DocumentQuery<TPage>>? filter = default,
            Func<TPage, TPageDto, TPageDto>? additionalMapper = default,
            Action<IPageCacheBuilder<TPage>>? buildCacheAction = default,
            bool includeAttachments = default)
        {
            var result = pageRetriever.Retrieve(query =>
            {
                query.Columns(DefaultDtoFactory().SourceColumns);
                filter?.Invoke(query);
            },
            buildCacheAction);

            return MapPages(result, additionalMapper, includeAttachments);
        }

        public virtual async Task<IEnumerable<TPageDto>> GetPagesInCurrentCultureAsync(
            CancellationToken? cancellationToken = default,
            Action<DocumentQuery<TPage>>? filter = default,
            Func<TPage, TPageDto, TPageDto>? additionalMapper = default,
            Action<IPageCacheBuilder<TPage>>? buildCacheAction = default,
            bool includeAttachments = default)
        {
            var result = await pageRetriever.RetrieveAsync(query =>
            {
                query.Columns(DefaultDtoFactory().SourceColumns);
                filter?.Invoke(query);

                Debug.WriteLine(query.GetFullQueryText());
            },
            buildCacheAction,
            cancellationToken);

            return MapPages(result, additionalMapper, includeAttachments);
        }

        /// <summary>
        /// Maps query results onto DTOs.
        /// </summary>
        /// <param name="pages">Xperience pages.</param>
        /// <param name="additionalMapper">Ad-hoc mapper supplied as a parameter.</param>
        /// <param name="includeAttachments">Indicates if attachment information shall be included.</param>
        /// <returns>Page DTOs.</returns>
        protected IEnumerable<TPageDto> MapPages(
            IEnumerable<TPage?>? pages = default,
            Func<TPage, TPageDto, TPageDto>? additionalMapper = default,
            bool includeAttachments = default)
        {
            if (pages != null && pages.Any())
            {
                if (additionalMapper != null)
                {
                    foreach (var page in pages)
                    {
                        var dto = ApplyMappers(page!, includeAttachments);

                        if (dto != null)
                        {
                            yield return additionalMapper(page!, dto);
                        }
                    }
                }
                else
                {
                    foreach (var page in pages)
                    {
                        var dto = ApplyMappers(page!, includeAttachments);

                        if (dto != null)
                        {
                            yield return dto;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Applies the basic mapper as well as the type-specific one.
        /// </summary>
        /// <param name="page">Xperience page.</param>
        /// <param name="includeAttachments">Indicates if attachment information shall be included.</param>
        /// <returns>Page DTO.</returns>
        protected TPageDto ApplyMappers(TPage page, bool includeAttachments)
        {
            var dto = MapBasicDtoProperties(page, includeAttachments);
            MapDtoProperties(page, dto);

            return dto;
        }

        /// <summary>
        /// Maps basic Xperience page properties onto DTO ones.
        /// </summary>
        /// <param name="page">Xperience page.</param>
        /// <param name="includeAttachments">Indicates if attachment information shall be included.</param>
        /// <returns>Page DTO.</returns>
        protected virtual TPageDto MapBasicDtoProperties(TPage page, bool includeAttachments = false)
        {
            var dto = DefaultDtoFactory();
            dto.Guid = page.DocumentGUID;
            dto.NodeId = page.NodeID;
            dto.Name = page.DocumentName;
            dto.NodeAliasPath = page.NodeAliasPath;
            dto.ParentId = page.NodeParentID;

            return dto;
        }

        /// <summary>
        /// Default DTO mapping method.
        /// </summary>
        /// <param name="page">Xperience page.</param>
        /// <param name="dto">Page DTO.</param>
        public virtual void MapDtoProperties(TPage page, TPageDto dto) { }
    }
}
