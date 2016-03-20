
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GitHubOrgVisualiser.Models;
using Octokit;
using PusherServer;

namespace GitHubOrgVisualiser.Controllers
{
    public class RadarController : ApiController
    {
        // GET api/values
        [Route("Radar/{orgName}")]
        public async Task GetRadarPoints(string orgName)
        {
            var pusher = new Pusher("189287", "9ba0fdd2241a9d87841e", "fc6c5aa5828c75f7c4fa");

            var token = new Credentials("b4d4ee29cbb858da8cb54a3ca80ebb5bc119f2c3");
            var client = new GitHubClient(new ProductHeaderValue("pugs-not-drugs")) {Credentials = token};

            
            var orgRepos = await client.Repository.GetAllForOrg(orgName);
            var orgReposGrouped = orgRepos.GroupBy(x => x.Language);

            var members = await client.Organization.Member.GetAll(orgName);
            List<Repository> allMemberRepos = new List<Repository>();

            foreach (var member in members)
            {
                var memberRepos = await client.Repository.GetAllForUser(member.Login);
                allMemberRepos.AddRange(memberRepos);
            }
            var memberReposGrouped = allMemberRepos.GroupBy(x => x.Language);

            var radarData = new RadarData
            {
                OrgData = orgReposGrouped.Select(grouping => new RadarPoint
                {
                    Language = grouping.Key,
                    Count = grouping.ToList().Count
                }).ToList(),
                MemberData = memberReposGrouped.Select(grouping => new RadarPoint
                {
                    Language = grouping.Key,
                    Count = grouping.ToList().Count
                }).ToList(),
            };

            pusher.Trigger("pugs", "updatedRadar", radarData);
        }
    }
}
