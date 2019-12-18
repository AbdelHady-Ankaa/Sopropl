using System.Collections.Generic;
using System.Threading.Tasks;
using Sopropl_Backend.Data;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public interface IProjectRepository
    {
        Task<bool> CreateAsync(Organization organization, Project project);
        Task<bool> ChangeAccessAsync(Organization organization, Project project, Team team, short accessType = AccessType.CONTRIBUTOR);
        Task<Access> FindAccessAsync(Organization organization, Project project, Team team);
        Task<bool> RevokeAccessAsync(Organization organization, Project project, Team team);
        Task<Project> FindByNameAsync(Organization organization, string projectName);
        Task<IAoNGraph> GetActivitiesGraphAsync(Organization organization, Project project);
        Task<bool> UpdateActivitiesGraphAsync(Organization organization, Project project, IAoNGraph newGraph);
        void UpdateActivitiesGraph(ICollection<Activity> activities);
        void UpdateActivityOrArrow(object entity);
        void RemoveActivity(Activity activity);
        // Task<Project> FindProjectInOrganizationOwnedByUserAsync(Organization organization, string organizationId, string projectId);
        Task<bool> SaveChangesAsync();
        Task<Project> FindByNameIncludeAllAsync(Organization organization, string name);
        Task<bool> UpdateWithNameAsync(Organization organization, Project oldProject, Project newProject);
        void UpdateWithoutName(Organization organization, Project oldProject, Project newProject);
        Task RemoveAsync(Organization organization, Project project);
        Task<IEnumerable<Project>> AllForTeamAsync(Organization organization, Team team);
        Task<IEnumerable<Project>> AllForOwnerAsync(Organization organization, Member member);
        Task<IEnumerable<Resource>> AllActivityResources(Organization organization, Project project, Activity activity);
        void AssignActivityAsync(Organization organization, Project project, Activity activity, Team team);
    }
}