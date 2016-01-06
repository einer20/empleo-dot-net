using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

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
    }
}