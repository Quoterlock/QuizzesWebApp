using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.BusinessLogic.Models
{
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public ProfileOwnerInfo Owner { get; set; }
    }

    public class ProfileOwnerInfo
    {
        public string Id { get; set; }
        public string Username { get; set; }
    }
}
