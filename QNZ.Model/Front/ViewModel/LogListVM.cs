
using System;
using System.Collections.Generic;
using System.Text;

namespace QNZ.Model.Front.ViewModel
{
    public  class LogListVM
    {
        public string Month { get; set; }
        public List<LogVM> Logs { get; set; }
    }

    public class LogVM
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string EquipmentId { get; set; }
       
    }
    public class ReportListVM
    {
        public string Month { get; set; }
        public List<ReportVM> Logs { get; set; }
    }
    public class ReportVM
    {

        public string CreatedDate => string.Format("{0}-{1}-{2} {3}:{4}", MEASUYY, MEASUMM, MEASUDD, MEASUHR, MEASUMN);
        public string EquipmentId { get; set; }
        public string MEASUYY { get; set; }
        public string MEASUMM { get; set; }
        public string MEASUDD { get; set; }
        public string MEASUHR { get; set; }
        public string MEASUMN { get; set; }

        public int MEASUYYint => Int32.Parse(MEASUYY);
        public int MEASUMMint => Int32.Parse(MEASUMM);
        public int MEASUDDint => Int32.Parse(MEASUDD);
        public int MEASUHRint => Int32.Parse(MEASUHR);
        public int MEASUMNint => Int32.Parse(MEASUMN);
    }


    //public class PageReportVM {

    //    public ReportData ReportData { get; set; }
    //    public List<ChartProductVM> Products { get; set; }
    //    public int[] Ages { get; set; }
    //    //毛孔
    //   public string PoresImg1BMP => string.Format("/Uploads/reports/{0}/{1}-{2}-F_FM_PW.BMP",this.ReportData.EquipmentId, this.ReportData.SERIAL, ReportData.MEASUNO);
    //    /// <summary>
    //    /// 照片 - 毛孔与皱纹共用
    //    /// </summary>
    //    public string PoresImg1 => string.Format("/Uploads/reports/{0}/{1}-{2}-F_FM_PW.jpg", this.ReportData.EquipmentId, this.ReportData.SERIAL, ReportData.MEASUNO);
    //    /// <summary>
    //    /// 人脸线描图 女用  毛孔与卡啉共用
    //    /// </summary>
    //    public string PoresImg2_woman => "/Images/Rv_Pore_Woman.jpg";
    //    /// <summary>
    //    /// 人脸线描图 男用  毛孔与卡啉共用
    //    /// </summary>
    //    public string PoresImg2_man => "/Images/Rv_Pore_Man.jpg";
    //    //public byte PoresValue => this.ReportData.N_PR;
    //    public int[] Pores { get; set; }
    //    public ZB[] Pores1 { get; set; }
    //    //皱纹      
    //    /// <summary>
    //    /// 人脸线描图 女用
    //    /// </summary>
    //    public string WrinklesImg2_woman => "/Images/Rv_Wr_Woman.jpg";
    //    /// <summary>
    //    /// 人脸线描图 男用
    //    /// </summary>
    //    public string WrinklesImg2_man => "/Images/Rv_Wr_Man.jpg";
    //    //public byte WrinklesValue => this.ReportData.N_WR;
    //    public int[] Wrinkles { get; set; }
    //    public ZB[] Wrinkles1 { get; set; }
    //    //表皮层
    //    public string EpidermisImg1BMP => string.Format("/Uploads/reports/{0}/{1}-{2}-F_FM_PF.BMP", this.ReportData.EquipmentId, this.ReportData.SERIAL, ReportData.MEASUNO);

    //    /// <summary>
    //    /// 表皮层照片
    //    /// </summary>
    //    public string EpidermisImg1 => string.Format("/Uploads/reports/{0}/{1}-{2}-F_FM_PF.jpg", this.ReportData.EquipmentId, this.ReportData.SERIAL, ReportData.MEASUNO);

    //    /// <summary>
    //    /// 表皮层与真皮层人脸线描图 女用
    //    /// </summary>
    //    public string EpidermisImg2_wonman => "/Images/Rv_Spot_Woman.jpg";
    //    /// <summary>
    //    /// 表皮层与真皮层人脸线描图 男用
    //    /// </summary>
    //    public string EpidermisImg2_man => "/Images/Rv_Spot_man.jpg";
    //    //public byte EpidermisValue => this.ReportData.SP_PL_9;
    //    public int[] Epidermis { get; set; }
    //    public ZB[] Epidermis1 { get; set; }
    //    //真皮层
    //    public string DermisImg1BMP => string.Format("/Uploads/reports/{0}/{1}-{2}-F_FM_UF.BMP", this.ReportData.EquipmentId, this.ReportData.SERIAL, ReportData.MEASUNO);
    //    /// <summary>
    //    /// 真皮层照片
    //    /// </summary>
    //    public string DermisImg1 => string.Format("/Uploads/reports/{0}/{1}-{2}-F_FM_UF.jpg", this.ReportData.EquipmentId, this.ReportData.SERIAL, ReportData.MEASUNO);

    //    //public byte DermisValue => this.ReportData.SP_UV_9;
    //    public int[] Dermis { get; set; }
    //    public ZB[] Dermis1 { get; set; }
    //    //卟啉

    //    public string PorphyrinImg1BMP => string.Format("/Uploads/reports/{0}/{1}-{2}-F_FM_UV.BMP", this.ReportData.EquipmentId, this.ReportData.SERIAL, ReportData.MEASUNO);
    //    /// <summary>
    //    /// 卟啉照片
    //    /// </summary>
    //    public string PorphyrinImg1 => string.Format("/Uploads/reports/{0}/{1}-{2}-F_FM_UV.jpg", this.ReportData.EquipmentId, this.ReportData.SERIAL, ReportData.MEASUNO);

    //    //public byte PorphyrinsValue => this.ReportData.N_PP;
    //    public int[] Porphyrins { get; set; }
    //    public ZB[] Porphyrins1 { get; set; }
    //}

    //public class ZB
    //{
    //    public int x { get; set; }
    //    public int y { get; set; }
    //}

    //public class ChartProductVM
    //{
    //    //public long Id { get; set; }
    //    public string PRODUCT { get; set; }      
    //    public string IMAGE { get; set; }
    //    public string EquipmentId { get; set; }      
    //    public string ImageFullPath => $"https://xcx.anyacos.com/Uploads/reports/Products/{EquipmentId}/{IMAGE}";
    //}
}
