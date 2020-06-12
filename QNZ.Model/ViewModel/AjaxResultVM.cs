using QNZ.Data.Enums;

using QNZ.Resources.Admin;

namespace QNZ.Model.Admin.ViewModel
{

    public class AjaxResultVM
    {
        public AjaxResultVM()
        {
            Status = Status.Success;
            Message = Messages.AlertActionSuccess;
        }
        public Status Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public object Id { get; set; }



    }

    public static class AjaxResultExtension{
        public static AjaxResultVM SetInfo(this AjaxResultVM AR, string Message)
        {
            AR.Message = Message;
            AR.Status = Status.Info;
            return AR;
        }
        public static AjaxResultVM SetWarning(this AjaxResultVM AR, string Message)
        {
            AR.Message = Message;
            AR.Status = Status.Warning;
            return AR;
        }
        public static AjaxResultVM SetSuccess(this AjaxResultVM AR, string Message)
        {
            AR.Message = Message;
            AR.Status = Status.Success;
            return AR;
        }
        public static AjaxResultVM SetSuccess(this AjaxResultVM AR, string Message,object Data)
        {
            AR.Message = Message;
            AR.Status = Status.Success;
            AR.Data = Data;
            return AR;
        }

        public static AjaxResultVM Setfailure(this AjaxResultVM AR, string Message)
        {
            AR.Message = Message;
            AR.Status = Status.Error;
            return AR;
        }
    }
}
