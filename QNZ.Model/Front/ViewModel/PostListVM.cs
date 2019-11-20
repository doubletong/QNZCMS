using System.Collections.Generic;
using QNZ.Data;

namespace QNZ.Model.Front.ViewModel
{
    public class PostListVM
    {
        public IEnumerable<Post> Posts { get; set; }
        public int? CategoryId { get; set; }
    }
}