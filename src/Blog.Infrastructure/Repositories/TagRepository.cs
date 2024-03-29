﻿using Blog.Core.Models;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(BlogDbContext db) : base(db)
        {

        }

        public async Task<IEnumerable<Tag>> GetAllTags()
        {
            var tags = Set.Include(tag => tag.Articles);

            return await tags.ToListAsync();
        }

        public async Task<Tag> GetTagById(Guid id)
        {
            var tags = Set.Include(tag => tag.Articles)
                          .ThenInclude(article => article.Tags)
                          .Include(tag => tag.Articles)
                          .ThenInclude(article => article.User);

            return await tags.FirstOrDefaultAsync(tag => tag.Id == id); ;
        }
    }
}