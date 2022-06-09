using ParkyAPI.Models;
using System.Collections.Generic;

namespace ParkyAPI.Repository.IRepository
{
    public interface INationalParkRepository
    {
        IEnumerable<NationalPark> GetNationalParks();
        NationalPark GetNationalPark(int id);
        bool NationalParkExists(string Name);
        bool NationalParkExists(int id);
        bool CreateNAtionalPark(NationalPark park);
        bool UpdateNationalPark(NationalPark park);
        bool DeleteNationalPark(NationalPark park);
        bool Save();


    }
}
