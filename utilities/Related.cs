using Microsoft.EntityFrameworkCore;
using nehsanet_app.db;
using nehsanet_app.Models;
using nehsanet_app.Services;

namespace nehsanet_app.utilities
{
    public class RelatedPagesUtility(DataContext context, ILoggingProvider logger)
    {
        private readonly DataContext _context = context;
        private readonly ILoggingProvider _logger = logger;

        public async Task<List<Page>> GetRelatedPages(string pagename)
        {
            _logger.Log($"Enter: GetRelatedPages/pagename [GET]. pagename: {pagename}");
            var pages = await _context.DBPage.Include(_ => _.RelatedPages)
                .Include(_ => _.RelatedPages)
                .Where(rp => rp.Title == pagename).Select(_ => new Page()
                {
                    Stem = _.Stem,
                    Title = _.Title
                }).ToListAsync();

            _logger.Log($"Exit: GetRelatedPages/pagename. Found {pages.Count} related pages.");
            return pages;
        }
    }
}