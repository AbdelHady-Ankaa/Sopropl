using System.Collections.Generic;
using System.Threading.Tasks;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        // private readonly IAoNGraph graph;
        public ActivityRepository()
        {
            // this.graph = new AoNGraph()
        }

        public bool AddArrow(Arrow arrow)
        {
            throw new System.NotImplementedException();
        }

        public bool AddGraph(Activity activity)
        {
            throw new System.NotImplementedException();
        }

        public bool AddNode(Activity activity)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AssignToTeamAsync(Organization organization, Team team, Activity activity)
        {
            throw new System.NotImplementedException();
        }

        public void ComputeValues()
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Activity> GetActivitiesGraph()
        {
            throw new System.NotImplementedException();
        }

        public Activity GetGraph()
        {
            throw new System.NotImplementedException();
        }

        public Activity GetNode(string activityName)
        {
            throw new System.NotImplementedException();
        }

        public void InitializeValues()
        {
            throw new System.NotImplementedException();
        }

        public void NormalizeGraph()
        {
            throw new System.NotImplementedException();
        }

        public Arrow RemoveArrow(Arrow arrow)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveFakeNodes()
        {
            throw new System.NotImplementedException();
        }

        public Activity RemoveNode(string activityName)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UnassignToTeam(Organization organization, Team team, Activity activity)
        {
            throw new System.NotImplementedException();
        }

        public Arrow UpdateArrow(Arrow arrow)
        {
            throw new System.NotImplementedException();
        }

        public Activity UpdateNode(string oldName, Activity activity)
        {
            throw new System.NotImplementedException();
        }
    }
}