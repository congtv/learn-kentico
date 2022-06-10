using System;
using System.Linq;
using System.Threading;

using XperienceAdapter.Repositories;
using XperienceAdapter.Services;
using Business.Models;
using Kentico.Content.Web.Mvc;

namespace Business.Repositories
{
    /// <summary>
    /// Stores the home.
    /// </summary>
    public class HomeRepository : BasePageRepository<Home, CMS.DocumentEngine.Types.Site1.Home>
    {
        public HomeRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever) 
            : base(pageRetriever, pageDataContextRetriever)
        {
        }
    }
}
