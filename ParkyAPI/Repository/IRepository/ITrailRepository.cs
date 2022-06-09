using ParkyAPI.Models;
using System.Collections.Generic;

namespace ParkyAPI.Repository.IRepository
{
    public interface ITrailRepository
    {
        IEnumerable<Trail> GetAllTrails();
        Trail GetTrail(int id);
        IEnumerable<Trail> GetTrailInNationalPark(int NPid);
        bool TrailExists(int id);
        bool TrailExists(string Name);
        bool CreateTrail(Trail trail);
        bool UpdateTrail(Trail trail);
        bool DeleteTrail(Trail trail);
        bool Save();
    }
}
