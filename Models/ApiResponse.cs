namespace ReviseDotnet.Models;

/// <summary>
/// Every response from API will send in this format
/// Controller will not use this class
/// CustomResponseMiddleware will use ApiResponse
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Actual Status Code of Response
    /// </summary>
    /// <value></value>
    public int Status { get; set; }
    /// <summary>
    /// Actual Response Body
    /// </summary>
    /// <value></value>
    public T Data { get; set; }
    /// <summary>
    /// Success and error messages
    /// </summary>
    /// <value></value>
    public List<string> Messages { get; set; }
    /// <summary>
    /// Error Code for error
    /// frontend can use errorCode to show error message for mulltilanguge app
    /// or can send to analytics
    /// </summary>
    /// <value></value>
    public int ErrorCode { get; set; }
}