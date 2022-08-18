using CommLibrary;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Base;


namespace DAL.CreateTable
{
    /// <summary>
    /// 基础数据查询类
    /// </summary>
    public class BaseQueryDAL
    {
        /// <summary>
        /// 获取数据库所有table
        /// </summary>
        /// <returns></returns>
        public DataTable GetTables()
        {
            return DBHelper.db.Ado.GetDataTable(" select TableName=name From  sys.tables ");
        }

        public DataTable GetFields(string tableName)
        {
            // select FieldName=a.name,DbType=b.name From sys.columns a,systypes  b where object_id=object_id(@name) and b.xusertype=a.user_type_id
            return DBHelper.db.Ado.GetDataTable(@" 
 select row_number() over(order by a.column_id) rownumber, FieldName=a.name,DbType=t.name,[Length]=
(case
when t.name in (N'decimal', N'numeric') then  Convert(varchar(10),a.precision) + ',' + Convert(varchar(10),a.scale)
when t.name in (N'varchar', N'nvarchar', N'char', N'nchar') then  Convert(varchar(10),a.max_length)
else null end)
,
Nullable=a.is_nullable ,
[Identity]= a.is_identity ,
isnull(def.text,N'') defalut_value,
isnull(ft.name + '.' + fc.name,N'') foreig_key,
isnull(ep.value,N'') Explain,
cast(CASE WHEN EXISTS ( SELECT   1
                           FROM     dbo.sysindexes si
                                    INNER JOIN dbo.sysindexkeys sik ON si.id = sik.id
                                                              AND si.indid = sik.indid
                                    INNER JOIN dbo.syscolumns sc ON sc.id = sik.id
                                                              AND sc.colid = sik.colid
                                    INNER JOIN dbo.sysobjects so ON so.name = si.name
                                                              AND so.xtype = 'PK'
                           WHERE    sc.id = sc_.id
                                    AND sc.colid = sc_.colid ) THEN 1
             ELSE 0
        END as bit) AS PrimaryKey
from sys.columns a
left join sys.types t on t.user_type_id = a.user_type_id
left join sys.syscolumns sc_ on sc_.id = a.object_id and sc_.colid = a.column_id
left join sys.syscomments def on def.id = sc_.cdefault
left join sys.foreign_key_columns fkc on fkc.parent_object_id = a.object_id and fkc.parent_column_id = a.column_id
left join sys.columns fc on fc.object_id = fkc.referenced_object_id and fc.column_id = fkc.referenced_column_id
left join sys.tables ft on ft.object_id = fkc.referenced_object_id
left join sys.extended_properties  ep on ep.major_id = a.object_id and ep.minor_id = a.column_id and ep.name = N'MS_Description'
where a.object_id=object_id(@name, N'U')   ", new SugarParameter("@name", tableName));

        }

        public PageInfo GetFields(Dictionary<string, string> dic, PageInfo pi)
        {
            List<SugarParameter> pars = new List<SugarParameter> { new SugarParameter("@pi", pi.PageIndex), new SugarParameter("@pz", pi.PageSize) };
            List<string> where = new List<string>();
            int i = 0;
            foreach (var item in dic)
            {
                where.Add($" { item.Key}=object_id(@par{i})");
                pars.Add(new SugarParameter("@par" + i, item.Value));
                i++;
            }
           
            pars.Add(new SugarParameter("@count", null, true));
            pi.Data = DBHelper.db.Ado.GetDataTable($" select FieldName=name,object_id into #k From sys.columns where {string.Join(" and ",where)} ;select @count=count(1) From #k ;select * From #k order by {pi.SortExpression} offset  (@pi-1)*@pz rows fetch next @pz rows only  ;drop table #k", pars);
            pi = new PageInfo() {Data=pi.Data };
            pi.TotalRowNum = Convert.ToInt32(pars.Last().Value);
            return pi;

        }
    }
}
