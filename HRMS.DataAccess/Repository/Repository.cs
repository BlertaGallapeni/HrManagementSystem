﻿using AutoMapper;
using HRMS.DataAccess.Data;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(AppDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        public void Update(T entity)
        {
            dbSet.Update(entity);

        }
        public void Delete(T entity)
        {         
            entity.GetType().GetProperty("Deleted").SetValue(entity, true);
            entity.GetType().GetProperty("DeletedDate").SetValue(entity, DateTime.Now);
            dbSet.Update(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }
  
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true)
        {

            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);

        }
        public List<KeyValuePair<int, string>> GetStatusListByLang(string culture, string q, string type)
        {
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                return _db.Status.Where(x => x.StatusType.Contains(type) && culture == "sq-AL" ? x.NameAl.ToLower().StartsWith(q.ToLower()) : x.Name.ToLower().StartsWith(q.ToLower())).Select(x => new KeyValuePair<int, string>(x.Id, culture == "sq-AL" ? x.NameAl : x.Name)).ToList();
            }
            else
            {
                return _db.Status.Where(x => x.StatusType.Contains(type)).Select(x => new KeyValuePair<int, string>(x.Id, culture == "sq-AL" ? x.NameAl : x.Name)).ToList();
            }
        }

    }
}
