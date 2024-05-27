using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class UserComment
    {
        public TraLoi traloi { get; set; }
        public NguoiDung nguoidung { get; set; }
    }
}