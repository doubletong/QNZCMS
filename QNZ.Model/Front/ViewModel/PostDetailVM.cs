using QNZ.Data;

namespace QNZ.Model.Front.ViewModel
{
    public class PostDetailVM
    {
        public Post PostDetail { get; set; }
        public Post PostPrev { get; set; }
        public Post PostNext { get; set; }
    }
}