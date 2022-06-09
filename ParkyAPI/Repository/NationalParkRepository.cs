using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _Db;

        public NationalParkRepository(ApplicationDbContext Db)
        {
            _Db = Db;
        }
        public bool CreateNAtionalPark(NationalPark park)
        {
            _Db.NationalParks.Add(park);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark park)
        {
            _Db.NationalParks.Remove(park);
            return Save();
        }

        public NationalPark GetNationalPark(int id)
        {
            return _Db.NationalParks.SingleOrDefault(X => X.Id == id);
        }

        public IEnumerable<NationalPark> GetNationalParks()
        {
            return _Db.NationalParks.OrderBy(a => a.Name).ToList();
        }

        public bool NationalParkExists(string Name)
        {
            return _Db.NationalParks.Any(x => x.Name.ToLower().Trim() == Name.ToLower().Trim());
        }

        public bool NationalParkExists(int id)
        {
            return _Db.NationalParks.Any(x => x.Id == id);
        }

        public bool Save()
        {
            return _Db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(NationalPark park)
        {
            _Db.NationalParks.Update(park);
            return Save();
        }
    }
}
