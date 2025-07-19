namespace Ioc.ObjModels.Model.CommonModel
{
    public class ResponseModel
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
