using Microsoft.EntityFrameworkCore;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Repositories
{
    public class BlogRepository : EntityFrameworkRepository<Blog, int>, IBlogRepository
    {

        public Task<List<Blog>> GetAllPosts()
        {
            IQueryable<Blog> query = this.DbSet.Select(u => new Blog { Id = u.Id, Title = u.Title, Comments = u.Comments, Date = u.Date, Tags = u.Tags, Views = u.Views , Author = u.Author, IdUser = u.IdUser, Likes = u.Likes, Type = u.Type }).OrderByDescending(u => u.Id);
            return query.ToListAsync();
        }
        public async Task<Blog> GetPostById(int id)
        {
            return await this.DbSet.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Blog>> GetPostsByEmail(int idUser)
        {
            IQueryable<Blog> query = this.DbSet.Where(c => c.IdUser == idUser).OrderByDescending(u => u.Id);
            return query.ToListAsync();
        }

        public Task<List<Blog>> GetListLikesBookmarks()
        {
            IQueryable<Blog> query = this.DbSet.Select(u => new Blog { Id = u.Id, ListLikes = u.ListLikes, ListBookmarks = u.ListBookmarks }).OrderByDescending(u => u.Id);
            return query.ToListAsync();
        }

        public void UpdateBookmarks(Blog blog)
        {
            var post = this.DbSet.Single(e => e.Id == blog.Id);
            post.ListBookmarks = blog.ListBookmarks;
            this.DbContext.SaveChanges();
        }

        public void UpdateLikes(Blog blog)
        {
            var post = this.DbSet.Single(e => e.Id == blog.Id);
            post.ListLikes = blog.ListLikes;
            this.DbContext.SaveChanges();
        }

    }
}
