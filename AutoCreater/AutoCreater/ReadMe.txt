1. 配置好config中的数据库连接 AutoCreaterCon 为项目需要使用的数据库(可以随便建个空库使用) ,ConnStr 为创建table时的目标数据库连接
2.在AutoCreaterCon连接的数据库中执行以下sql
//添加菜单 
create table Menus
 (
 ID uniqueidentifier primary key,
 MenuName nvarchar(32),
 MenuExplain nvarchar(512),
 MenuUrl varchar(128),
 IsActive bit,
 ParentID uniqueidentifier,
 _Index int
 )
 go

 insert into Menus values(newid(),N'自动创建',N'配置并自动创建页面,DLL,BLL','CreateTable',1,null,1)


 //添加支持数据类型
 
  create table DBType(
  DBType varchar(32),
  IsLength bit,
  Explain nvarchar(256)
  )
  go
   insert into DBType
   select 'varchar',1,null
   union all
   select 'nvarchar',1,null
   union all
   select 'decimal',1,null
   union all
   select 'int',0,null
   union all
   select 'bigint',0,null
   union all
   select 'bit',0,null
   union all
   select 'datetime',0,null
   union all
   select 'date',0,null
    union all
   select 'text',0,null
