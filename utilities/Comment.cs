using Microsoft.EntityFrameworkCore;
using nehsanet_app.db;
using nehsanet_app.Models;
using nehsanet_app.Services;

namespace nehsanet_app.utilities
{
    public class CommentsUtility(ILoggingProvider logger, DataContext context)
    {
        private readonly ILoggingProvider _logger = logger;
        private readonly DataContext _context = context;

        public async Task<DBComment?> GetSingleCommentById(int commentid)
        {
            _logger.Log("Enter: GetComment/id [GET]");
            DBComment? dbResult = await _context.DBComment.FindAsync(commentid);
            if (dbResult == null)
                throw new Exception("Comment not found: " + commentid);

            _logger.Log("Enter: GetComment/id [GET]");
            return dbResult;
        }

        public async Task<IReadOnlyList<DBComment>> GetLastXCommentsByPage(string page, int numberToReturn)
        {
            _logger.Log("Enter: GetComment/id [GET]");
            return await _context.DBComment.Include(c => c.PageNavigation)
                .Where(c => c.PageNavigation.Stem == page)
                .OrderByDescending(c => c.CommentID)
                .Take(numberToReturn)
                .ToListAsync();
        }
    }
}