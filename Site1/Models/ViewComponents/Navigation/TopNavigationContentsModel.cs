using System.Collections.Generic;

using XperienceAdapter.Models;
using Business.Models;

namespace Site1.Models.ViewComponents.Navigation
{
    public class TopNavigationContentsModel
    {
        public string? MainId { get; set; }
        
        public string? CultureSwitchId { get; set; }

        public string? ListClass { get; set; }

        public bool DisplaySecondLevel { get; set; }
    }
}
