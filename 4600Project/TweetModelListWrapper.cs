using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4600Project
{
    public class TweetModelListWrapper : BaseModel
    {
        public TweetModelListWrapper(){
            TweetModelList = new List<TweetModel>();
            }

        public List<TweetModel> TweetModelList { get; set; }
    }
}
