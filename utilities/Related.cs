using System.Collections;
using Microsoft.EntityFrameworkCore;
using nehsanet_app.db;
using nehsanet_app.Models;
using nehsanet_app.Services;
using ZstdSharp.Unsafe;

namespace nehsanet_app.utilities
{
    public class RelatedPagesUtility(DataContext context, ILogger logger)
    {
        private readonly DataContext _context = context;
        private readonly ILogger _logger = logger;

        public async Task<dynamic> GetRelatedPages(string pagename)
        {
            _logger.LogInformation("Enter: GetRelatedPages/pagename [GET]. pagename: {pagename}", pagename);
            var id = _context.DBPage.Where(p => p.Stem == pagename).Select(p => p.Id).FirstOrDefault();
            var pages = await _context.DBRelatedPage
                .Include(rp => rp.Page)
                .Where(rp => rp.RelatedPageId == id).Select(_ => new
                {
                    _.Page.Stem,
                    _.Page.Title
                }).ToListAsync();

            _logger.LogInformation("Exit: GetRelatedPages/pagename. Found {pages} related pages.", pages.Count);
            return pages;
        }
    }
}