
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ui.Controllers
{
    public class BaseController : Controller
    {
        public void MessageCreate(string header, string body, Type type, WindowType windowType)
        {
            Message message = new Message();
            message.Header = header;
            message.Body = body;
            message.Type = type;
            if (windowType == WindowType.toastr)
            {
                message.WindowType = windowType;
            }
            else
            {
                message.WindowType=windowType;
            }
            TempData["Message"] = JsonConvert.SerializeObject(message);
        }
        ////////////    Messge Class    ///////////////
        public class Message
        {
            public string Header { get; set; }
            public string Body { get; set; }
            public Type Type { get; set; }
            public WindowType WindowType { get; set; }
        }
        ////////////    Error Window    ///////////////
        public enum WindowType
        {
            toastr,
            sweet
        }
        ////////////    Error Type    ///////////////
        public enum Type
        {
            success,
            error,
            info,
        }
    }
}
