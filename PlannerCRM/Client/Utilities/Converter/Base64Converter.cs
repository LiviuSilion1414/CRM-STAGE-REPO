using Microsoft.AspNetCore.Components.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace PlannerCRM.Client.Utilities.Converter;

public class Base64Converter
{
    public async Task<(string thumbnail, string imageType)> ConvertImageAsync(InputFileChangeEventArgs args)
    {
        var imgFile = args.File;
        var buffers = new byte[imgFile.Size];

        await imgFile.OpenReadStream().ReadAsync(buffers);

        var imageType = imgFile.ContentType;
        var thumbnail = $"data:{imageType};base64,{Convert.ToBase64string(buffers)}";

        return (thumbnail, imageType);
    }
}