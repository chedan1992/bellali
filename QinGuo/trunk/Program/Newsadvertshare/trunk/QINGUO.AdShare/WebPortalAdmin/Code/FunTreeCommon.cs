using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Collections;
using QINGUO.ViewModel;
using QINGUO.Business;
using QINGUO.Model;

namespace WebPortalAdmin.Code
{
    public class FunTreeCommon
    {
        #region ===权限分配树查询过滤 GetSearchTreeNodes

        public DataSet ClearSame(DataSet ds)
        {
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "CreateTime asc";
            DataTable dvv = dv.ToTable();

            for (int i = 0; i < dvv.Rows.Count; i++)
            {
                dv.RowFilter = "ParentID='" + dvv.Rows[i]["id"].ToString() + "'";
                DataTable childDiv = dv.ToTable();
                if (childDiv.Rows.Count > 0)
                {
                    for (int j = 0; j < childDiv.Rows.Count; j++)
                    {
                        for (int a = 0; a < ds.Tables[0].Rows.Count;a++)
                        {
                            if (childDiv.Rows[j]["id"] == ds.Tables[0].Rows[a]["id"])
                            {
                                ds.Tables[0].Rows[a].Delete();
                                ds.Tables[0].AcceptChanges();//提交改变值
                                break;
                            }
                        }
                    }
                   
                }
            }
            return ds;
        }

        /// <summary>
        /// 首页导航树,权限分配树查询过滤 GetSearchTreeNodes
        /// </summary>
        /// <param name="parDs"></param>
        /// <returns></returns>
        public List<JsonFunTree> GetSearchTreeNodes(DataSet parDs)
        {
            DataSet ds = ClearSame(parDs);
            List<JsonFunTree> list = new List<JsonFunTree>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int b = 0; b < ds.Tables[0].Rows.Count; b++)
                {
                    JsonFunTree model = new JsonFunTree();
                    model.id = ds.Tables[0].Rows[b]["id"].ToString();
                    model.parentId = ds.Tables[0].Rows[b]["parentId"].ToString();
                    model.text = ds.Tables[0].Rows[b]["text"].ToString();
                    model.iconCls = ds.Tables[0].Rows[b]["iconCls"].ToString();
                    model.pageUrl = ds.Tables[0].Rows[b]["PageUrl"].ToString();
                    model.className = ds.Tables[0].Rows[b]["ClassName"].ToString();
                    model.funSort = Convert.ToInt32(ds.Tables[0].Rows[b]["FunSort"]);
                    model.expanded = true;
                    model.IsSystem = Convert.ToBoolean(ds.Tables[0].Rows[b]["IsSystem"]); //是否系统定义
                    model.CreateTime = Convert.ToDateTime(ds.Tables[0].Rows[b]["CreateTime"]);
                    model.leaf = true;

                    model.children = GetSearchChildNodes(ds.Tables[0].Rows[b]["id"].ToString());

                    if (model.children.Count > 0)
                    {
                        model.leaf = false;
                    }
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public List<JsonFunTree> GetSearchChildNodes(string ParentID)
        {
            List<JsonFunTree> list = new List<JsonFunTree>();
            DataSet ds = new BllSysFun().GetTreeList("ParentID='" + ParentID + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int b = 0; b < ds.Tables[0].Rows.Count; b++)
                {
                    JsonFunTree model = new JsonFunTree();
                    model.id = ds.Tables[0].Rows[b]["id"].ToString();
                    model.parentId = ds.Tables[0].Rows[b]["parentId"].ToString();
                    model.text = ds.Tables[0].Rows[b]["text"].ToString();
                    model.iconCls = ds.Tables[0].Rows[b]["iconCls"].ToString();
                    model.pageUrl = ds.Tables[0].Rows[b]["PageUrl"].ToString();
                    model.className = ds.Tables[0].Rows[b]["ClassName"].ToString();
                    model.funSort = Convert.ToInt32(ds.Tables[0].Rows[b]["FunSort"]);
                    model.expanded = true;
                    model.IsSystem = Convert.ToBoolean(ds.Tables[0].Rows[b]["IsSystem"]); //是否系统定义
                    model.CreateTime = Convert.ToDateTime(ds.Tables[0].Rows[b]["CreateTime"]);
                    model.leaf = true;

                    model.children = GetSearchChildNodes(ds.Tables[0].Rows[b]["id"].ToString());

                    if (model.children.Count > 0)
                    {
                        model.leaf = false;
                    }
                    
                    list.Add(model);
                }
            }
            return list;
        }

        #endregion

        #region ===公司树 GetCompanyTreeNodes| GetCompanyChildNodes
        /// <summary>
        /// 根据全部查询获取树形递归
        /// </summary>
        /// <param name="parDs"></param>
        /// <returns></returns>
        public List<JsonCompanyTree> GetCompanyTreeNodes(DataSet parDs)
        {
            DataView dv = parDs.Tables[0].DefaultView;
            dv.RowFilter = "CreateCompanyId='0'";//树形默认根节点0
            DataTable ds = dv.ToTable();

            List<JsonCompanyTree> list = new List<JsonCompanyTree>();
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                JsonCompanyTree model = new JsonCompanyTree();
                model.id = ds.Rows[i]["Id"].ToString();
                model.parentId = ds.Rows[i]["CreateCompanyId"].ToString();
                model.text = ds.Rows[i]["Name"].ToString();
                switch (int.Parse(ds.Rows[i]["Attribute"].ToString()))//1:集团 2:公司 3:分公司 4:子公司 5:岗位
                {
                    case 0:
                        model.iconCls = "";
                        break;
                    case 1:
                        model.iconCls = "GTP_home";
                        break;
                    case 2:
                        model.iconCls = "GTP_dept";
                        break;
                    case 3:
                        model.iconCls = "GTP_post";
                        break;
                    case 4:
                        model.iconCls = "GTP_post";
                        break;
                    case 5:
                        model.iconCls = "GTP_post";
                        break;
                }
                model.Status = int.Parse(ds.Rows[i]["Status"].ToString());
                model.expanded = false;
                model.CreateTime = Convert.ToDateTime(ds.Rows[i]["CreateTime"]);
                model.leaf = true;
                model.Code = ds.Rows[i]["Code"].ToString();
                model.LinkUser = ds.Rows[i]["LinkUser"].ToString();
                model.Phone = ds.Rows[i]["Phone"].ToString();
                model.NameTitle = ds.Rows[i]["NameTitle"].ToString();
                model.Attribute = int.Parse(ds.Rows[i]["Attribute"].ToString());
                //查询子节点
                dv.RowFilter = "CreateCompanyId='" + ds.Rows[i]["Id"].ToString() + "'";
                DataTable childSet = dv.ToTable();
                if (childSet.Rows.Count > 0)
                {
                    model.leaf = false;
                    model.children = GetCompanyChildNodes(parDs, childSet);
                }
                list.Add(model);
            }
            return list;
        }

        //获取根节点
        public List<JsonCompanyTree> GetCompanyChildNodes(DataSet parDs, DataTable ds)
        {
            DataView dv = parDs.Tables[0].DefaultView;

            List<JsonCompanyTree> list = new List<JsonCompanyTree>();
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                JsonCompanyTree model = new JsonCompanyTree();
                model.id = ds.Rows[i]["id"].ToString();
                model.parentId = ds.Rows[i]["CreateCompanyId"].ToString();
                model.text = ds.Rows[i]["Name"].ToString();
                switch (int.Parse(ds.Rows[i]["Attribute"].ToString()))//1:集团 2:公司 3:分公司 4:子公司
                {
                    case 0:
                        model.iconCls = "";
                        break;
                    case 1:
                        model.iconCls = "GTP_home";
                        break;
                    case 2:
                        model.iconCls = "GTP_dept";
                        break;
                    case 3:
                        model.iconCls = "GTP_post";
                        break;
                    case 4:
                        model.iconCls = "GTP_post";
                        break;
                    case 5:
                        model.iconCls = "GTP_post";
                        break;
                }
                model.Status = int.Parse(ds.Rows[i]["Status"].ToString());
                model.expanded = false;
                model.leaf = true;
                model.Code = ds.Rows[i]["Code"].ToString();
                model.LinkUser = ds.Rows[i]["LinkUser"].ToString();
                model.Phone = ds.Rows[i]["Phone"].ToString();
                model.CreateTime = Convert.ToDateTime(ds.Rows[i]["CreateTime"]);
                model.NameTitle = ds.Rows[i]["NameTitle"].ToString();
                model.Attribute = int.Parse(ds.Rows[i]["Attribute"].ToString());
                //查询子节点
                dv.RowFilter = "CreateCompanyId='" + ds.Rows[i]["id"].ToString() + "'";
                DataTable childSet = dv.ToTable();
                if (childSet.Rows.Count > 0)
                {
                    model.leaf = false;
                    model.children = GetCompanyChildNodes(parDs, childSet);
                }
                list.Add(model);
            }
            return list;
        }

        #endregion

        #region ===首页导航树 GetFunTreeNodes| GetChildNodes
        /// <summary>
        /// 根据全部查询获取树形递归
        /// </summary>
        /// <param name="parDs"></param>
        /// <returns></returns>
        public List<JsonFunTree> GetFunTreeNodes(DataSet parDs)
        {
            DataView dv = parDs.Tables[0].DefaultView;
            dv.RowFilter = "ParentID='0'";//树形默认根节点0
            DataTable ds = dv.ToTable();

            List<JsonFunTree> list = new List<JsonFunTree>();
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                JsonFunTree model = new JsonFunTree();
                model.id = ds.Rows[i]["id"].ToString();
                model.parentId = ds.Rows[i]["parentId"].ToString();
                model.text = ds.Rows[i]["text"].ToString();
                model.iconCls = ds.Rows[i]["iconCls"].ToString();
                model.pageUrl = ds.Rows[i]["PageUrl"].ToString();
                model.TypeId = int.Parse(ds.Rows[i]["TypeID"].ToString());
                model.Status = int.Parse(ds.Rows[i]["Status"].ToString());
                model.className = ds.Rows[i]["ClassName"].ToString();
                model.funSort = Convert.ToInt32(ds.Rows[i]["FunSort"]);
                model.expanded = false;
                model.IsSystem = Convert.ToBoolean(ds.Rows[i]["IsSystem"]);//是否系统定义
                model.CreateTime = Convert.ToDateTime(ds.Rows[i]["CreateTime"]);
                model.leaf = true;
                //查询子节点
                dv.RowFilter = "ParentID='" + ds.Rows[i]["id"].ToString() + "'";
                DataTable childSet = dv.ToTable();
                if (childSet.Rows.Count > 0)
                {
                    model.leaf = false;
                    model.children = GetChildNodes(parDs, childSet);
                }
                list.Add(model);
            }
            return list;
        }

        //获取根节点
        public List<JsonFunTree> GetChildNodes(DataSet parDs, DataTable ds)
        {
            DataView dv = parDs.Tables[0].DefaultView;

            List<JsonFunTree> list = new List<JsonFunTree>();
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                JsonFunTree model = new JsonFunTree();
                model.id = ds.Rows[i]["id"].ToString();
                model.parentId = ds.Rows[i]["parentId"].ToString();
                model.text = ds.Rows[i]["text"].ToString();
                model.iconCls = ds.Rows[i]["iconCls"].ToString();
                model.pageUrl = ds.Rows[i]["PageUrl"].ToString();
                model.TypeId = int.Parse(ds.Rows[i]["TypeID"].ToString());
                model.Status = int.Parse(ds.Rows[i]["Status"].ToString());
                model.className = ds.Rows[i]["ClassName"].ToString();
                model.funSort = Convert.ToInt32(ds.Rows[i]["FunSort"]);
                model.IsSystem = Convert.ToBoolean(ds.Rows[i]["IsSystem"]);//是否系统定义
                model.expanded = false;
                model.leaf = true;
                model.CreateTime = Convert.ToDateTime(ds.Rows[i]["CreateTime"]);
                //查询子节点
                dv.RowFilter = "ParentID='" + ds.Rows[i]["id"].ToString() + "'";
                DataTable childSet = dv.ToTable();
                if (childSet.Rows.Count > 0)
                {
                    model.leaf = false;
                    model.children = GetChildNodes(parDs, childSet);
                }
                list.Add(model);
            }
            return list;
        }

        #endregion

        #region ===公司选择树 GetTreeByShoper
        /// <summary>
        /// 公司选择树
        /// </summary>
        /// <param name="ds">集合</param>
        /// <returns></returns>
        public List<JsonTree> GetTreeByShoper(DataSet ds)
        {
            List<JsonTree> list = new List<JsonTree>();

            for (int b = 0; b < ds.Tables[0].Rows.Count; b++)
            {
                JsonTree model = new JsonTree();
                model.id = ds.Tables[0].Rows[b]["Id"].ToString();
                model.text = ds.Tables[0].Rows[b]["Name"].ToString();
                model.leaf = true;
                model.expanded = true;

                list.Add(model);
            }
            return list;
        }

        #endregion

        #region ===商品类别导航树 GetTreeByShopCategory
        /// <summary>
        /// 商品类别导航树
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public List<ModJsonShopCategory> GetTreeByShopCategory(DataSet ds)
        {
            List<ModJsonShopCategory> list = new List<ModJsonShopCategory>();

            for (int b = 0; b < ds.Tables[0].Rows.Count; b++)
            {
                ModJsonShopCategory model = new ModJsonShopCategory();
                model.Id = ds.Tables[0].Rows[b]["Id"].ToString();
                model.Name = ds.Tables[0].Rows[b]["Name"].ToString();
                model.OrderNum = ds.Tables[0].Rows[b]["OrderNum"].ToString();
                model.ParentCategoryId = ds.Tables[0].Rows[b]["ParentCategoryId"].ToString();
                model.leaf = (Convert.ToBoolean(ds.Tables[0].Rows[b]["HasChild"].ToString()) == true ? false : true);
                model.IsSystem = Convert.ToBoolean(ds.Tables[0].Rows[b]["IsSystem"].ToString());
                model.Path = ds.Tables[0].Rows[b]["Path"].ToString();
                model.PicUrl = ds.Tables[0].Rows[b]["PicUrl"].ToString();
                model.Remark = ds.Tables[0].Rows[b]["Remark"].ToString();
                model.Status = Convert.ToInt32(ds.Tables[0].Rows[b]["Status"].ToString());
                model.CreateTime = Convert.ToDateTime(ds.Tables[0].Rows[b]["CreateTime"]);
                model.CreaterName = ds.Tables[0].Rows[b]["CreaterName"].ToString();
                list.Add(model);
            }
            return list;
        }


        #endregion

        #region ===分类树 CategoryTreeNodes| CategoryChildNodes
        /// <summary>
        /// 根据全部查询获取树形递归
        /// </summary>
        /// <param name="parDs"></param>
        /// <returns></returns>
        public List<JsonFunTree> CategoryTreeNodes(DataSet parDs)
        {
            DataView dv = parDs.Tables[0].DefaultView;
            dv.RowFilter = "ParentID='0'";//树形默认根节点0
            DataTable ds = dv.ToTable();

            List<JsonFunTree> list = new List<JsonFunTree>();
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                JsonFunTree model = new JsonFunTree();
                model.id = ds.Rows[i]["id"].ToString();
                model.parentId = ds.Rows[i]["parentId"].ToString();
                model.text = ds.Rows[i]["text"].ToString();
                //model.Depth = int.Parse(ds.Rows[i]["Depth"].ToString());
                model.expanded = false;
                model.leaf = true;
                //查询子节点
                dv.RowFilter = "ParentID='" + ds.Rows[i]["id"].ToString() + "'";
                DataTable childSet = dv.ToTable();
                if (childSet.Rows.Count > 0)
                {
                    model.leaf = false;
                    model.children = CategoryChildNodes(parDs, childSet);
                }
                list.Add(model);
            }
            return list;
        }

        //获取根节点
        public List<JsonFunTree> CategoryChildNodes(DataSet parDs, DataTable ds)
        {
            DataView dv = parDs.Tables[0].DefaultView;

            List<JsonFunTree> list = new List<JsonFunTree>();
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                JsonFunTree model = new JsonFunTree();
                model.id = ds.Rows[i]["id"].ToString();
                model.parentId = ds.Rows[i]["parentId"].ToString();
                model.text = ds.Rows[i]["text"].ToString();
                //model.Depth = int.Parse(ds.Rows[i]["Depth"].ToString());
                model.expanded = false;
                model.leaf = true;
                //查询子节点
                dv.RowFilter = "ParentID='" + ds.Rows[i]["id"].ToString() + "'";
                DataTable childSet = dv.ToTable();
                if (childSet.Rows.Count > 0)
                {
                    model.leaf = false;
                    model.children = CategoryChildNodes(parDs, childSet);
                }
                list.Add(model);
            }
            return list;
        }

        #endregion

        #region ===分类类别导航树 GetTreeBySysCategory
        /// <summary>
        /// 类别导航树
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public List<jsonSysCategory> GetTreeBySysCategory(DataSet ds)
        {
            List<jsonSysCategory> list = new List<jsonSysCategory>();

            for (int b = 0; b < ds.Tables[0].Rows.Count; b++)
            {
                jsonSysCategory model = new jsonSysCategory();
                model.Id = ds.Tables[0].Rows[b]["Id"].ToString();
                model.Name = ds.Tables[0].Rows[b]["Name"].ToString();
                model.OrderNum = ds.Tables[0].Rows[b]["OrderNum"].ToString();
                model.ParentCategoryId = ds.Tables[0].Rows[b]["ParentCategoryId"].ToString();
                model.leaf = (Convert.ToBoolean(ds.Tables[0].Rows[b]["HasChild"].ToString()) == true ? false : true);
                model.IsSystem = Convert.ToBoolean(ds.Tables[0].Rows[b]["IsSystem"].ToString());
                model.Path = ds.Tables[0].Rows[b]["Path"].ToString();
                model.PicUrl = ds.Tables[0].Rows[b]["PicUrl"].ToString();
                model.Remark = ds.Tables[0].Rows[b]["Remark"].ToString();
                model.Status = Convert.ToInt32(ds.Tables[0].Rows[b]["Status"].ToString());
                model.CreateTime = Convert.ToDateTime(ds.Tables[0].Rows[b]["CreateTime"]);
                model.CreaterName = ds.Tables[0].Rows[b]["CreaterName"].ToString();
                model.Depth =int.Parse(ds.Tables[0].Rows[b]["Depth"].ToString());
                list.Add(model);
            }
            return list;
        }


        #endregion

        #region ===品牌树 BrandTreeNodes
        /// <summary>
        /// 根据全部查询获取树形递归
        /// </summary>
        /// <param name="parDs"></param>
        /// <returns></returns>
        public List<JsonFunTree> BrandTreeNodes(IList<ModSysQRCode> parDs)
        {
            List<JsonFunTree> list = new List<JsonFunTree>();
            for (int i = 0; i < parDs.Count; i++)
            {
                JsonFunTree model = new JsonFunTree();
                model.id = parDs[i].Id;
                model.parentId ="0";
                model.text = parDs[i].Name;
                model.Depth = 0;
                model.expanded = false;
                model.leaf = true;
                model.leaf = true;
                list.Add(model);
            }
            return list;
        }
        #endregion


        #region ===查询获取树形递归(省市区) GetJsonTree
        /// <summary>
        /// 根据查询获取树形递归(城市地区商圈模块)
        /// </summary>
        /// <param name="parDs"></param>
        /// <returns></returns>
        public List<jsonSysBusinessCircle> GetJsonTree(DataSet parDs)
        {
            List<jsonSysBusinessCircle> list = new List<jsonSysBusinessCircle>();
            if (parDs != null)
            {
                for (int b = 0; b < parDs.Tables[0].Rows.Count; b++)
                {
                    jsonSysBusinessCircle model = new jsonSysBusinessCircle();
                    model.id = parDs.Tables[0].Rows[b]["id"].ToString();
                    model.text = parDs.Tables[0].Rows[b]["text"].ToString();
                    model.Code = parDs.Tables[0].Rows[b]["Code"].ToString();
                    model.expanded = true;
                    model.leaf = true;
                    model.children = null;
                    list.Add(model);
                }
            }
            return list;
        }
        #endregion

        #region ===分类 GetCategoryNodes| GetChildCategoryNodes
        /// <summary>
        /// 根据全部查询获取树形递归
        /// </summary>
        /// <param name="parDs"></param>
        /// <returns></returns>
        public List<JsonFunTree> GetCategoryNodes(DataSet parDs)
        {
            DataView dv = parDs.Tables[0].DefaultView;
            dv.RowFilter = "parentId='0'";//树形默认根节点0
            DataTable ds = dv.ToTable();
            List<JsonFunTree> list = new List<JsonFunTree>();
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                JsonFunTree model = new JsonFunTree();
                model.id = ds.Rows[i]["id"].ToString();
                model.parentId = ds.Rows[i]["parentId"].ToString();
                model.text = ds.Rows[i]["text"].ToString();
                model.expanded = false;
                model.leaf = true;
                //查询子节点
                dv.RowFilter = "parentId='" + ds.Rows[i]["id"].ToString() + "'";
                DataTable childSet = dv.ToTable();
                if (childSet.Rows.Count > 0)
                {
                    model.leaf = false;
                    model.children = GetChildCategoryNodes(parDs, childSet);
                }
                list.Add(model);
            }
            return list;
        }

        //获取根节点
        public List<JsonFunTree> GetChildCategoryNodes(DataSet parDs, DataTable ds)
        {
            DataView dv = parDs.Tables[0].DefaultView;

            List<JsonFunTree> list = new List<JsonFunTree>();
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                JsonFunTree model = new JsonFunTree();
                model.id = ds.Rows[i]["id"].ToString();
                model.parentId = ds.Rows[i]["parentId"].ToString();
                model.text = ds.Rows[i]["text"].ToString();
                model.expanded = false;
                model.leaf = true;
                //查询子节点
                dv.RowFilter = "parentId='" + ds.Rows[i]["id"].ToString() + "'";
                DataTable childSet = dv.ToTable();
                if (childSet.Rows.Count > 0)
                {
                    model.leaf = false;
                    model.children = GetChildCategoryNodes(parDs, childSet);
                }
                list.Add(model);
            }
            return list;
        }

        #endregion 
    }
}