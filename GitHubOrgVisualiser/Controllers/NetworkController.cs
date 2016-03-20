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
    public class NetworkController : ApiController
    {
        // GET api/values
        [Route("Network/{orgName}")]
        public async Task<NetworkData> GetNetworkData(string orgName)
        {

            var token = new Credentials("b4d4ee29cbb858da8cb54a3ca80ebb5bc119f2c3");
            var client = new GitHubClient(new ProductHeaderValue("pugs-not-drugs")) { Credentials = token };

            var orgMembers = await client.Organization.Member.GetAll(orgName);


            var networkData = new  NetworkData();
            networkData.NetworkCollabs = orgMembers.Select(x => new NetworkCollab
            {
                Name = x.Login,
                AvatarUrl = x.AvatarUrl
            }).ToList();

            foreach (var member in orgMembers)
            {
                var following = await client.User.Followers.GetAllFollowing(member.Login);
                var filtered = following.Where(x => orgMembers.Any(m => m.Login == x.Login));

                networkData.Network.Add(member.Login, filtered.Select(x=> x.Login).ToList());
            }

            return networkData;
        }
    }
}
