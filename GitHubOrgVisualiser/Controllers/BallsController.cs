using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using GitHubOrgVisualiser.Models;
using Octokit;
using PusherServer;

namespace GitHubOrgVisualiser.Controllers
{
    public class BallsController : ApiController
    {
        [Route("balls/{orgName}")]
        public async Task<List<BallsData>> GetBallData(string orgName)
        {

            var token = new Credentials("b4d4ee29cbb858da8cb54a3ca80ebb5bc119f2c3");
            var client = new GitHubClient(new ProductHeaderValue("pugs-not-drugs")) { Credentials = token };

            var members = await client.Organization.Member.GetAll(orgName);
            var allFollowings = new List<User>();

            foreach (var member in members)
            {
                var following = await client.User.Followers.GetAllFollowing(member.Login);

                allFollowings.AddRange(following);
            }

            var groupedFollowings = allFollowings.GroupBy(x => x.Login);

            return groupedFollowings.Select(grouping => new BallsData
            {
                Name = grouping.Key,
                Count = grouping.ToList().Count
            }).ToList();
        }
    }
}
