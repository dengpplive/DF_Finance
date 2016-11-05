using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DF.Finance.WebCommon
{
    /// <summary>
    /// 处理model null 问题 
    /// 2016-05-17 jim
    /// </summary>
    public class EmptyStringDataAnnotationsModelMetadataProvider : System.Web.Mvc.DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var modelMetadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            modelMetadata.ConvertEmptyStringToNull = false;
            return modelMetadata;
        }
    }
}
