using System.Threading.Tasks;
using Sopropl_Backend.Models;
using System.Collections.Generic;

namespace Sopropl_Backend.Repositories
{
    public interface IActivityRepository
    {
        Task<bool> AssignToTeamAsync(Organization organization, Team team, Activity activity);
        Task<bool> UnassignToTeam(Organization organization, Team team, Activity activity);
        ICollection<Activity> GetActivitiesGraph();
        bool AddNode(Activity activity);
        Activity UpdateNode(string oldName, Activity activity);
        Activity RemoveNode(string activityName);
        Activity GetNode(string activityName);
        Activity GetGraph();
        bool AddGraph(Activity activity);
        bool AddArrow(Arrow arrow);
        Arrow RemoveArrow(Arrow arrow);
        Arrow UpdateArrow(Arrow arrow);
        void ComputeValues();
        void InitializeValues();
        void NormalizeGraph();
        void RemoveFakeNodes();
    }
}