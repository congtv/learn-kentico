using System.Collections.Generic;
using XperienceAdapter.Models;

namespace Business.Models
{
    /// <summary>
    /// A home page.
    /// </summary>
    public class Home : BasicPage
    {
        public List<HomeSection> HomeSections { get; set; } = new List<HomeSection>();
    }
}
