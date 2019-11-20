using System.Collections.Generic;
using QNZ.Data;

namespace QNZ.Model.Front.ViewModel
{
    public class PostCategoryListVM
    {
        public IEnumerable<PostCategory> Categories{ get; set; }
        public int? CurrentId { get; set; }
    }
}