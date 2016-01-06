using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EmpleoDotNet.Models.Core.Services.Data.EF
{
    /// <summary>
    /// Provides the basic method for access to a model data
    /// </summary>
    internal abstract class BaseEFService<T> where T : class
    {

        

        public BaseEFService()
        {
            this.Context = new EmpleadoContext();
            this.DbSet = this.Context.Set<T>();
        }

        /// <summary>
        /// Get the current DbContext instance
        /// </summary>
        protected readonly EmpleadoContext Context;

        /// <summary>
        /// Get the dbset of the current entity
        /// </summary>
        protected readonly DbSet<T> DbSet;

        protected virtual T FindById(int id)
        {
            return this.DbSet.Find(id);
        }

        protected IQueryable<T> GetAll()
        {
            return this.DbSet;
        }

        
    }
}