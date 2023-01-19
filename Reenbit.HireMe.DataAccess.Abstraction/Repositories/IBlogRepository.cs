using Reenbit.HireMe.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Abstraction.Repositories
{
    public interface IBlogRepository : IRepository<Blog, int>
    {

        Task<List<Blog>> GetAllPosts();
        Task<Blog> GetPostById(int id);
        Task<List<Blog>> GetPostsByEmail(int idUser);
        Task<List<Blog>> GetListLikesBookmarks();
        void UpdateBookmarks(Blog blog);
        void UpdateLikes(Blog blog);

    }
}
