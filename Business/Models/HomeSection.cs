using System.Collections.Generic;
using System.Linq;
using XperienceAdapter.Models;

namespace Business.Models
{
    public class HomeSection : BasicPage
    {
        public override IEnumerable<string> SourceColumns => base.SourceColumns.Concat(new[] { "HomeSectionTitle", "HomeSectionDetail" });
        public int Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Url { get; set; }
    }
}
