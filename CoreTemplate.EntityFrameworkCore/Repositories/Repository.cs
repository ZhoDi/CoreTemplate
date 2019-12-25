using CoreTemplate.Domain;
using CoreTemplate.Domain.Extensions;
using CoreTemplate.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemplate.EntityFrameworkCore.Repositories
{
    public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {

        //定义数据访问上下文对象
        protected readonly TempDbContext _dbContext;

        /// <summary>
        /// 通过构造函数注入得到数据上下文对象实例
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(TempDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual DbSet<TEntity> Table => _dbContext.Set<TEntity>();

        #region Select
        public IQueryable<TEntity> GetAll()
        {
            return Table;
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors.IsNullOrEmpty())
            {
                return GetAll();
            }

            var query = GetAll();

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        public TEntity Get(TPrimaryKey id)
        {
            return GetAll().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="startPage">页码</param>
        /// <param name="pageSize">单页数据数</param>
        /// <param name="rowCount">行数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public IQueryable<TEntity> FindPageList(int startPage, int pageSize, out int rowCount, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order = null, string orderType = "asc")
        {
            var result = Table.Where(where);

            if (order != null)
            {
                if (orderType == "desc")
                    result = result.OrderByDescending(order);
                else
                    result = result.OrderBy(order);
            }
            else
            {
                result = result.OrderBy(m => m.Id);
            }
            rowCount = result.Count();
            return result.Skip((startPage - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 通过Sql分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecord"></param>
        /// <param name="sql"></param>
        /// <param name="order"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public IQueryable<TEntity> FindPageListFromSql(int pageIndex, int pageSize, out int totalRecord, string sql, Expression<Func<TEntity, object>> order, string orderType = "asc")
        {
            var _list = Table.FromSql(sql);
            totalRecord = _list.Count();
            if (order != null)
            {
                if (orderType == "asc")
                    _list = _list.OrderBy(order);
                else
                    _list = _list.OrderByDescending(order);
            }

            _list = _list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return _list;
        }

        /// <summary>
        /// 通过Sql查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetFromSql(string sql)
        {
            var _list = Table.FromSql(sql);

            return _list;
        }

        #endregion

        #region Insert
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity, bool autoSave = true)
        {
            Table.Add(entity);
            if (autoSave)
                Save();
            return entity;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entitys"></param>
        public void BatchInsert(List<TEntity> entitys)
        {
            Table.AddRange(entitys);
            Save();
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public TEntity Update(TEntity entity, bool autoSave = true)
        {
            AttachIfNot(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            if (autoSave)
                Save();
            return entity;
        }

        //private void EntityToEntity<T>(T pTargetObjSrc, T pTargetObjDest)
        //{
        //    foreach (var mItem in typeof(T).GetProperties())
        //    {
        //        mItem.SetValue(pTargetObjDest, mItem.GetValue(pTargetObjSrc, new object[] { }), null);
        //    }
        //}
        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public TEntity InsertOrUpdate(TEntity entity, bool autoSave = true)
        {
            if (Get(entity.Id) != null)
                return Update(entity, autoSave);
            return Insert(entity, autoSave);
        }

        /// <summary>
        /// 实体类不存在,追加到上下文中
        /// </summary>
        /// <param name="entity"></param>
        public void AttachIfNot(TEntity entity)
        {
            if (!Table.Local.Contains(entity))
            {
                Table.Attach(entity);
            }
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public void Delete(TEntity entity, bool autoSave = true)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
            if (autoSave)
                Save();
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public void Delete(TPrimaryKey id, bool autoSave = true)
        {
            Table.Remove(Get(id));
            if (autoSave)
                Save();
        }

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        /// <param name="autoSave">是否自动保存</param>
        public void Delete(Expression<Func<TEntity, bool>> predicate, bool autoSave = true)
        {

            DeleteRange(GetAll().Where(predicate));
            if (autoSave)
                Save();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IQueryable<TEntity> entities)
        {
            Table.RemoveRange(entities);
        }


        #endregion


        /// <summary>
        /// 事务性保存
        /// </summary>
        public void Save()
        {
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// 根据主键构建判断表达式
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            //参数
            var lambdaParam = Expression.Parameter(typeof(TEntity));
            //比较==
            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
                );
            //生成表达式
            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

    }
}
