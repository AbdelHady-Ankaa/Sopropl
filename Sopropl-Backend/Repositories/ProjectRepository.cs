using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopropl_Backend.Data;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly SoproplDbContext context;
        private readonly INormalizer<string> normalizer;

        public ProjectRepository(SoproplDbContext context)
        {
            this.context = context;
            this.normalizer = new NameNormalizer();
        }

        public async Task<IEnumerable<Project>> AllAsync()
        {
            var projects = await this.context.Projects.ToListAsync();

            return projects;
        }


        public async Task<IAoNGraph> GetActivitiesGraphAsync(Organization organization, Project project)
        {
            var Activities = await this.context.Activities
                .Include(a => a.OutArrows)
                .Include(a => a.InArrows)
                .Where(a => a.NormalizedProjectName == project.NormalizedName && a.Project.NormalizedOrganizationName == organization.NormalizedName)
                .ToListAsync();


            return new AoNGraph(Activities);
        }

        // public async Task<Project> FindProjectInOrganizationOwnedByUserAsync(string userId, string organizationId, string projectId)
        // {
        //     var project = await this.context.Projects
        //         .Include(p => p.Logo)
        //         .FirstOrDefaultAsync(p => p.Id == projectId && p.NormalizedOrganizationName == organizationId);

        //     return project;
        // }

        public async Task<bool> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateWithNameAsync(Organization organization, Project oldProject, Project newProject)
        {

            var p = await this.FindByNameIncludeAllAsync(organization, oldProject.Name);
            newProject.NormalizedName = this.normalizer.Normalize(newProject.Name);
            newProject.Logo = p.Logo;
            newProject.Organization = organization;
            newProject.AccessList = p.AccessList;
            newProject.Activities = p.Activities;
            await this.RemoveAsync(organization, p);
            if (await this.SaveChangesAsync())
            {
                oldProject.DateUpdated = DateTime.Now;
                if (await this.CreateAsync(organization, newProject))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task RemoveAsync(Organization organization, Project project)
        {
            await this.UpdateActivitiesGraphAsync(organization, project, new AoNGraph(new List<Activity>()));
            this.context.Remove(project);
        }

        public async Task<IEnumerable<Resource>> AllActivityResources(Organization organization, Project project, Activity activity)
        {
            var resources = await this.context.Resources.Where(r => r.NormalizedOrganizationName == organization.NormalizedName &&
            r.NormalizedProjectName == project.NormalizedName && r.ActivityName == activity.Name).ToListAsync();
            return resources;
        }
        public async Task<IEnumerable<Resource>> AllActivityResourcesForTeamAsync(Organization organization, Project project, Team team)
        {
            var resources = await this.context.Resources.Where(r => r.NormalizedOrganizationName == organization.NormalizedName &&
                        r.NormalizedProjectName == project.NormalizedName && r.NormalizedTeamName == team.NormalizedName)
                        .ToListAsync();
            return resources;
        }

        public void AssignActivityAsync(Organization organization, Project project, Activity activity, Team team)
        {

            // if (team.NormalizedName == project.NormalizedName)
            // {
            var r = new Resource
            {
                Team = team,
                Activity = activity,
                NormalizedOrganizationName = organization.NormalizedName,
                NormalizedProjectName = project.NormalizedName
            };
            this.context.Resources.Add(r);
            // return true;
            // }
            // else
            // {
            //     var team = await this.context.Teams.FirstOrDefaultAsync(
            //         t => t.NormalizedName == member.Team.NormalizedName &&
            //         t.NormalizedOrganizationName == organization.NormalizedName);
            //     if (team != null)
            //     {
            //         var r = new Resource
            //         {
            //             Team = team,
            //             Activity = activity,
            //             NormalizedOrganizationName = organization.NormalizedName,
            //             NormalizedProjectName = project.NormalizedName
            //         };
            //         this.context.Add(r);
            //         return true;
            //     }
            //     return false;
            // }
        }

        public async Task<bool> UpdateActivitiesGraphAsync(Organization organization, Project project, IAoNGraph newGraph)
        {
            if (newGraph == null)
            {
                return false;
            }
            var activities = await this.context.Activities
                .Include(a => a.InArrows)
                .Include(a => a.OutArrows)
                .Where(
                    a => a.NormalizedProjectName == project.NormalizedName &&
                    a.NormalizedOrganizationName == organization.NormalizedName
                )
                .ToListAsync();
            if (activities.Count > 0)
            {
                activities.ForEach(a =>
                {
                    this.context.Arrows.RemoveRange(a.OutArrows);
                    this.context.Arrows.RemoveRange(a.InArrows);
                });
                this.context.Activities.RemoveRange(activities);
                if (!await this.SaveChangesAsync())
                {
                    return false;
                }
            }
            var newActivities = newGraph.GetActivitiesGraph();

            foreach (var item in newActivities)
            {
                item.Organization = organization;
                item.Project = project;
            }
            this.context.Activities.AddRange(newActivities);
            return true;
        }

        public void UpdateActivitiesGraph(ICollection<Activity> activities)
        {
            this.context.Activities.UpdateRange(activities);
        }

        public void UpdateActivityOrArrow(object entity)
        {
            this.context.Update(entity);//Activities.Update(activity);
        }

        public void RemoveActivity(Activity activity)
        {
            this.context.RemoveRange(activity.InArrows);
            this.context.RemoveRange(activity.OutArrows);
            this.context.Activities.Remove(activity);
        }

        public async Task<bool> CreateAsync(Organization organization, Project project)
        {
            var p = await this.FindByNameAsync(organization, project.Name);
            if (p != null)
            {
                return false;
            }
            project.NormalizedName = this.normalizer.Normalize(project.Name);
            project.Organization = organization;
            this.context.Projects.Add(project);
            return true;
        }

        public async Task<Project> FindByNameAsync(Organization organization, string projectName)
        {
            projectName = this.normalizer.Normalize(projectName);
            var project = await this.context.Projects.Include(p => p.Logo).FirstOrDefaultAsync(p => p.NormalizedOrganizationName == organization.NormalizedName && p.NormalizedName == projectName);

            return project;
        }

        public async Task<bool> ChangeAccessAsync(Organization organization, Project project, Team team, short accessType = AccessType.CONTRIBUTOR)
        {
            if (team != null)
            {
                var access = await this.FindAccessAsync(organization, project, team);
                if (access != null)
                {
                    access.Permission = accessType;
                    return true;
                }
                else
                {
                    access = new Access { Project = project, Team = team, Permission = accessType, NormalizedOrganizationName = organization.NormalizedName };
                    this.context.Add(access);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> RevokeAccessAsync(Organization organization, Project project, Team team)
        {
            var access = await this.FindAccessAsync(organization, project, team);
            if (access != null)
            {
                this.context.Remove(access);
                return true;
            }
            return false;
        }

        public async Task<Access> FindAccessAsync(Organization organization, Project project, Team team)
        {
            var access = await this.context.AccessList.FirstOrDefaultAsync(a =>
            a.NormalizedOrganizationName == organization.Name &&
            a.NormalizedProjectName == project.NormalizedName &&
            a.NormalizedTeamName == team.NormalizedName);

            return access;
        }

        public async Task<Project> FindByNameIncludeAllAsync(Organization organization, string name)
        {
            name = this.normalizer.Normalize(name);
            var project = await this.context.Projects.Include(p => p.AccessList).Include(p => p.Logo).Include(p => p.Activities)
                .FirstOrDefaultAsync(p => p.NormalizedOrganizationName == organization.NormalizedName &&
                p.NormalizedName == name);

            return project;
        }

        public void UpdateWithoutName(Organization organization, Project oldProject, Project newProject)
        {
            oldProject.ShortName = newProject.ShortName;
            oldProject.DateUpdated = DateTime.Now;
            oldProject.Description = oldProject.Description;
            oldProject.ProjectType = newProject.ProjectType;
            this.context.Update(oldProject);
        }

        public async Task<IEnumerable<Project>> AllForTeamAsync(Organization organization, Team team)
        {
            var projects = new List<Project>();
            if (team != null)
            {
                var accessList = await this.context.AccessList.Include(a => a.Project)
                            .Where(a => a.NormalizedOrganizationName == organization.NormalizedName && a.NormalizedTeamName == team.NormalizedName)
                            .ToListAsync();
                foreach (var item in accessList)
                {
                    projects.Add(item.Project);
                }
            }
            return projects;
        }

        public async Task<IEnumerable<Project>> AllForOwnerAsync(Organization organization, Member member)
        {
            var projects = await this.context.Projects
            .Where(p => p.NormalizedOrganizationName == member.NormalizedOrganizationName && member.Type == @short.OWNER)
            .ToListAsync();

            return projects;
        }
    }
}