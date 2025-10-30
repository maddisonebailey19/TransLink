using PrideLink.Server.Interfaces;
using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.General;

namespace PrideLink.Server.Helpers
{
    public class GeneralHelper : IGeneralInterface
    {
        public List<Hobbys> GetHobbies()
        {
            List<Hobbys> hobbys = new List<Hobbys>();
            using(var context = new MasContext())
            {
                List<TblHobby> listOfHobby = context.TblHobbies.ToList();
                foreach(TblHobby hobby in listOfHobby)
                {
                    Hobbys newHobby = new Hobbys();
                    newHobby.HobbyNo = hobby.HobbyNo;
                    newHobby.HobbyName = hobby.HobbyName;
                    hobbys.Add(newHobby);
                }
                return hobbys;
            }
        }
    }
}
