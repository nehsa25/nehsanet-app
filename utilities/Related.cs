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

        public async Task<List<DBPage>> GetRelatedPages(string pagename)
        {
            _logger.Log("Enter: GetRelatedPages/pagename [GET]");


            //command.CommandText = @"
            //SELECT stem, title 
            //FROM related_pages rp
            //join pages p on rp.related_page_id = p.id 
            //WHERE rp.page_id = (select id from pages where stem = @Page)";

            var relatedPages = await _context.DBRelatedPage
                .Include(_ => _.Page)
                .Include(_ => _.RelatedPage)
                .Where(rp => rp.Page.stem == pagename).Select(_ => new DBPage()
                {
                    stem = _.RelatedPage.stem,
                    title = _.RelatedPage.title
                }).ToListAsync();

            return relatedPages;
        }
    }
}