using System.Collections.Generic;
using QNZ.Data;

namespace QNZ.Model.Site.ViewModel
{
    public class PostListVM
    {
        public IEnumerable<Post> Posts { get; set; }
        public int? CategoryId { get; set; }
    }
    public class PostDetailVM
    {
        public Post PostDetail { get; set; }
        public Post PostPrev { get; set; }
        public Post PostNext { get; set; }
    }
    public class PostCategoryListVM
    {
        public IEnumerable<PostCategory> Categories{ get; set; }
        public int? CurrentId { get; set; }
    }
}