using System.Collections.Generic;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public interface IAoNGraph
    {
        ICollection<Activity> GetActivitiesGraph();
        bool AddNode(Activity activity);
        Activity UpdateNode(Activity oldActivity, Activity newActivity);
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