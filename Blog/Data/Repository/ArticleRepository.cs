﻿using Blog.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class ArticleRepository : Repository<Article>
    {
        public ArticleRepository(BlogDbContext db) : base(db)
        {

        }

        public IQueryable<Article> GetAllArticles()
        {
            var articles = Set.Include(a => a.Tags)
                              .Include(b => b.ArticleLikes)
                              .Include(c => c.Comments)
                              .Include(u => u.User);

            return articles.AsNoTracking();
        }

        public async Task<Article> GetArticleById(Guid id)
        {
            var article = Set.Include(article => article.Tags)
                             .Include(article => article.Comments)
                                .ThenInclude(comments => comments.User)
                             .Include(article => article.User);

            return await article.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
