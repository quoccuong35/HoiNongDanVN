
using Microsoft.EntityFrameworkCore;
using HoiNongDan.Models.Entitys;
using System.Linq.Expressions;


namespace HoiNongDan.DataAccess.Repository
{
    public class HistoryModelRepository : IHistoryModelRepository
    {
        private AppDbContext _db;
        public HistoryModelRepository(AppDbContext db)
        {
            _db = db;
        }
        public const string LastModifiedTime = "lastmodifiedtime";
        public void SaveUpdateHistory<T>(string id, Guid accountId, T obj, params Expression<Func<T, object>>[] propertiesToUpdate) where T : class
        {
            var dbEntityEntry = _db.Entry(obj);
            string tableName = dbEntityEntry.Entity.GetType().Name;

            if (propertiesToUpdate.Any())
            {
                //update explicitly mentioned properties
                foreach (var property in propertiesToUpdate)
                {
                    dbEntityEntry.Property(property).IsModified = true;
                }
            }
            else
            {
                //no items mentioned, so find out the updated entries
                var dateTime = DateTime.Now;
                foreach (var property in dbEntityEntry.OriginalValues.Properties)
                {
                    string nameField = property.Name;
                    
                    var original = dbEntityEntry.OriginalValues[nameField];
                    var current = dbEntityEntry.CurrentValues[nameField];
                    if (original != null && !original.Equals(current) && nameField.ToLower() != LastModifiedTime)
                    {
                        //dbEntityEntry.Property(property).IsModified = true;

                        //NOTE: MUST USING VIEW MODEL
                        #region Insert into HistoryModel
                        HistoryModel model = new HistoryModel();
                        model.HistoryModifyId = Guid.NewGuid();
                        //get PageId
                        model.Key = id;
                        model.FieldName = nameField;
                        model.OldData = original != null ? original.ToString() : "";
                        model.NewData = current != null ? current.ToString() : "";
                        model.TableName = tableName;
                        model.ModifiedAccountId = accountId;
                        model.ModifiedTime = dateTime;
                        _db.Entry(model).State = EntityState.Added;
                        #endregion Insert into HistoryModel
                    }
                }
            }
        }
    }
}
