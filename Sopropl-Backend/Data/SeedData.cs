using System;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;
using Sopropl_Backend.Repositories;

namespace Sopropl_Backend.Data
{
    public static class SeedData
    {
        private static T GetAppService<T>(IApplicationBuilder app)
        {
            var scopedFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var scope = scopedFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }
        public static void seed(this IApplicationBuilder app)
        {
            INormalizer<string> norm = new NameNormalizer();
            IAuthManager authRepository = (IAuthManager)GetAppService<IAuthManager>(app);
            var orgs = new Collection<Organization>();
            var acts = new Collection<Activity>();
            var A = new Activity
            {
                Name = "A",
                EarlyStart = DateTime.Parse("2019-05-12T08:00:00"),
                Duration = 10
            };

            var B = new Activity
            {
                Name = "B",
                Duration = 12
            };


            var C = new Activity
            {
                Name = "C",
                Duration = 9
            };

            var D = new Activity
            {
                Name = "D",
                Duration = 16
            };


            var E = new Activity
            {
                Name = "E",
                Duration = 11
            };


            var F = new Activity
            {
                Name = "F",
                Duration = 4
            };


            var G = new Activity
            {
                Name = "G",
                Duration = 14,
            };


            var H = new Activity
            {
                Name = "H",
                Duration = 16
            };


            var AC = new Arrow
            {
                FromActivity = A,
                ToActivity = C,
                Value = 3,
                Type = ArrowType.START_TO_START
            };
            A.OutArrows.Add(AC);
            var AB = new Arrow
            {
                FromActivity = A,
                ToActivity = B,
                Value = 4,
                Type = ArrowType.FINISH_TO_START
            };
            A.OutArrows.Add(AB);
            var BF = new Arrow
            {
                FromActivity = B,
                ToActivity = F,
                Value = 8,
                Type = ArrowType.FINISH_TO_FINISH
            };
            B.OutArrows.Add(BF);
            var CE = new Arrow
            {
                FromActivity = C,
                ToActivity = E,
                Value = 17,
                Type = ArrowType.START_TO_START
            };
            C.OutArrows.Add(CE);
            var CD = new Arrow
            {
                FromActivity = C,
                ToActivity = D,
                Value = 0,
                Type = ArrowType.FINISH_TO_START
            };
            C.OutArrows.Add(CD);
            var EF = new Arrow
            {
                FromActivity = E,
                ToActivity = F,
                Value = 3,
                Type = ArrowType.START_TO_START
            };

            E.OutArrows.Add(EF);
            var FH = new Arrow
            {
                FromActivity = F,
                ToActivity = H,
                Value = 0,
                Type = ArrowType.FINISH_TO_START
            };
            F.OutArrows.Add(FH);
            var DG = new Arrow
            {
                FromActivity = D,
                ToActivity = G,
                Value = 0,
                Type = ArrowType.FINISH_TO_START
            };
            D.OutArrows.Add(DG);

            var GH = new Arrow
            {
                FromActivity = G,
                ToActivity = H,
                Value = 3,
                Type = ArrowType.FINISH_TO_FINISH
            };
            G.OutArrows.Add(GH);
            acts.Add(A);
            acts.Add(B);
            acts.Add(C);
            acts.Add(D);
            acts.Add(E);
            acts.Add(F);
            acts.Add(G);
            acts.Add(H);


            var projects = new Collection<Project>();
            var project = new Project
            {
                Name = "Dating App",
                NormalizedName = norm.Normalize("Dating App"),
                Activities = acts
            };
            projects.Add(project);
            var teams = new Collection<Team>();

            teams.Add(new Team
            {
                NormalizedName = "developers",
                Name = norm.Normalize("developers")
            });
            var org = new Organization
            {
                Name = "phoenix",
                NormalizedName = norm.Normalize("phoenix"),
                Projects = projects,
                Teams = teams
            };
            orgs.Add(org);

            var noti = new Notification { Title = "run" };
            var notis = new Collection<Notification>();
            notis.Add(noti);

            User user = new User
            {
                UserName = "AbdelHady",
                Country = "Syria",
                City = "Homs",
                Email = "aboghazi@gmail.com",
                PhoneNumber = "0993456585",
                Notifications = notis
            };

            user.Members.Add(new Member { Type = @short.OWNER, Organiztion = org });

            authRepository.Register(user, "P@$$w0rd");


            var user2 = new User
            {
                UserName = "AbdelHady-PH",
                Country = "Syria",
                City = "Homs",
                Email = "aboghazi@gmail.com",
                PhoneNumber = "0993456585",
            };

            // var access = new Access{}

        }
    }
}