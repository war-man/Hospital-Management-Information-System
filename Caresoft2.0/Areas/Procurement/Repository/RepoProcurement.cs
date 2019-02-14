using Caresoft2._0.Areas.Procurement.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.ModelBinding;

namespace Caresoft2._0.Areas.Procurement.Repository
{
    public class RepoProcurement<TEntity> where TEntity : class
    {
        ProcurementDbContext db;

        public RepoProcurement()
        {
            db = new ProcurementDbContext();
        }
        public IEnumerable<TEntity> GetAll()
        {
            var data = db.Set<TEntity>().ToList();
            return data;
        }

        public TEntity Get(int Id)
        {
            var data = db.Set<TEntity>().Find(Id);
            return data;
        } 

        public void Add(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity,bool>> predicate)
        {
            var data = db.Set<TEntity>().Where(predicate);
            return data;
        }

        public void Remove(TEntity entity)
        {
            db.Set<TEntity>().Remove(entity);
        }

    }
}