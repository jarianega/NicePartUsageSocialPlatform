using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NicePartUsageSocialPlatform;

public class JsonFormDataModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            return;
        }

        var value = valueProviderResult.FirstValue;
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        try
        {
            var result = JsonSerializer.Deserialize(
                value, 
                bindingContext.ModelType, 
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        catch (JsonException)
        {
            bindingContext.Result = ModelBindingResult.Failed();
        }
    }
}
