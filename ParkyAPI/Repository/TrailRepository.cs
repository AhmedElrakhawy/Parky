using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private ApplicationDbContext _ApplicationDbContext;

        public TrailRepository(ApplicationDbContext applicationDbContext)
        {
            _ApplicationDbContext = applicationDbContext;
        }
        public bool CreateTrail(Trail trail)
        {
            _ApplicationDbContext.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _ApplicationDbContext.Remove(trail);
            return Save();
        }

        public IEnumerable<Trail> GetAllTrails()
        {
            return _ApplicationDbContext.Trails.OrderBy(x => x.Name).Include(x => x.NationalPark).ToList();
        }

        public Trail GetTrail(int id)
        {
            return _ApplicationDbContext.Trails.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Trail> GetTrailInNationalPark(int NPid)
        {
            return _ApplicationDbContext.Trails
                .Include(c => c.NationalPark).Where(x => x.NationalParkId == NPid).ToList();
        }

        public bool Save()
        {
            return _ApplicationDbContext.SaveChanges() >= 0 ? true : false;
        }

        public bool TrailExists(int id)
        {
            return _ApplicationDbContext.Trails.Any(x => x.Id == id);
        }

        public bool TrailExists(string Name)
        {
            return _ApplicationDbContext.Trails.Any(x => x.Name == Name);
        }

        public bool UpdateTrail(Trail trail)
        {
            _ApplicationDbContext.Trails.Update(trail);
            return Save();
        }
    }
}
