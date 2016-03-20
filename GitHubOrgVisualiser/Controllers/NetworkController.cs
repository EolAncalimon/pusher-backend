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

            var orgRepos = await client.Repository.GetAllForOrg(orgName);

            var networkData = new  NetworkData();
            foreach (var repo in orgRepos)
            {
                var contributors = await client.Repository.GetAllContributors(orgName, repo.Name);
                networkData.NetworkCollabs.AddRange(contributors.Select(collab => new NetworkCollab
                {
                    Name = collab.Login,
                    AvatarUrl = collab.AvatarUrl
                }).ToList());

                networkData.Network.Add(repo.Name, contributors.Select(x=> x.Login).ToList());
            }

            networkData.NetworkCollabs = networkData.NetworkCollabs.GroupBy(x => x.Name).Select(y => y.First()).ToList();
            return networkData;
        }
    }
}
