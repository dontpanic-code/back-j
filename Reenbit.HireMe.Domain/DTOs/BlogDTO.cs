using System;
using System.Collections.Generic;
using System.Text;

namespace Reenbit.HireMe.Domain.DTOs
{
    class BlogDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string? Date { get; set; }
        public string Text { get; set; }
        public int Views { get; set; }
        public string Tags { get; set; }
        public int IdUser { get; set; }
        public int Comments { get; set; }
        public string Type { get; set; }
        public int Likes { get; set; }
        public string ListLikes { get; set; }
        public string ListBookmarks { get; set; }
    }
}
