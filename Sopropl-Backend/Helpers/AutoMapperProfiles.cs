using System;
using System.Globalization;
using AutoMapper;
using Sopropl_Backend.DTOs;
using Sopropl_Backend.Models;
using Sopropl_Backend.Repositories;

namespace Sopropl_Backend.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            INormalizer<string> normalizer = new NameNormalizer();
            // map data for output from the server
            CreateMap<User, UserToReturnDTO>();
            CreateMap<Photo, PhotoToReturnDTO>()
            .ForMember(dist => dist.OwnerId,
                options => options.MapFrom(
                    src => src.User != null ? src.User.Id : src.Organization != null ? src.Organization.Id : src.Project.Id
                ));
            CreateMap<Arrow, ArrowToReturnDTO>();
            // .ForMember(dist => dist.FinishToStart, options => options.MapFrom(
            //     src =>
            //         src.FinishToStart != 0 ? src.FinishToStart
            //         : src.FinishToFinish != 0 ? src.FinishToFinish - src.ToActivity.Duration
            //         : src.StartToFinish != 0 ? src.StartToFinish - src.ToActivity.Duration
            //                                                      + src.FromActivity.Duration
            //         : src.ConstraintValue != 0 ? src.ConstraintValue + src.FromActivity.Duration : src.FinishToStart
            // ));
            CreateMap<Activity, ActivityToReturnDTO>()
            .ForMember(dist => dist.EarlyStart, opts => opts.MapFrom(
                src => src.Name != AoNGraph.FAKE_START_ACTIVITY_NAME ?
                src.EarlyStart.Value.ToUniversalTime().ToString(new CultureInfo("ja-JP")) :
                DateTime.Now.ToUniversalTime().ToString(new CultureInfo("ja-JP"))
            ));
            CreateMap<User, UserProfileDTO>();
            CreateMap<Project, ProjectToReturnDTO>();
            CreateMap<Notification, NotificationDTO>();
            CreateMap<Team, TeamToReturnDTO>();
            CreateMap<Access, AccessToReturnDTO>();

            // map data for input to the server   
            CreateMap<TeamForCreateDTO, Team>();
            CreateMap<UserForLoginDTO, User>();
            CreateMap<UserForRegisterDTO, User>();

            CreateMap<OrganizationForCreationDTO, Organization>();

            CreateMap<ProjectForCreationDTO, Project>();
            CreateMap<ActivityForCreationDTO, Activity>();
            CreateMap<ArrowForCreationDTO, Arrow>();
            CreateMap<ArrowForCreationDTO2, Arrow>();
            CreateMap<ActivityArrowForCreationDTO, Activity>();
            CreateMap<UserProfileDTO, User>();

            CreateMap<Organization, OrganizationToReturnDTO>().ForMember(dist => dist.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}