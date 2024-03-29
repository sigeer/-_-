﻿using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DDDEF.EntityLog
{
    public static class EntityLogExtension
    {
        public static EntityLog? GenerateRecord(this DbContext dbContext, string? tableName, int tablePrimaryId, EntityLogType type, object? oldObj, object? newObj, int userId)
        {
            if (string.IsNullOrEmpty(tableName))
                return null;

            if (oldObj == null && newObj == null)
                return null;

            string? newValueList = "{";
            string? oldValueList = "{";
            object? oldValue;
            object? newValue;
            Type ejType = oldObj == null ? newObj!.GetType() : oldObj.GetType();

            foreach (PropertyInfo p in ejType.GetProperties())
            {
                oldValue = oldObj == null ? null : p.GetValue(oldObj);
                newValue = newObj == null ? null : p.GetValue(newObj);
                if (oldValue != null || newValue != null)
                {
                    if ((oldValue == null) || (newValue == null))
                    {
                        if (newObj == null)
                        {
                            newValueList = null;
                        }
                        else
                        {
                            if (newValue == null)
                            {
                                newValueList += p.Name + ":null,";
                            }
                            else
                            {
                                newValueList += p.Name + ":'" + newValue.ToString() + "',";
                            }
                        }

                        if (oldObj == null)
                        {
                            oldValueList = null;
                        }
                        else
                        {
                            if (oldValue == null)
                            {
                                oldValueList += p.Name + ":null,";
                            }
                            else
                            {
                                oldValueList += p.Name + ":'" + oldValue.ToString() + "',";
                            }
                        }

                    }
                    else
                    {
                        if (oldValue.GetType().Name == "Decimal")
                        {
                            if (Convert.ToDecimal(oldValue) != Convert.ToDecimal(newValue))
                            {
                                newValueList += p.Name + ":'" + newValue.ToString() + "',";
                                oldValueList += p.Name + ":'" + oldValue.ToString() + "',";
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (oldValue.ToString() != newValue.ToString())
                        {
                            //if oldvalue is not null and newvalue is not null,tostring
                            newValueList += p.Name + ":'" + newValue.ToString() + "',";
                            oldValueList += p.Name + ":'" + oldValue.ToString() + "',";
                        }
                    }
                }
            }

            if (newValueList == oldValueList && type != EntityLogType.Delete)
                return null;

            if (string.IsNullOrEmpty(newValueList) && type != EntityLogType.Delete)
                return null;

            var oldPairs = oldValueList == null ? null : oldValueList.Substring(0, oldValueList.Length - 1) + "}";
            var newPairs = newValueList == null ? null : newValueList?.Substring(0, newValueList.Length - 1) + "}";
            var dbModel = new EntityLog(tableName, type, tablePrimaryId, oldPairs, newPairs, userId);

            // 这里修改entity log的保存方式
            dbContext.Database.ExecuteSql($"insert into table_log (`TableName`, `TablePrimaryId`, `Type`, `OldValuePairs`, `NewValuePairs`, `UserId`, `CreateTime`) values({dbModel.TableName}, {dbModel.TablePrimaryId}, {dbModel.Type}, {dbModel.OldValuePairs}, {dbModel.NewValuePairs}, {dbModel.UserId}, {dbModel.CreationTime});");
            return dbModel;
        }

        public static EntityLogType GetTypeFromEntityState(this EntityState state)
        {
            switch (state)
            {
                case EntityState.Modified:
                    return EntityLogType.Update;
                case EntityState.Deleted:
                    return EntityLogType.Delete;
                case EntityState.Added:
                    return EntityLogType.Add;
                default:
                    return EntityLogType.None;
            }
        }
    }
}
