using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public class SqlDAL
    {
        /// <summary>
        /// 根据商品编码查询商品信息
        /// </summary>
        /// <param name="cInvCode"></param>
        /// <returns></returns>
        public DataSet GetGoodByCode(string cInvCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select c.cComUnitName as cinva_unit,ComputationUnit.cComUnitName as UnitName,ComputationUnit.iChangRate,Inventory.* from Inventory");
            sb.Append(" left join ComputationUnit on Inventory.cComUnitCode=ComputationUnit.cComunitCode  ");
            sb.Append(" left join ComputationUnit as c on c.cComunitCode=Inventory.cSAComUnitCode");
            sb.Append(" where cInvCode='" + cInvCode + "'");
            DataSet ds = SqlHelper.ExcuteDataSet(sb.ToString());
            return ds;
        }

        /// <summary>
        /// 材料出库单-根据生产订单编码查询生产订单信息
        /// </summary>
        /// <param name="cInvCode"></param>
        /// <returns></returns>
        public DataSet GetMomOrder(string mocode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("if exists ( select 0 where not object_id('tempdb..#STRefIDs') is null ) drop table #STRefIDs ");
            sb.AppendLine();
            sb.Append("if exists ( select 0 where not object_id('tempdb..#STRefID') is null ) drop table #STRefID ");
            sb.AppendLine();
            sb.Append(@"select IDENTITY(int) as tmpId,convert(int,0)  as M_ID ,convert(int,0) as   S_ID,convert(money,ufts) as oriufts ,
convert(int,0) as m_ST_moid into #STRefIDs from  v_st_mom_orderdetail where 1=0  ");
            sb.AppendLine();
            sb.Append("create clustered index ix_STRefIDs_tmpid_870 on #STRefIDs( M_ID,S_ID,tmpId )  ");
            sb.AppendLine();
            sb.Append(@"insert into #STRefIDs(M_ID ,S_ID,oriufts,m_ST_moid) select v_st_mom_orderdetail.modid,allocateid,convert(money,v_st_mom_orderdetail.ufts),
  v_st_mom_orderdetail.moid FROM  v_mom_orderdetail_st v_st_mom_orderdetail   left join  (SELECT  modid as m_modid,allocateid,CREJECTCODE from 
   v_st_moallocate   where isnull(WIPType,0)=3 and isnull(ByproductFlag,0)=0 and (ISNULL(Qty,0)-ISNULL(IssQty,0)+ISNULL(ReplenishQty,0))>0 
    and isnull(RequisitionFlag,0) = 0  and 1=1) v_mom_moallocate ON v_st_mom_orderdetail.modid=  v_mom_moallocate.m_modid Where
     v_st_mom_orderdetail.byproductflag=0 and ISNULL(v_st_mom_orderdetail.Status,0) = 3 and ( isnull(v_st_mom_orderdetail.bWorkshopTrans,0)
      = 0 or (isnull(v_st_mom_orderdetail.bWorkshopTrans,0) =1 and isnull(v_st_mom_orderdetail.sfcflag,0) =1)) and
       isnull(v_mom_moallocate.m_modid,0)<>0  AND  (1>0) AND (1>0)  AND ( 1>0 )   ");
            sb.AppendLine();
            sb.Append(@"select IDENTITY(int) as tmpId,M_ID,max(oriufts) as oriufts,max(m_ST_moid) as  m_ST_moid into #STRefID 
from #STRefIDs group by M_ID ");
            sb.AppendLine();
            sb.Append("create clustered index ix_STRefID_tmpid_870 on #STRefID( tmpId,M_ID ) ");
            sb.AppendLine();
            sb.Append(@" select '' as selcol,sotype,MDept,MoCode,MoSeq,DeptName,invcode,invaddcode,SoCode,invname,SoSeq,invstd,invdefine1,
     invdefine2,invdefine3,invdefine4,invdefine5,invdefine6,invdefine7,invdefine8,invdefine9,invdefine10,invdefine11,
     invdefine12,invdefine13,invdefine14,invdefine15,invdefine16,unitId,unitcode,auxunitid,auxunitcode,free1,
     free2,free3,free4,free5,free6,free7,free8,free9,free10,Startdate,DueDate,Auxqty,impqty,QualifiedInQty,
     ((isnull(impQty,0)-isnull(QualifiedInQty,0))) as iqty,maker,SoDId,MoDId,ufts,MoLotcode,OrderCode,OrderDId,
     OrderSeq,OrderType,cusname,cuscode,cusabbname,cservicecode,remark,motypecode,motypedesc,define22,define23,
     define24,define25,define26,define27,define28,define29,define30,define31,define32,define33,define34,define35
     ,define36,define37,lplancode FROM  v_st_mom_orderdetail    where modid in 
     (select m_id from #STRefID where tmpId > 0 and tmpId < 501) and v_st_mom_orderdetail.byproductflag=0 
     and MoCode='" + mocode + "'");
            DataSet ds = SqlHelper.ExcuteDataSet(sb.ToString());
            return ds;
        }

        /// <summary>
        /// 产成品入库单-根据生产订单编码查询生产订单信息
        /// </summary>
        /// <param name="cInvCode"></param>
        /// <returns></returns>
        public DataSet GetMomOrderByProductIn(string mocode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from mom_order");
            sb.Append(" where MoCode='" + mocode + "'");
            DataSet ds = SqlHelper.ExcuteDataSet(sb.ToString());
            return ds;
        }
        /// <summary>
        /// 根据生产订单编码查询生产订单明细信息
        /// </summary>
        /// <param name="cInvCode"></param>
        /// <returns></returns>
        public DataSet GetMomDetail(string mocode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from mom_orderdetail");
            sb.Append(" where Moid=(select MoId from mom_order  where MoCode='" + mocode + "')");
           
            DataSet ds = SqlHelper.ExcuteDataSet(sb.ToString());
            return ds;
        }
        /// <summary>
        /// 材料出库单-根据生产订单编码查询生产订单明细信息
        /// </summary>
        /// <param name="cInvCode"></param>
        /// <returns></returns>
        public DataSet GetMomOrderDetails(string mocode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("if exists ( select 0 where not object_id('tempdb..#STRefIDs') is null ) drop table #STRefIDs ");
            sb.AppendLine();
            sb.Append("select IDENTITY(int) as tmpId,convert(int,0)  as M_ID ,convert(int,0) as   S_ID,convert(money,ufts) as oriufts ,convert(int,0) as m_ST_moid into #STRefIDs from  v_st_mom_orderdetail where 1=0");
            sb.AppendLine();
            sb.Append(@"insert into #STRefIDs(M_ID ,S_ID,oriufts,m_ST_moid)  
select v_st_mom_orderdetail.modid,allocateid,convert(money,v_st_mom_orderdetail.ufts),v_st_mom_orderdetail.moid FROM  v_mom_orderdetail_st v_st_mom_orderdetail   
left join  (SELECT  modid as m_modid,allocateid,CREJECTCODE from  v_st_moallocate   where isnull(WIPType,0)=3 and isnull(ByproductFlag,0)=0 
and (ISNULL(Qty,0)-ISNULL(IssQty,0)+ISNULL(ReplenishQty,0))>0  and isnull(RequisitionFlag,0) = 0  and 1=1) v_mom_moallocate ON v_st_mom_orderdetail.modid=  v_mom_moallocate.m_modid 
Where v_st_mom_orderdetail.byproductflag=0 and ISNULL(v_st_mom_orderdetail.Status,0) = 3 and ( isnull(v_st_mom_orderdetail.bWorkshopTrans,0) = 0 
or (isnull(v_st_mom_orderdetail.bWorkshopTrans,0) =1 and isnull(v_st_mom_orderdetail.sfcflag,0) =1)) and isnull(v_mom_moallocate.m_modid,0)<>0  AND  
(1>0) AND (1>0)  AND ( 1=1 )");
            sb.AppendLine();
            sb.Append(@"select soseq,'' as selcol,wccode,wcname,whcode,whname,Dept,DeptName,invcode,invaddcode,invname,invstd,invdefine1,invdefine2,invdefine3,invdefine4,(ftransqty) 
as ftransqty,invdefine5,(ftransnum) as ftransnum,invdefine6,invdefine7,invdefine8,invdefine9,invdefine10,invdefine11,invdefine12,invdefine13,invdefine14,
invdefine15,invdefine16,unitcode,unitname,BomQty,free1,free2,free3,free4,free5,free6,free7,free8,free9,free10,LotNo,DemDate,Qty,((isnull(IssQty,0)-isnull(ReplenishQty,0))) 
as IssQty,((ISNULL(Qty,0)-ISNULL(IssQty,0)+isnull(ReplenishQty,0))) as IsnQty,OpSeq,OpDecs,MoDId,AllocateId,ufts,WIPType,cassunit,cinva_unit,iinvexchrate,
itnum,isnum,iunnum,CostItemCode,CostItemName,(convert(decimal(38,6),Null)) as fstockanum,(convert(decimal(38,6),Null)) as fstockaqty,(convert(decimal(38,6),Null))
 as fstocknum,(convert(decimal(38,6),Null)) as fstockqty,ReplenishQty,define22,define23,define24,define25,define26,define27,define28,define29,define30,define31,
 define32,define33,define34,define35,define36,define37,istsodid,istsotype,crejectcode,cCiqBookCode,cbatchproperty1,cbatchproperty2,cbatchproperty3,cbatchproperty4,
 cbatchproperty5,cbatchproperty6,cbatchproperty7,cbatchproperty8,cbatchproperty9,cbatchproperty10,cbmemo FROM v_st_moallocate with (nolock)  
  inner join #STRefIDs b on v_st_moallocate.modid=b.m_id and v_st_moallocate.allocateid=b.s_id where b.m_id in (
  select MoDId from v_st_mom_orderdetail where  MoCode='" + mocode + "')");
            DataSet ds = SqlHelper.ExcuteDataSet(sb.ToString());
            return ds;
        }

        /// <summary>
        ///产成品入库单中根据生产订单编码查询红蓝订单明细信息
        /// </summary>
        /// <param name="cInvCode"></param>
        /// <returns></returns>
        public DataSet GetMomOrderDetailsByProductIn(string mocode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("if exists ( select 0 where not object_id('tempdb..#STRefIDs') is null ) drop table #STRefIDs ");
            sb.AppendLine();
            sb.Append("select IDENTITY(int) as tmpId,convert(int,0)  as M_ID ,convert(int,0) as   S_ID,convert(money,ufts) as oriufts,convert(smallint ,0) as mo_Status,MoCode as m_MoCode ,convert(bit,1) as m_bp into #STRefIDs from  mom_order where 1=0 ");
            sb.AppendLine();
            sb.Append(@"insert into #STRefIDs(M_ID ,S_ID,oriufts,mo_Status,m_MoCode,m_bp) select MoId,MoDId,convert(money,ufts),Status,
 MoCode,byproductflag FROM  v_st_mom_orderdetail with (nolock)   Where ( (1>0) AND (1>0) ) AND ( 1>0 ) and 
  (case when ISNULL(SfcFlag,0)=1 then iquantity+QualifiedInQty else 1 end)<>0  and ISNULL(QCFLAG,0)=0 and 
  ISNULL(Status,0) = 3 and (ISNULL(iQuantity,0))>0  and   ( isnull(bWorkshopTrans,0) = 0 or (isnull(bWorkshopTrans,0) =1
   and isnull(sfcflag,0) =1))  and isnull(MrpQty,0)<>0 ");
            sb.AppendLine();
//            sb.Append(@"select soseq,'' as selcol,wccode,wcname,whcode,whname,Dept,DeptName,invcode,invaddcode,invname,invstd,invdefine1,invdefine2,invdefine3,invdefine4,(ftransqty) 
//as ftransqty,invdefine5,(ftransnum) as ftransnum,invdefine6,invdefine7,invdefine8,invdefine9,invdefine10,invdefine11,invdefine12,invdefine13,invdefine14,
//invdefine15,invdefine16,unitcode,unitname,BomQty,free1,free2,free3,free4,free5,free6,free7,free8,free9,free10,LotNo,DemDate,Qty,((isnull(IssQty,0)-isnull(ReplenishQty,0))) 
//as IssQty,((ISNULL(Qty,0)-ISNULL(IssQty,0)+isnull(ReplenishQty,0))) as IsnQty,OpSeq,OpDecs,MoDId,AllocateId,ufts,WIPType,cassunit,cinva_unit,iinvexchrate,
//itnum,isnum,iunnum,CostItemCode,CostItemName,(convert(decimal(38,6),Null)) as fstockanum,(convert(decimal(38,6),Null)) as fstockaqty,(convert(decimal(38,6),Null))
// as fstocknum,(convert(decimal(38,6),Null)) as fstockqty,ReplenishQty,define22,define23,define24,define25,define26,define27,define28,define29,define30,define31,
// define32,define33,define34,define35,define36,define37,istsodid,istsotype,crejectcode,cCiqBookCode,cbatchproperty1,cbatchproperty2,cbatchproperty3,cbatchproperty4,
// cbatchproperty5,cbatchproperty6,cbatchproperty7,cbatchproperty8,cbatchproperty9,cbatchproperty10,cbmemo FROM v_st_moallocate with (nolock)  
//  inner join #STRefIDs b on v_st_moallocate.modid=b.m_id and v_st_moallocate.allocateid=b.s_id where b.m_id in (
//  select MoDId from v_st_mom_orderdetail where  MoCode='" + mocode + "')");
            sb.Append(@"select '' AS SELCOL,SOTYPE,MDEPT,MOLOTCODE,MOSEQ,INVCODE,INVADDCODE,
INVNAME,INVSTD,INVDEFINE1,INVDEFINE2,INVDEFINE3,INVDEFINE4,INVDEFINE5,INVDEFINE6,INVDEFINE7,
INVDEFINE8,INVDEFINE9,INVDEFINE10,INVDEFINE11,INVDEFINE12,INVDEFINE13,INVDEFINE14,INVDEFINE15,
INVDEFINE16,UNITID,UNITCODE,AUXUNITID,AUXUNITCODE,FREE1,FREE2,FREE3,FREE4,FREE5,FREE6,FREE7,
FREE8,FREE9,FREE10,QTY,QUALIFIEDINQTY,DEPTNAME,STARTDATE,DUEDATE,SOCODE,SOSEQ,OPSEQ,DESCRIPTION,
WCCODE,WCNAME,WHCODE,WHNAME,SODID,MODID,UFTS,BYPRODUCTFLAG,DEFINE22,DEFINE23,DEFINE24,DEFINE25,DEFINE26,
DEFINE27,DEFINE28,DEFINE29,DEFINE30,DEFINE31,DEFINE32,DEFINE33,DEFINE34,DEFINE35,DEFINE36,DEFINE37,AUXQTY,
AUXQUALIFIEDINQTY,CHANGERATE,MOID,ORDERCODE,ORDERDID,ORDERSEQ,ORDERTYPE,CCIQBOOKCODE,CUSCODE,CUSNAME,
CUSABBNAME,CSERVICECODE,MOTYPECODE,MOTYPEDESC,COMPLETEQTY AS BALQUALIFIEDQTY,CBMEMO,LPLANCODE,CFACTORYCODE,
CFACTORYNAME FROM v_st_mom_orderdetail with (nolock)   
inner join #STRefIDs b on v_st_mom_orderdetail.Status =b.mo_Status and 
v_st_mom_orderdetail.MoId=b.m_id and v_st_mom_orderdetail.MoDId=b.s_id 
  where  (isnull(v_st_mom_orderdetail.SourceQCVouchType,0) <>1 or byproductflag=1)
and m_id in (select MoId from mom_order where MoCode='"+mocode+"') and v_st_mom_orderdetail.byproductflag =b.m_bp ");
            DataSet ds = SqlHelper.ExcuteDataSet(sb.ToString());
            return ds;
        }
    }
}
