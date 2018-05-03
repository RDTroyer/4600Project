using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4600Project
{
    public class TweetModelListWrapper : BaseModel
    {
        /// <summary>
        /// The following contructor assigns the TweetModelList property to a new list.
        /// This is used to wrap the TweetModelList to peoperly view the list on xaml.
        /// 
        /// Precondition: none
        /// Postcondition: none
        /// </summary>
        public TweetModelListWrapper()
        {

            TweetModelList = new List<TweetModel>();

        }

        public List<TweetModel> TweetModelList { get; set; }
    }
}
