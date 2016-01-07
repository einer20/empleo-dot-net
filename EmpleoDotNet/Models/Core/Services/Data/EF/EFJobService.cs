using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PagedList;
using EmpleoDotNet.Models.Dto;

namespace EmpleoDotNet.Models.Core.Services.Data.EF
{
    internal class EFJobService : BaseEFService<JobOpportunity>, IJobsService
    {
        public EFJobService()
        {
           
        }

        public IEnumerable<JobOpportunity> GetAllJobs()
        {
            var jobs = this.GetAll().Include(x => x.Location);
            return jobs;
        }



        public IEnumerable<JobOpportunity> GetJobsByCategory(JobCategory category)
        {
            return GetAllJobs().Where(x => x.Category == category);
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        public JobOpportunity GetJobById(int id)
        {
            return this.FindById(id);
        }

        protected override JobOpportunity FindById(int id)
        {
            var job = this.DbSet.Include(x => x.Location).FirstOrDefault(x => x.Id == id); ;
            return job;
        }

        public List<JobOpportunity> GetAllJobOpportunitiesByLocation(Location location)
        {
            var jobOpportunities = this.DbSet.Where(x => x.LocationId == location.Id).ToList();

            return jobOpportunities;
        }

        public IPagedList<JobOpportunity> GetAllJobOpportunitiesByLocationPaged(JobOpportunityPagingParameter parameter)
        {
            IPagedList<JobOpportunity> result;

            if (parameter.Page <= 0)
                parameter.Page = 1;

            if (parameter.PageSize <= 0)
                parameter.PageSize = 15;

            var jobs = DbSet;

            if (parameter.SelectedLocation <= 0)
            {
                result = jobs.Include(x => x.Location)
                    .OrderByDescending(x => x.Id)
                    .ToPagedList(parameter.Page, parameter.PageSize);
            }
            else
            {
                result = DbSet.Include(x => x.Location)
                    .Where(x => x.LocationId.Equals(parameter.SelectedLocation))
                    .OrderByDescending(x => x.Id)
                    .ToPagedList(parameter.Page, parameter.PageSize);
            }

            return result;
        }

        public IEnumerable<Location> GetAllJobsLocations()
        {

            return this.Context.Locations.AsEnumerable();
        }
    }
}