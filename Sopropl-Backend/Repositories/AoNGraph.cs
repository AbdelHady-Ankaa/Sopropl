using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.OData.Edm;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public class AoNGraph : IAoNGraph
    {
        public IDictionary<string, Activity> ActivitesGraph { get; set; }
        public ICollection<Activity> Activites { get; set; }
        public const string FAKE_START_ACTIVITY_NAME = "FakeStartActivity";
        public const string FAKE_END_ACTIVITY_NAME = "FakeEndActivity";
        public AoNGraph(ICollection<Activity> Activites)
        {
            this.ActivitesGraph = Activites.ToDictionary(a => a.Name);
            this.Activites = Activites;
        }

        // private bool IsNormalized = false;
        public void NormalizeGraph()
        {
            if (this.ActivitesGraph.ContainsKey(FAKE_START_ACTIVITY_NAME))
            {
                var fakeStratActivity = this.ActivitesGraph[FAKE_START_ACTIVITY_NAME];
                fakeStratActivity.OutArrows.Clear();
                fakeStratActivity.InArrows.Clear();
                this.ActivitesGraph.Remove(FAKE_START_ACTIVITY_NAME);
            }
            if (this.ActivitesGraph.ContainsKey(FAKE_END_ACTIVITY_NAME))
            {
                var fakeEndActivity = this.ActivitesGraph[FAKE_END_ACTIVITY_NAME];
                fakeEndActivity.OutArrows.Clear();
                fakeEndActivity.InArrows.Clear();
                this.ActivitesGraph.Remove(FAKE_END_ACTIVITY_NAME);
            }
            this.AddFakeActivityToTheStart();
            this.AddFakeActivityToTheEnd();
        }

        private ICollection<Activity> EndActivities()
        {
            var endActivities = new Collection<Activity>();
            foreach (var activity in this.ActivitesGraph.Values)
            {
                if (activity.OutArrows.Count == 0)
                {
                    endActivities.Add(activity);
                }
            }
            return endActivities;
        }

        private ICollection<Activity> StartActivities()
        {
            var startActivities = new Collection<Activity>();
            foreach (var activity in this.ActivitesGraph.Values)
            {
                if (activity.InArrows.Count == 0)
                {
                    startActivities.Add(activity);
                }
            }
            return startActivities;
        }

        private Activity AddFakeActivityToTheStart()
        {
            var startActivities = StartActivities();
            if (startActivities.Count == 1)
            {
                return startActivities.FirstOrDefault();
            }
            else
            {
                var fakeStartActivity = new Activity { Name = FAKE_START_ACTIVITY_NAME, Duration = 0, EarlyStart = startActivities.Min(a => a.EarlyStart) };
                foreach (var activity in startActivities)
                {
                    var arrow = new Arrow { FromActivity = fakeStartActivity, ToActivity = activity, Type = ArrowType.FINISH_TO_START, Value = 0 };
                    activity.InArrows.Add(arrow);
                    fakeStartActivity.OutArrows.Add(arrow);
                }
                this.ActivitesGraph.Add(fakeStartActivity.Name, fakeStartActivity);
                return fakeStartActivity;
            }
        }



        private Activity AddFakeActivityToTheEnd()
        {
            var endActivities = EndActivities();
            if (endActivities.Count == 1)
            {
                return endActivities.FirstOrDefault();
            }
            else
            {
                var fakeEndActivity = new Activity { Name = FAKE_END_ACTIVITY_NAME, Duration = 0 };
                foreach (var activity in endActivities)
                {
                    var arrow = new Arrow { FromActivity = activity, ToActivity = fakeEndActivity, Type = ArrowType.FINISH_TO_START, Value = 0 };
                    activity.OutArrows.Add(arrow);
                    fakeEndActivity.InArrows.Add(arrow);
                }
                this.ActivitesGraph.Add(fakeEndActivity.Name, fakeEndActivity);
                return fakeEndActivity;
            }
        }

        public bool AddNode(Activity Activity)
        {
            if (!this.ActivitesGraph.ContainsKey(Activity.Name))
            {
                this.RemoveFakeNodes();
                this.ActivitesGraph.Add(Activity.Name, Activity);
                this.Activites.Add(Activity);
                IsComputed = false;
                if (this.Activites.Count > 0)
                {
                    this.ComputeValues();
                }
                this.RemoveFakeNodes();
                return true;
            }
            return false;
        }

        public Activity RemoveNode(string activityName)
        {
            if (this.ActivitesGraph.ContainsKey(activityName))
            {
                // this.RemoveFakeNodes();
                var Activity = this.ActivitesGraph[activityName];
                this.Activites.Remove(Activity);
                this.ActivitesGraph.Remove(Activity.Name);
                IsComputed = false;
                // this.ComputeValues();
                // this.RemoveFakeNodes();
                return Activity;
            }
            return null;
        }

        public Activity GetGraph()
        {
            this.NormalizeGraph();
            if (this.Activites.Count > 0)
            {
                this.ComputeValues();

            }
            // this.RemoveFakeNodes();
            var s = this.StartActivities().FirstOrDefault();
            return s;
        }

        public bool AddArrow(Arrow Arrow)
        {
            if (this.ActivitesGraph.ContainsKey(Arrow.ToActivity.Name) && this.ActivitesGraph.ContainsKey(Arrow.FromActivity.Name))
            {
                var FromActivity = ActivitesGraph[Arrow.FromActivity.Name]; var ToActivity = ActivitesGraph[Arrow.ToActivity.Name];
                if (FromActivity.OutArrows.FirstOrDefault(oa => oa.ToActivity.Name == ToActivity.Name) == null)
                {
                    // this.RemoveFakeNodes();
                    FromActivity.OutArrows.Add(Arrow);
                    ToActivity.InArrows.Add(Arrow);
                    IsComputed = false;
                    // this.ComputeValues();
                    // this.RemoveFakeNodes();
                    return true;
                }
            }
            return false;
        }

        public Arrow RemoveArrow(Arrow Arrow)
        {

            if (this.ActivitesGraph.ContainsKey(Arrow.ToActivity.Name) && this.ActivitesGraph.ContainsKey(Arrow.FromActivity.Name))
            {
                var FromActivity = ActivitesGraph[Arrow.FromActivity.Name]; var ToActivity = ActivitesGraph[Arrow.ToActivity.Name];
                if (FromActivity.OutArrows.FirstOrDefault(oa => oa.ToActivity.Name == ToActivity.Name) != null)
                {
                    // this.RemoveFakeNodes();
                    FromActivity.InArrows.Remove(Arrow);
                    ToActivity.OutArrows.Remove(Arrow);
                    IsComputed = false;
                    // this.ComputeValues();
                    // this.RemoveFakeNodes();
                    return Arrow;
                }
            }
            return null;
        }



        private bool IsComputed = false;
        public void ComputeValues()
        {
            if (!IsComputed)
            {
                this.InitializeValues();
                IsComputed = true;
                var start = this.StartActivities().FirstOrDefault();
                start.EarlyFinish = start.EarlyStart?.AddDays(start.Duration);
                // if (start.Name != FAKE_START_ACTIVITY_NAME)
                // {
                //     var actvFromDB = this.Activites.First(a => a.Name == start.Name);
                //     actvFromDB.EarlyFinish = start.EarlyFinish;
                // }

                bool AllIsComputed = false;
                while (!AllIsComputed)
                {
                    AllIsComputed = true;
                    foreach (var Activity in this.ActivitesGraph.Values)
                    {
                        // var actvFromDB = this.Activites.First(a => a.Name == Activity.Name);
                        if (Activity.EarlyStart == null)
                        {
                            if (Activity.InArrows.Count > 0)
                            {
                                AllIsComputed = false;
                                bool valid = true;
                                foreach (var inArrow in Activity.InArrows)
                                {
                                    if (inArrow.FromActivity.EarlyStart == null)
                                    {
                                        valid = false;
                                    }
                                }
                                if (valid)
                                {
                                    foreach (var inArrow in Activity.InArrows)
                                    {
                                        Activity.EarlyStart = Compute(ES, inArrow);
                                        // actvFromDB.EarlyStart = Activity.EarlyStart;
                                        // this.Activite
                                    }
                                    Activity.EarlyFinish = Activity.EarlyStart?.AddDays(Activity.Duration);
                                    // actvFromDB.EarlyFinish = Activity.EarlyFinish;
                                }
                            }
                            if (Activity.OutArrows.Count == 0)
                            {
                                Activity.LateFinish = Activity.EarlyFinish;
                                // actvFromDB.LateFinish = Activity.LateFinish;
                                Activity.LateStart = Activity.LateFinish?.AddDays(-Activity.Duration);
                                // actvFromDB.LateStart = Activity.LateStart;
                            }
                        }
                    }
                }
                AllIsComputed = false;
                while (!AllIsComputed)
                {
                    AllIsComputed = true;
                    foreach (var Activity in this.ActivitesGraph.Values)
                    {
                        // var actvFromDB = this.Activites.First(a => a.Name == Activity.Name);
                        if (Activity.LateFinish == null)
                        {

                            if (Activity.OutArrows.Count > 0)
                            {
                                AllIsComputed = false;
                                bool valid = true;
                                foreach (var outArrow in Activity.OutArrows)
                                {
                                    if (outArrow.ToActivity.LateStart == null)
                                    {
                                        valid = false;
                                    }
                                }
                                if (valid)
                                {
                                    Activity.FreeFloat = double.MaxValue;
                                    // actvFromDB.FreeFloat = Activity.FreeFloat;
                                    foreach (var outArrow in Activity.OutArrows)
                                    {
                                        Activity.LateFinish = Compute(LF, outArrow);
                                        // actvFromDB.LateFinish = Activity.LateFinish;
                                        Activity.FreeFloat = ComputeFF(outArrow);
                                        // actvFromDB.FreeFloat = Activity.FreeFloat;
                                    }
                                    Activity.LateStart = Activity.LateFinish?.AddDays(-Activity.Duration);
                                    // actvFromDB.LateStart = Activity.LateStart;
                                    Activity.TotalFloat = (Activity.LateStart - Activity.EarlyStart).Value.TotalDays;
                                    // actvFromDB.TotalFloat = Activity.TotalFloat;
                                }
                            }
                        }
                    }
                }
            }
        }


        public void RemoveFakeNodes()
        {
            foreach (var activity in this.ActivitesGraph.Values)
            {
                activity.InArrows.RemoveAll(a => a.FromActivity.Name == FAKE_START_ACTIVITY_NAME);

                activity.OutArrows.RemoveAll(a => a.ToActivity.Name == FAKE_END_ACTIVITY_NAME);
            }
        }

        private const string ES = "ES";
        private const string LF = "LF";

        private double ComputeFF(Arrow arrow)
        {
            double temp;
            switch (arrow.Type)
            {
                case ArrowType.FINISH_TO_START:
                    temp = (arrow.ToActivity.EarlyStart - arrow.FromActivity.EarlyFinish).Value.TotalDays - arrow.Value;
                    return temp < arrow.FromActivity.FreeFloat ? temp : arrow.FromActivity.FreeFloat;

                case ArrowType.FINISH_TO_FINISH:
                    temp = (arrow.ToActivity.LateFinish - arrow.FromActivity.LateFinish).Value.TotalDays - arrow.Value;
                    return temp < arrow.FromActivity.FreeFloat ? temp : arrow.FromActivity.FreeFloat;


                case ArrowType.START_TO_FINISH:
                    temp = (arrow.ToActivity.LateFinish - arrow.FromActivity.EarlyStart).Value.TotalDays - arrow.Value;
                    return temp < arrow.FromActivity.FreeFloat ? temp : arrow.FromActivity.FreeFloat;


                case ArrowType.START_TO_START:
                    temp = (arrow.ToActivity.EarlyStart - arrow.FromActivity.EarlyStart).Value.TotalDays - arrow.Value;
                    return temp < arrow.FromActivity.FreeFloat ? temp : arrow.FromActivity.FreeFloat;


                default:
                    temp = (arrow.ToActivity.EarlyStart - arrow.FromActivity.EarlyFinish).Value.TotalDays - arrow.Value;
                    return temp < arrow.FromActivity.FreeFloat ? temp : arrow.FromActivity.FreeFloat;
            }
        }

        private DateTime? Compute(string Value, Arrow arrow)
        {
            DateTime? temp;
            switch (Value)
            {
                case ES:
                    switch (arrow.Type)
                    {
                        case ArrowType.FINISH_TO_START:
                            temp = arrow.FromActivity.EarlyFinish?.AddDays(arrow.Value);
                            return arrow.ToActivity.EarlyStart == null ? temp : (temp > arrow.ToActivity.EarlyStart ? temp : arrow.ToActivity.EarlyStart);

                        case ArrowType.FINISH_TO_FINISH:
                            temp = arrow.FromActivity.EarlyFinish?.AddDays(arrow.Value).AddDays(-arrow.ToActivity.Duration);
                            return arrow.ToActivity.EarlyStart == null ? temp : temp > arrow.ToActivity.EarlyStart ? temp : arrow.ToActivity.EarlyStart;

                        case ArrowType.START_TO_FINISH:
                            temp = arrow.FromActivity.EarlyStart?.AddDays(arrow.Value).AddDays(-arrow.ToActivity.Duration);
                            return arrow.ToActivity.EarlyStart == null ? temp : temp > arrow.ToActivity.EarlyStart ? temp : arrow.ToActivity.EarlyStart;

                        case ArrowType.START_TO_START:
                            temp = arrow.FromActivity.EarlyStart?.AddDays(arrow.Value);
                            return arrow.ToActivity.EarlyStart == null ? temp : temp > arrow.ToActivity.EarlyStart ? temp : arrow.ToActivity.EarlyStart;

                        default:
                            temp = arrow.FromActivity.EarlyFinish?.AddDays(arrow.Value);
                            return arrow.ToActivity.EarlyStart == null ? temp : temp > arrow.ToActivity.EarlyStart ? temp : arrow.ToActivity.EarlyStart;

                    }
                case LF:
                    switch (arrow.Type)
                    {
                        case ArrowType.FINISH_TO_START:
                            temp = arrow.ToActivity.LateStart?.AddDays(-arrow.Value);
                            return arrow.FromActivity.LateFinish == null ? temp : temp < arrow.FromActivity.LateFinish ? temp : arrow.FromActivity.LateFinish;

                        case ArrowType.FINISH_TO_FINISH:
                            temp = arrow.ToActivity.LateFinish?.AddDays(-arrow.Value);
                            return arrow.FromActivity.LateFinish == null ? temp : temp < arrow.FromActivity.LateFinish ? temp : arrow.FromActivity.LateFinish;


                        case ArrowType.START_TO_FINISH:
                            temp = arrow.ToActivity.LateFinish?.AddDays(arrow.Value).AddDays(arrow.ToActivity.Duration);
                            return arrow.FromActivity.LateFinish == null ? temp : temp < arrow.FromActivity.LateFinish ? temp : arrow.FromActivity.LateFinish;


                        case ArrowType.START_TO_START:
                            temp = arrow.ToActivity.LateStart?.AddDays(-arrow.Value).AddDays(arrow.FromActivity.Duration);
                            return arrow.FromActivity.LateFinish == null ? temp : temp < arrow.FromActivity.LateFinish ? temp : arrow.FromActivity.LateFinish;


                        default:
                            temp = arrow.ToActivity.LateStart?.AddDays(-arrow.Value);
                            return arrow.FromActivity.LateFinish == null ? temp : temp < arrow.FromActivity.LateFinish ? temp : arrow.FromActivity.LateFinish;

                    }
                default: return null;
            }

        }

        public Activity UpdateNode(Activity oldActivity, Activity newActivity)
        {
            if (this.ActivitesGraph.ContainsKey(oldActivity.Name))
            {
                // this.RemoveFakeNodes();
                oldActivity = this.ActivitesGraph[oldActivity.Name];
                newActivity.OutArrows = oldActivity.OutArrows;
                newActivity.InArrows = oldActivity.InArrows;
                newActivity.Id = oldActivity.Id;
                IsComputed = false;
                // this.ComputeValues();
                // this.RemoveFakeNodes();
                return oldActivity;
            }
            return null;
        }

        public Arrow UpdateArrow(Arrow modifiedArrow)
        {
            if (this.ActivitesGraph.ContainsKey(modifiedArrow.FromActivity.Name) && this.ActivitesGraph.ContainsKey(modifiedArrow.ToActivity.Name))
            {

                var fromActivity = this.ActivitesGraph[modifiedArrow.FromActivity.Name];
                var toActivity = this.ActivitesGraph[modifiedArrow.ToActivity.Name];
                var oldArrow = fromActivity.OutArrows.FirstOrDefault(oa => oa.ToActivityName == toActivity.Id);
                if (oldArrow != null)
                {
                    // this.RemoveFakeNodes();
                    oldArrow.Value = modifiedArrow.Value;
                    IsComputed = false;
                    // this.ComputeValues();
                    // this.RemoveFakeNodes();
                    return oldArrow;
                }
            }
            return null;
        }

        public Activity GetNode(string activityName)
        {
            if (this.ActivitesGraph.ContainsKey(activityName))
            {
                var activity = this.Activites.First(a => a.Name == activityName);
                activity.OutArrows = null;
                activity.InArrows = null;
                return activity;
            }
            return null;
        }

        public bool AddGraph(Activity activity)
        {
            this.AddNode(activity.Clone() as Activity);

            var currentActivities = new List<Activity>();
            var nextActivities = new List<Activity>();

            currentActivities.Add(activity);
            do
            {
                nextActivities.Clear();
                foreach (var currentActivity in currentActivities)
                {
                    foreach (var outArrow in currentActivity.OutArrows)
                    {
                        var arrow = outArrow.Clone() as Arrow;
                        arrow.FromActivity = this.ActivitesGraph[currentActivity.Name];
                        arrow.ToActivity = this.ActivitesGraph.ContainsKey(outArrow.ToActivity.Name) ? this.ActivitesGraph[outArrow.ToActivity.Name] : outArrow.ToActivity.Clone() as Activity;

                        this.AddNode(arrow.ToActivity);
                        this.AddArrow(arrow);
                        // if (this.AddArrow(arrow))
                        // {
                        //     Console.WriteLine(this.ActivitesGraph[arrow.FromActivity.Name].Equals(arrow.FromActivity) + " : " + arrow.FromActivity.Name + " --- " + arrow + " ---> " + this.ActivitesGraph[arrow.ToActivity.Name].Equals(arrow.ToActivity) + " : " + arrow.ToActivity.Name);
                        // }

                        nextActivities.Add(outArrow.ToActivity);
                    }
                }
                currentActivities.Clear();
                currentActivities.AddRange(nextActivities);
            } while (nextActivities.Count > 0);
            return true;
        }

        public void InitializeValues()
        {
            foreach (var activity in this.ActivitesGraph.Values)
            {
                if (activity.InArrows.Count > 0)
                {
                    activity.EarlyStart = null;
                    activity.EarlyFinish = null;
                    activity.LateStart = null;
                    activity.LateFinish = null;
                    activity.TotalFloat = 0;
                    activity.FreeFloat = 0;
                }
            }
        }

        public ICollection<Activity> GetActivitiesGraph()
        {
            return this.ActivitesGraph.Values;
        }
    }
}